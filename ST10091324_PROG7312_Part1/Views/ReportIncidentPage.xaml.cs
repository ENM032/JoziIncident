using MaterialDesignThemes.Wpf;
using ST10091324_PROG7312_Part1.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for ReportIncidentPage.xaml
    /// </summary>
    public partial class ReportIncidentPage : Page
    {
        private readonly Guid UserID;
        private string mediaFilePath = null;
        private readonly List<string> categories = new List<string>() 
        {"Sanitation", "Roads", "Utilities", "Transportation", "Education", "Healthcare", "Public Space"};

        // The names of cities in Joburg North & East was taken from Joburg.co.za
        // Author: [s.n.]
        // Link: https://joburg.co.za/welcome-to-johannesburg/#Areas%20Of%20Johannesburg
        private readonly List<string> cities = new List<string>() 
        { "Alexandra", "Birnam", "Bramley", "Dunkeld", "Emmarentia", "Greenside", "Higshland North", "Houghton", 
            "Linden", "Melrose North", "Melville", "Midrand", "Norwood", "Orange Grove", "Orchards", "Parkhurst", "Parktown North",
            "Parkview", "Randburg", "Rosebank", "Sandton", "Saxonwold", "Steyn City", "Victory Park", "Waverley", "Wynberg", 
            "Alberton", "Bedfordview", "Benoni", "Boksburg", "Brakpan", "Edenvale", "Germiston", "Modderfontein", "Kempton Park", 
            "Linksfield", "Nigel", "Serengeti Golf and Wildlife Estate", "Springs"
        };
        
        public ReportIncidentPage(Guid userid)
        {
            InitializeComponent();
            //((MainWindow)App.Current.Windows[0]).HideSearchGrid();
            PopulateListBox();
            UserID = userid;
        }

        // The code to set the combobox itemsource was taken from StackOverflow
        // Author: Allen Rice
        // Link: https://stackoverflow.com/questions/2417960/populating-a-combobox-using-c-sharp
        private void PopulateListBox()
        {            
            CategoryCmbBx.ItemsSource = categories;
            CategoryCmbBx.SelectedItem = null;
        }

        private void UploaderBtn_Click(object sender, RoutedEventArgs e)
        {
            // The code to initialise and open a file dialog box was taken from a YouTube video
            // Title: Upload and display Image Dynamically in WPF
            // Uploaded by: WPF
            // Link: https://www.youtube.com/watch?v=QSdAHV6IocM
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image File(*.bmp;*.jpg;*.png;*.jpeg)|*.bmp;*.jpg;*.png;*.jpeg|Audio File(*.mp3)|*.mp3;|Video File(*.mp4)|*.mp4;|Text File(*.txt)|*.txt;";
            openFileDialog.FilterIndex = 1;

            
            if (openFileDialog.ShowDialog() == true)
            {
                mediaFilePath = openFileDialog.FileName;
                ShowToast("INFO", "File has been uploaded!");
            }
        }

        private async void SubmitIncidentBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if the category is selected
            if (CategoryCmbBx.SelectedItem != null)
            {
                try
                {
                    // Retrieve the form data
                    string location = LocationTxtBx.Text;
                    string category = CategoryCmbBx.SelectedItem.ToString();
                    string description = new TextRange(DescriptionRichTxtBx.Document.ContentStart, DescriptionRichTxtBx.Document.ContentEnd).Text;

                    // Validate if any required field is empty
                    if (string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(mediaFilePath))
                    {
                        ShowWarningMessage("Almost done! Please fill out all required fields on this form to proceed.");
                    }
                    else
                    {
                        // Fetch the user ID from the database based on the userIdentifier (e.g., username or email)
                        var user = await GetUserFromDatabaseAsync(UserID); // Assuming UserIdentifier is available
                        if (user == null)
                        {
                            ShowWarningMessage("User not found!");
                            return;
                        }

                        // Create a new Incident object and associate it with the UserId
                        Incident incident = new Incident(location, category, description, mediaFilePath);
                        incident.UserId = user.Id;

                        // Add the incident to the database asynchronously
                        await AddIncidentToDatabaseAsync(incident);

                        // Clear the form inputs after submission
                        ClearFormFields();

                        // Show success message
                        ShowToast("SUCCESS", "Thank you! Your incident has been reported.");

                        // Navigate back to the home page
                        NavigateToHomePage();
                    }
                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    MessageTxtBlock.Text = $"Error: {ex.Message}";
                    MessageIcon.Kind = PackIconKind.AlertCircle;
                    MessageTxtBlock.Foreground = Brushes.Red;
                    MessageIcon.Foreground = Brushes.Red;
                }
            }
            else
            {
                ShowWarningMessage("Almost done! Please fill out all required fields on this form to proceed.");
            }
        }

        private async Task<User> GetUserFromDatabaseAsync(Guid userId)
        {
            // Fetch the user from the database asynchronously based on the userIdentifier (e.g., username or email)
            using (var context = new UserDbContext())
            {
                return await context.Users
                                    .FirstOrDefaultAsync(u => u.Id == userId);
            }
        }

        private async Task AddIncidentToDatabaseAsync(Incident incident)
        {
            // Use async database operations to add the incident
            using (var context = new UserDbContext())
            {
                context.Incidents.Add(incident);  // Add the incident to the context
                await context.SaveChangesAsync(); // Save changes asynchronously to the database
            }
        }

        private void ShowWarningMessage(string message)
        {
            MessageTxtBlock.Text = message;
            MessageIcon.Kind = PackIconKind.ThumbUp;
            MessageTxtBlock.Foreground = Brushes.DarkOrange;
            MessageIcon.Foreground = Brushes.DarkOrange;
        }

        private void ClearFormFields()
        {
            // Clear form fields after successful submission
            LocationTxtBx.Clear();
            CategoryCmbBx.SelectedItem = null;
            DescriptionRichTxtBx.Document.Blocks.Clear();
            mediaFilePath = null;
        }

        private void NavigateToHomePage()
        {
            // Navigate to the home page after submission
            var mainWindow = (MainWindow)App.Current.Windows[0];
            mainWindow.homeFrame.Navigate(new Home(UserID));
            mainWindow.currentPageHeader.Text = "Home";
            mainWindow.ShowSearchGrid();
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

        // The code to set the autocomplete text from the autocomplete list to the textbox text was taken from StackOverflow
        // Author: Romylussone
        // Link: https://stackoverflow.com/questions/950770/autocomplete-textbox-in-wpf
        private void AutoCompletorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AutoCompletorList.SelectedItem != null)
                {
                    LocationTxtBx.Text = AutoCompletorList.SelectedValue.ToString();                  
                }
            }
            catch (Exception)
            {
                ShowToast("ERROR", "An unexpected error has occured!");
            }
        }

        // The code to display the autocompleted list based off of the textbox input was taken from StackOverflow
        // Author: Romylussone
        // Link: https://stackoverflow.com/questions/950770/autocomplete-textbox-in-wpf
        private void LocationTxtBx_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    // change focus or remove focus on this element
                    CategoryCmbBx.Focus();
                }
                else
                {
                    if (LocationTxtBx.Text.Trim() != "")
                    {
                        cities.Sort();
                        var filteredCities = cities.Where(x => x.Trim()
                        .ToLower()
                        .Contains(LocationTxtBx.Text.Trim().ToLower()));

                        if (filteredCities.Any())
                        {
                            AutoCompletorListPopup.IsOpen = true;
                            AutoCompletorListPopup.Visibility = Visibility.Visible;
                            AutoCompletorList.ItemsSource = filteredCities;
                        }
                        else
                        {
                            ShowToast("INFO", "No location autocomplete options available.");
                        }                      
                    }
                    else
                    {
                        AutoCompletorListPopup.IsOpen = false;
                        AutoCompletorListPopup.Visibility = Visibility.Collapsed;
                        AutoCompletorList.ItemsSource = null;
                    }
                }
            }
            catch (Exception)
            {
                ShowToast("ERROR", "An unexpected error has occured!");
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Romylussone
        // Link: https://stackoverflow.com/questions/950770/autocomplete-textbox-in-wpf
        private void LocationTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AutoCompletorList.SelectedItem != null)
                {
                    LocationTxtBx.Text = AutoCompletorList.SelectedValue.ToString();
                }
            }
            catch (Exception)
            {
                ShowToast("ERROR", "An unexpected error has occured!");
            }
        }
    }
}
