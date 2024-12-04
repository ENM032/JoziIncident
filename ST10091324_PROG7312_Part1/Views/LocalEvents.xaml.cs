using ST10091324_PROG7312_Part1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for LocalEvents.xaml
    /// </summary>
    public partial class LocalEvents : Page, INotifyPropertyChanged
    {
        // Boolean property to control the visibility of recommendations
        private bool _isRecommendationsVisible;
        
        // Event for property change notifications
        public event PropertyChangedEventHandler PropertyChanged;

        // A sorted dictionary to store events categorized by date
        private SortedDictionary<DateTime, List<LocalEvent>> EventsByDate { get; set; } = new SortedDictionary<DateTime, List<LocalEvent>>();

        // Observable collection for events displayed in the UI
        public ObservableCollection<LocalEvent> DisplayedEvents { get; set; } = new ObservableCollection<LocalEvent>();

        // Queue to manage events for display
        private Queue<LocalEvent> EventQueue { get; set; } = new Queue<LocalEvent>();

        // HashSet to store unique event categories
        private HashSet<string> UniqueCategories { get; set; } = new HashSet<string>();

        // Dictionary to track user preferences for categories
        private Dictionary<string, int> CategoryPreference { get; set; } = new Dictionary<string, int>();

        // Dictionary to count how many times each event has been viewed
        private Dictionary<string, int> EventViewCount { get; set; } = new Dictionary<string, int>();

        private Dictionary<DateTime, int> DateSearchCount = new Dictionary<DateTime, int>();


        // Bool variable to determine if recommendations should display
        private bool filtersApplied = false;

        // Constructor initializes the page and loads events
        public LocalEvents()
        {
            // This code was taken from a StackOverflow post
            // Posted by: Nehorai Elbaz
            // Available at: https://stackoverflow.com/questions/58048031/how-to-access-a-mainwindow-variable-from-a-page-in-c-sharp-wpf
            // Hide search grid in main window
            //((MainWindow)App.Current.Windows[0]).HideSearchGrid();

            InitializeComponent();
           
            // This code below was taken from a blog post
            // Uploaded by: WPF Tutorial
            // Available at: https://wpf-tutorial.com/data-binding/using-the-datacontext/#google_vignette
            // Accessed: 10 October 2024

            // Set the DataContext to the current instance
            DataContext = this;

            // Load events asynchronously
            LoadEventsAsync();
        }

        // The method below was taken from a StackOverflow post
        // Posted by: Styxxy
        // Available at: https://stackoverflow.com/questions/12034840/handling-onpropertychanged

        // Method to raise property change notifications
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // The method below was taken from a StackOverflow post
        // Posted by: Styxxy
        // Available at: https://stackoverflow.com/questions/12034840/handling-onpropertychanged
        public bool IsRecommendationsVisible
        {
            get => _isRecommendationsVisible;
            set
            {
                if (_isRecommendationsVisible != value)
                {
                    _isRecommendationsVisible = value;
                    // Notify that the property has changed
                    OnPropertyChanged(); 
                }
            }
        }

        // Asynchronously load events into the application
        private async void LoadEventsAsync()
        {
            // The async task run code was taken from a StackOverflow post
            // Posted by: Robin
            // Available at: https://stackoverflow.com/questions/50004373/understanding-async-await-and-task-run
            await Task.Run(() =>
            {
                // The localevent population process was taken from a blog post
                // Titled: LINQ SelectMany Method in C#
                // Uploaded by: Rout
                // Available at: https://dotnettutorials.net/lesson/selectmany-in-linq/
                // Access: 10 October 2024
                var loadedEvents = new[]
                {
                    // Made up hardcoded events Hardcoded events
                    new LocalEvent { Title = "Powerstation Shutdown", Description = "We unfortunately have to shut down the powerstation in Woodmead, Midrand.", Date = DateTime.Now.AddDays(9), ImageUrl = "/Resources/shutdown.jpg", Category = "Government" },
                    new LocalEvent { Title = "Dam Repairs", Description = "The constant heavy rainfall has caused our dam to fill up and cause visible damage to the wall lining.", Date = DateTime.Now.AddDays(14), ImageUrl = "/Resources/damn.jpg", Category = "Government" },
                    new LocalEvent { Title = "City Council Meeting", Description = "Attend the monthly city council meeting to discuss community issues.", Date = DateTime.Now.AddDays(5), ImageUrl = "/Resources/meeting.jpg", Category = "Community" },
                    new LocalEvent { Title = "Community Clean-Up Day", Description = "Join us for a community clean-up to beautify our local parks.", Date = DateTime.Now.AddDays(13), ImageUrl = "/Resources/cleanup.jpg", Category = "Community" },
                    new LocalEvent { Title = "Public Library Book Fair", Description = "Join us for a book fair at the local library with various activities for all ages.", Date = DateTime.Now.AddDays(27), ImageUrl = "/Resources/library.jpg", Category = "Public" },
                    new LocalEvent { Title = "Elderly Rates and Taxes Discount", Description = "Join us in honoring our veterans at the annual ceremony.", Date = DateTime.Now.AddDays(34), ImageUrl = "/Resources/deduction.jpg", Category = "Public" }
                };

                // Populate the SortedDictionary and HashSets
                foreach (var ev in loadedEvents)
                {
                    // The code below was taken from a StackOverflow post
                    // Posted by: Mark Byers
                    // Available at: https://stackoverflow.com/questions/2829873/how-can-i-detect-if-this-dictionary-key-exists-in-c
                    if (!EventsByDate.ContainsKey(ev.Date.Date))
                    {
                        EventsByDate[ev.Date.Date] = new List<LocalEvent>();
                    }
                    EventsByDate[ev.Date.Date].Add(ev);

                    // Add unique categories and dates
                    UniqueCategories.Add(ev.Category);
                }

                // Update the UI on the main thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    populateComboBox();

                    DisplayEvents();

                    ShowRandomRecommendations();
                });
            });
        }

        // Populate the ComboBox with unique categories from dataset
        private void populateComboBox()
        {
            CategoryComboBox.Items.Clear();

            // The code to set the combobox itemsource was taken from StackOverflow
            // Author: Allen Rice
            // Link: https://stackoverflow.com/questions/2417960/populating-a-combobox-using-c-sharp
            foreach (var category in UniqueCategories)
            {
                CategoryComboBox.Items.Add(new ComboBoxItem { Content = category });
                // Initialize each category preference
                CategoryPreference[category] = 0; 
                
            }
        }

        // Display events in the ItemsControl
        private void DisplayEvents()
        {
            DisplayedEvents.Clear();

            // The foreach code below was taken from a StackOverflow
            // Posted by: Mark Cooper
            // Available at: https://stackoverflow.com/questions/141088/how-to-iterate-over-a-dictionary
            foreach (var dateEvents in EventsByDate)
            {
                foreach (var ev in dateEvents.Value)
                {
                    // Add events to the displayed list
                    DisplayedEvents.Add(ev);
                }
            }

            // Update the ItemsControl
            EventsList.ItemsSource = DisplayedEvents;
        }

        // Show recommended events based on user preferences
        private void ShowRecommendations()
        {

            if (filtersApplied)
            {
                // If filters are applied, do not show recommendations
                RecommendationsList.ItemsSource = null;
                IsRecommendationsVisible = false;
                return;
            }

            // List to store recommendations
            var recommendedEvents = new List<LocalEvent>();

            // Sort categories by user preference
            var sortedCategories = CategoryPreference.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            // Sort search dates by frequency (highest first)
            var sortedDates = DateSearchCount.OrderByDescending(x => x.Value)
                                             .Select(x => x.Key)
                                             .ToList();

            // Only consider dates searched more than 3 times)
            const int popularityThreshold = 3;

            // Filter dates that have been searched more than the threshold
            var popularDates = sortedDates.Where(date => DateSearchCount[date] > popularityThreshold).ToList();

            // The foreach loop code below was taken from a blog post
            // Titled: LINQ SelectMany Method in C#
            // Uploaded by: Rout
            // Available at: https://dotnettutorials.net/lesson/selectmany-in-linq/
            // Access: 10 October 2024
            foreach (var category in sortedCategories)
            {
                var eventsInCategory = EventsByDate.SelectMany(kvp => kvp.Value)
                                                    .Where(e => e.Category == category)
                                                    .ToList();
                // Add events from the preferred categories
                recommendedEvents.AddRange(eventsInCategory);
            }

            // Prioritize events that match frequently searched dates
            foreach (var searchDate in popularDates)
            {
                // Add events that occur on this frequently searched date
                var eventsOnSearchDate = EventsByDate.Where(kvp => kvp.Key.Date == searchDate)
                                                     .SelectMany(kvp => kvp.Value)
                                                     .ToList();

                recommendedEvents.AddRange(eventsOnSearchDate);
            }

            // Remove duplicate events (in case an event is part of both a frequently searched date and a popular category)
            var uniqueRecommendedEvents = recommendedEvents.Distinct().Take(5).ToList();

            // Bind recommended events to the UI
            RecommendationsList.ItemsSource = new ObservableCollection<LocalEvent>(recommendedEvents.Distinct().Take(4));

            // Set visibility based on the availability of recommendations
            IsRecommendationsVisible = recommendedEvents.Any();
        }

        // The following algorithm was taken from a StackOverflow post
        // Uploaded by: Dmitry Bychenko
        // Available at: https://stackoverflow.com/questions/56378647/fisher-yates-shuffle-in-c-sharp
        private List<LocalEvent> ShuffleEvents(List<LocalEvent> events)
        {
            var random = new Random();
            int n = events.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                // Swap events[i] with the element at random index
                var temp = events[i];
                events[i] = events[j];
                events[j] = temp;
            }
            return events;
        }

        // The following method was adapted from a StackOverflow post
        // Uploaded by: Dmitry Bychenko
        // Available at: https://stackoverflow.com/questions/56378647/fisher-yates-shuffle-in-c-sharp
        private void ShowRandomRecommendations()
        {
            var allEvents = EventsByDate.SelectMany(kvp => kvp.Value).ToList();

            // Shuffle the events using Fisher-Yates
            var randomRecommendations = ShuffleEvents(allEvents).Take(4).ToList();

            RecommendationsList.ItemsSource = new ObservableCollection<LocalEvent>(randomRecommendations);
            IsRecommendationsVisible = randomRecommendations.Any();
        }

        // Filter events based on user selections
        private void FilterEvents()
        {
            EventQueue.Clear();
            
            // Filtering logic based on category and date range
            var filteredEvents = EventsByDate.SelectMany(kvp => kvp.Value).AsEnumerable();

            // Reset the flag at the start
            filtersApplied = false; 
               
            // Filter by selected category
            if (CategoryComboBox.SelectedItem is ComboBoxItem selectedCategory)
            {
                string category = selectedCategory.Content.ToString();

                if (!string.IsNullOrEmpty(category))
                {
                    filteredEvents = filteredEvents.Where(e => e.Category == category);

                    ResizeUIForUpcomingEvents();

                    // Increment preference for the selected category
                    CategoryPreference[category]++;
                }
            }

            // Filter by selected date range
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            // The following linq code was taken from a StackOverflow post
            // Posted by: Jon Skeet
            // Available at: https://stackoverflow.com/questions/6359980/proper-linq-where-clauses
            if (startDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.Date >= startDate.Value);

                // Track frequency of start date searches
                TrackDateSearch(startDate.Value);

                if (filteredEvents.Count() == 0)
                {
                    ResizeUIForRecommendations();
                }
                else
                {
                    ResizeUIForUpcomingEvents();
                }
            }
            if (endDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.Date <= endDate.Value);

                // Track frequency of each date in the range
                if (startDate.HasValue && endDate.HasValue && endDate.Value >= startDate.Value)
                {
                    for (var date = startDate.Value; date <= endDate.Value; date = date.AddDays(1))
                    {
                        TrackDateSearch(date);
                    }
                }
                else if (endDate.HasValue)
                {
                    TrackDateSearch(endDate.Value);
                }

                if (filteredEvents.Count() == 0)
                {
                    ResizeUIForRecommendations();
                }
                else
                {
                    ResizeUIForUpcomingEvents();
                }
            }

            // Enqueue filtered events and track their view count
            foreach (var ev in filteredEvents)
            {
                EventQueue.Enqueue(ev);

                // The code below was taken from a StackOverflow post
                // Posted by: Mark Byers
                // Available at: https://stackoverflow.com/questions/2829873/how-can-i-detect-if-this-dictionary-key-exists-in-c

                // Track how many times each event has been viewed
                if (EventViewCount.ContainsKey(ev.Title))
                {
                    EventViewCount[ev.Title]++;
                }
                else
                {
                    // Initialize view count for new events
                    EventViewCount[ev.Title] = 1;
                }
            }

            UpdateDisplayedEventsFromQueue();
            ShowRecommendations();
        }

        private void TrackDateSearch(DateTime searchDate)
        {
            // Normalize to just the date portion (ignore time)
            var dateKey = searchDate.Date;

            if (DateSearchCount.ContainsKey(dateKey))
            {
                DateSearchCount[dateKey]++;
            }
            else
            {
                DateSearchCount[dateKey] = 1;
            }
        }


        private void ResizeUIForRecommendations()
        {
            ShowToast("INFO", "No event available during this date range.");

            // Move Recommendations section up
            UpcomingEventsHeader.Visibility = Visibility.Collapsed;
            EventScrollViewer.Visibility = Visibility.Collapsed;

            // The code to set new margins was taken from a StackOverflow post
            // Uploaded by: Eben Geer
            // Available at: https://stackoverflow.com/questions/5611658/change-margin-programmatically-in-wpf-c-sharp
            RecommendationsHeader.Margin = new Thickness(10, 0, 0, 5);

            //Display more recommendations on screen
            RecommendationsScrollViewer.Height = 400;

            filtersApplied = false;

            // Check if the dictionary contains any zero values
            // Stops as soon as a zero is found efficient
            bool containsZeros = CategoryPreference.Values.Any(value => value == 0);

            if (containsZeros)
            {
                // It does, user filter patterns haven't been analysed
                // Display random recommendations
                ShowRandomRecommendations();
            }
            else
            {
                // It doesn't, user filter patterns have been analysed
                // Display their pattern recommendations
                ShowRecommendations();
            }
        }

        private void ResizeUIForUpcomingEvents()
        {
            // Set flag to true if a filter was applied
            filtersApplied = true;

            // Adjust UI since only one scrollviewer is showing
            // The following code was sugged by Visual Studio
            EventScrollViewer.Height = 400;
        }

        // Update displayed events from the queue
        private void UpdateDisplayedEventsFromQueue()
        {
            DisplayedEvents.Clear();

            // The code below was taken and modified from a Blog post
            // Titled: C# Queue with Examples
            // Uploaded by: ankita_saini
            // Available at: https://www.geeksforgeeks.org/c-sharp-queue-with-examples/
            // Access: 
            while (EventQueue.Count > 0)
            {
                // Add events from the queue
                DisplayedEvents.Add(EventQueue.Dequeue());
            }

            // Bind the updated event list to the UI
            EventsList.ItemsSource = DisplayedEvents;
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Re-filter events based on the selected category
            FilterEvents();

            // Set flag to true if a category filter is applied
            filtersApplied = true;
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Re-filter events based on the selected start date
            FilterEvents();

            // Set flag to true if a start date filter is applied
            filtersApplied = true;
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Re-filter events based on the selected end date
            FilterEvents();

            // Set flag to true if an end date filter is applied
            filtersApplied = true; 
        }

        // Resets filter UI
        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.SelectedIndex = -1;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            UpcomingEventsHeader.Visibility = Visibility.Visible;
            EventScrollViewer.Visibility = Visibility.Visible;
            EventScrollViewer.Height = 250;

            RecommendationsHeader.Margin = new Thickness(10, 10, 0, 0);
            RecommendationsScrollViewer.Height = 150;
            // Refresh events after clearing filters
            FilterEvents();
        }

        // The code for this method was taken from a YouTube video
        // Titled: Tutorial : How to Create a Toast Message. C# | Windows Form
        // Uploaded By: Coding Ideas
        // Link: https://www.youtube.com/watch?v=vLWWShU9gKY
        private void ShowToast(string type, string message)
        {
            ToastForm toast = new ToastForm(type, message);
            toast.Show();
        }
    }
}
