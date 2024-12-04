using ST10091324_PROG7312_Part1.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void UsernameTxtBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PasswordBox.Focus();
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void PasswordBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RememberMeCheckBox.Focus();
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void RememberMeCheckBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RememberMeCheckBox.IsChecked = true;
            }
        }

        // The code to minimise the window was taken from StackOverflow
        // Author: Oded
        // Link: https://stackoverflow.com/questions/2841258/minimize-a-window-in-wpf
        private void MinimiseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // The code to close the application programmatically was taken from StackOverflow
                // Author: Dirk Vollmar
                // Link: https://stackoverflow.com/questions/2820357/how-do-i-exit-a-wpf-application-programmatically
                Application.Current.Shutdown();
            }
            catch (Exception)
            {
                MessageTxtBlock.Text = "Could not close! An unexpected error has occured.";
            }
        }

        private void SignupLinkText_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            // The code to get the current instance of the app was taken from C#Corner
            // Author: XAML designer
            // Link: https://www.c-sharpcorner.com/Resources/840/get-current-application-instance-in-wpf-and-C-Sharp.aspx
            var currentApp = ((App)Application.Current);

            // The code to open and show a new window was taken from StackOverflow
            // Author: Chandra
            // Link: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/windows/how-to-get-set-main-application-window?view=netdesktop-8.0
            currentApp.MainWindow = new SignUp();
            currentApp.MainWindow.Show();
            this.Close();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userIdentifier = UsernameTxtBox.Text;

            // Validate inputs
            if (!ValidateInput(userIdentifier, PasswordBox.Password))
            {
                return;
            }

            try
            {
                // Attempt to log the user in
                var retrievedUser = await RetrieveUserByIdentifier(userIdentifier);
                if (retrievedUser == null)
                {
                    MessageTxtBlock.Text = "User not found!";
                    return;
                }

                // Verify password
                bool isPasswordValid = await VerifyUserPassword(retrievedUser, PasswordBox.Password);
                if (isPasswordValid)
                {
                    HandleSuccessfulLogin(retrievedUser);
                }
                else
                {
                    MessageTxtBlock.Text = "Invalid password entered!";
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private bool ValidateInput(string userIdentifier, string password)
        {
            if (string.IsNullOrWhiteSpace(userIdentifier))
            {
                MessageTxtBlock.Text = "Please enter a username or email to login";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                MessageTxtBlock.Text = "Please enter a password to login";
                return false;
            }

            return true;
        }

        private async Task<bool> VerifyUserPassword(User user, string password)
        {
            return await Task.Run(() => PasswordHasher.VerifyPasswordHash(user.Username, password, user.Password));
        }

        private async Task<User> RetrieveUserByIdentifier(string userIdentifier)
        {
            using (var context = new UserDbContext())
            {
                return await context.Users
                                    .FirstOrDefaultAsync(u => u.Username == userIdentifier || u.Email == userIdentifier);
            }
        }

        private async void HandleSuccessfulLogin(User user)
        {
            ShowToast("SUCCESS", "Login successful!");

            // Add a delay of 1 second (1000 milliseconds)
            await Task.Delay(2000);

            var currentApp = (App)Application.Current;
            currentApp.MainWindow = new MainWindow(user.Id);
            currentApp.MainWindow.Show();
            this.Close();         
        }

        private void HandleError(Exception ex)
        {
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                MessageTxtBlock.Text = "An error occurred while processing your login: " + ex.InnerException.Message;
            }
            else
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                MessageTxtBlock.Text = "An error occurred while processing your login: " + ex.Message;
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

        private void RememberMeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowToast("INFO", "This feature is coming soon.");
        }

        public async Task WarmUpDatabaseAsync()
        {
            try
            {
                // Create a DbContext instance (make sure your context is correct)
                using (var context = new UserDbContext())
                {
                    // Simple query to test the connection, like fetching the first user or just checking count
                    var testQuery = await context.Users
                                                  .Take(1)  // Just fetching one record to avoid heavy load
                                                  .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., connection issues)
                MessageBox.Show($"Error warming up database: {ex.Message}");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await WarmUpDatabaseAsync();
        }
    }
}
