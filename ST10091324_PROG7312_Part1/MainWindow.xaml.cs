using ST10091324_PROG7312_Part1.Model;
using ST10091324_PROG7312_Part1.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ST10091324_PROG7312_Part1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Guid UserId;
        private bool isMaximized = false;

        public MainWindow(Guid userIdentifier)
        {
            InitializeComponent();
            UserId = userIdentifier;
            ShowHomePage();
            
            LoadUserData(UserId);
        }

        private void LoadUserData(Guid userIdentifier)
        {
            // Fetch the user from the database
            var user = GetUserFromDatabase(userIdentifier);

            if (user != null)
            {
                // Display user information
                DisplayUserInfo(user);
            }
            else
            {
                // Handle user not found
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private User GetUserFromDatabase(Guid userIdentifier)
        {
            using (var context = new UserDbContext())
            {
                // Query to find the user by username or email
                return context.Users
                              .FirstOrDefault(u => u.Id == userIdentifier);
            }
        }

        private void DisplayUserInfo(User user)
        {
            // Set the display name
            displayNameTxtBlock.Text = user.Username;

            // Set the profile image
            SetProfileImage(user.ProfileImgPath);
        }

        private void SetProfileImage(string profileImgPath)
        {
            if (!string.IsNullOrEmpty(profileImgPath))
            {
                try
                {
                    profileImg.ImageSource = new BitmapImage(new Uri(profileImgPath));
                    profileImg.Stretch = System.Windows.Media.Stretch.UniformToFill;
                }
                catch (Exception ex)
                {
                    // Handle image loading error
                    MessageBox.Show($"Error loading profile image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // The code for the method below was taken from a YouTube video
        // Titled: C# WPF UI | How to Design Flat Data Table Dashboard in WPF (Customize Datagrid)
        // Uploaded by: C# WPF UI Academy
        // Link: https://www.youtube.com/watch?v=mlmyFXJy8gQ
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(e.ChangedButton == MouseButton.Left)
                {
                    this.DragMove();
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("ERROR BOR410! Border mouse down error occured.");
            }

        }

        // The code to maximise and minise was taken from a YouTube video
        // Titled: C# WPF UI | How to Design Flat Data Table Dashboard in WPF (Customize Datagrid)
        // Uploaded by: C# WPF UI Academy
        // Link: https://www.youtube.com/watch?v=mlmyFXJy8gQ
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    ResetScrollerHeight();
                    isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    ChangeScrollerHeight();
                    isMaximized = true;
                }
            }
        }

        private void ReportIncidentBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPageHeader.Text = "Incident Report Creation";
            txtSearch.Text = string.Empty;
            // The code to navigate to a new page inside the frame was taken from StackOverflow 
            // AUthor: Greg
            // Link: https://stackoverflow.com/questions/15156625/wpf-application-not-browser-and-navigation
            homeFrame.Navigate(new ReportIncidentPage(UserId));
            HideSearchGrid();
        }

        private async void logoutBtn_Click(object sender, RoutedEventArgs e)
        {           
            ShowToast("SUCESS", "Logging out...");

            // Add a delay of 1 second (1000 milliseconds)
            await Task.Delay(2000);

            // Retrieve Current Application Instance
            // This line gets the current application instance and casts it to the App class.
            var currentApp = ((App)Application.Current);

            // Set a New Main Window
            // This sets the MainWindow property of the application to a new instance of the Login window.
            currentApp.MainWindow = new Login();

            // Show the New Main Window
            currentApp.MainWindow.Show();

            // Close the Current Window
            this.Close();           
        }

        private void burgerMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.Menu.Visibility = this.Menu.Visibility == Visibility.Visible
            //                    ? Visibility.Collapsed
            //                    : Visibility.Visible;
            ShowToast("INFO", "This feature is coming soon.");
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPageHeader.Text = "Home";
            txtSearch.Text = string.Empty;
            // The code to navigate to a new page inside the frame was taken from StackOverflow 
            // AUthor: Greg
            // Link: https://stackoverflow.com/questions/15156625/wpf-application-not-browser-and-navigation
            homeFrame.Navigate(new Home(UserId));
            ShowSearchGrid();
        }

        private void profileHolder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // The code to initialise and open a file dialog box was taken from a YouTube video
            // Title: Upload and display Image Dynamically in WPF
            // Uploaded by: WPF
            // Link: https://www.youtube.com/watch?v=QSdAHV6IocM
            // Initialize and open the file dialog box to select an image
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files|*.bmp; *.jpg; *.png; *.jpeg",
                FilterIndex = 1
            };


            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected image file path
                string newProfilePicPath = openFileDialog.FileName;

                // Update the profile image in the database
                UpdateUserProfileImage(newProfilePicPath);

                // Update the profile image in the UI
                SetProfileImage(newProfilePicPath);

                // Show a success toast message
                ShowToast("SUCCESS", "New profile picture added!");
            }
        }

        private void UpdateUserProfileImage(string newProfilePicPath)
        {
            // Retrieve the user from the database using UserDbContext
            using (var context = new UserDbContext())
            {
                var user = context.Users
                                  .FirstOrDefault(u => u.Id == UserId);

                if (user != null)
                {
                    // Update the user's profile image path in the database
                    user.ProfileImgPath = newProfilePicPath;

                    // Save the changes to the database
                    context.SaveChanges();
                }
                else
                {
                    // Handle the case where the user is not found in the database
                    MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text.Equals(""))
            {
                homeFrame.Navigate(new Home(UserId));
            }
            homeFrame.Navigate(new Home(txtSearch
                            .Text
                            .Trim()
                            .ToUpper(), UserId));
        }

        private void ShowHomePage()
        {
            homeFrame.Navigate(new Home(UserId));
            currentPageHeader.Text = "Home";
            ShowSearchGrid();
        }

        // The code to minimise the window was taken from StackOverflow
        // Author: Oded
        // Link: https://stackoverflow.com/questions/2841258/minimize-a-window-in-wpf
        private void miniseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // The code to close the application programmatically was taken from StackOverflow
                // Author: Dirk Vollmar
                // Link: https://stackoverflow.com/questions/2820357/how-do-i-exit-a-wpf-application-programmatically
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception)
            {
                ShowToast("INFO", "Could not close! An unexpected error has occured.");
            }
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

        private void LocalEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPageHeader.Text = "Local events & announcements";
            txtSearch.Text = string.Empty;
            // The code to navigate to a new page inside the frame was taken from StackOverflow 
            // AUthor: Greg
            // Link: https://stackoverflow.com/questions/15156625/wpf-application-not-browser-and-navigation
            homeFrame.Navigate(new LocalEvents());
            HideSearchGrid();
        }

        public void HideSearchGrid()
        {
            searchGrid.Visibility = Visibility.Hidden;
        }

        public void ShowSearchGrid()
        {
            searchGrid.Visibility = Visibility.Visible;
        }

        public void CollapseNotFoundGrid()
        {
            NotFoundStackPanel.Visibility = Visibility.Collapsed;
        }

        public void ShowNotFoundGrid()
        {
            NotFoundStackPanel.Visibility = Visibility.Visible;
        }

        public void HideNotFoundGrid()
        {
            NotFoundStackPanel.Visibility = Visibility.Collapsed;
        }

        private void SetScrollerHeight(double eventHeight, double recommendationsHeight)
        {
            if (homeFrame.Content is LocalEvents page)
            {
                // Accesses contents of the LocalEvents Page
                page.EventScrollViewer.Height = eventHeight;
                page.RecommendationsScrollViewer.Height = recommendationsHeight;
            }
        }

        private void ChangeScrollerHeight()
        {
            // Set height of event scroll viewer to 500
            // Set height of recommendations scroll viewer to 270
            SetScrollerHeight(500, 270); 
        }

        private void ResetScrollerHeight()
        {
            // Set height of event scroll viewer to 250
            // Set height of recommendations scroll viewer to 150
            SetScrollerHeight(250, 150); 
        }

        private void ServiceRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPageHeader.Text = "Service Request";
            txtSearch.Text = string.Empty;
            // The code to navigate to a new page inside the frame was taken from StackOverflow 
            // AUthor: Greg
            // Link: https://stackoverflow.com/questions/15156625/wpf-application-not-browser-and-navigation
            homeFrame.Navigate(new ServiceRequestPage1(UserId));
            HideSearchGrid();
            ServiceRequest._counter = 0;
        }
    }
}
