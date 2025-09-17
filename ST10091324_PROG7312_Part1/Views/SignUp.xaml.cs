using ST10091324_PROG7312_Part1.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTxtBox.Text;
            string email = EmailTxtBox.Text;
            if (EmailIsValid(email))
            {
                string tempPassword = PasswordBox.Password;
                string confirmPassword = ConfirmPasswordBox.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(tempPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    MessageTxtBlock.Text = "Please fill out all fields of this form";
                }
                else if(username.Length < 4)
                {
                    MessageTxtBlock.Text = "Username must be at least 4 characters";
                }
                else if (!PasswordChecker.IsPasswordValid(tempPassword))
                {
                    RowDefinitionForm2.Height = new GridLength(4, GridUnitType.Star);
                    MessageTxtBlock.Text = "Password must be 8 or more characters in length and contain at least one digit, one uppercase letter, one lowercase letter and a special character";
                }
                else if (!tempPassword.Equals(confirmPassword))
                {
                    MessageTxtBlock.Text = "Password entered in Password field and Confirm Password field do not match!";
                }
                else if (AgreementCheckBox.IsChecked == false)
                {
                    MessageTxtBlock.Text = "Please agree to the T&Cs for your account to be created";
                }
                else
                {
                    string password = await Task.Run(() => PasswordHasher.GenerateHash(tempPassword));

                    try
                    {
                        using (var context = new UserDbContext())
                        {
                            if (await context.Users.AnyAsync(u => u.Email == email || u.Username == username).ConfigureAwait(false))
                            {
                                MessageTxtBlock.Text = "Email or Username already exists!";
                                return;
                            }

                            var newUser = new User(username, email, password, "Customer");

                            context.Users.Add(newUser);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                        }

                        // The code to get the current instance of the app was taken from C#Corner
                        // Author: XAML designer
                        // Link: https://www.c-sharpcorner.com/Resources/840/get-current-application-instance-in-wpf-and-C-Sharp.aspx

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

                        ShowToast("SUCCESS", "Profile successfully created!");
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine("Creation error: " + ex);
                        ShowToast("ERROR", "Something went wrong! Couldn't create profile.");
                    }
                }
            }
            else
            {
                MessageTxtBlock.Text = "Invalid email address entered! Please enter a valid email address before proceeding";
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
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

        // The code to minimise the window was taken from StackOverflow
        // Author: Oded
        // Link: https://stackoverflow.com/questions/2841258/minimize-a-window-in-wpf
        private void MinimiseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // The code to open and show a new window was taken from StackOverflow
        // Author: Chandra
        // Link: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/windows/how-to-get-set-main-application-window?view=netdesktop-8.0
        private void LoginLinkText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // The code to get the current instance of the app was taken from C#Corner
            // Author: XAML designer
            // Link: https://www.c-sharpcorner.com/Resources/840/get-current-application-instance-in-wpf-and-C-Sharp.aspx
            var currentApp = ((App)Application.Current);

            // The code to open and show a new window was taken from StackOverflow
            // Author: Chandra
            // Link: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/windows/how-to-get-set-main-application-window?view=netdesktop-8.0
            currentApp.MainWindow = new Login();
            currentApp.MainWindow.Show();
            this.Close();
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

        // The code to validate an email address was taken from StackOverflow and modified
        // Author: CodeArtist
        // Link: https://stackoverflow.com/questions/5342375/regex-email-validation
        private Boolean EmailIsValid(string email)
        {
            string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            Regex defaultRegex = new Regex(pattern, RegexOptions.IgnoreCase);

            return defaultRegex.IsMatch(email);
        }

        // Gives feedback to the user if the message is invalid
        private void EmailTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!EmailIsValid(EmailTxtBox.Text))
            {
                MessageTxtBlock.Text = "Invalid email address entered! Please enter a valid email address before proceeding";
            }
            else
            {
                MessageTxtBlock.Text = String.Empty;
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void UsernameTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EmailTxtBox.Focus();
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void EmailTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PasswordBox.Focus();
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ConfirmPasswordBox.Focus();
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void ConfirmPasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AgreementCheckBox.Focus();
            }
        }

        // The code for this method was taken from StackOverflow
        // Author: Mark W
        // Link: https://stackoverflow.com/questions/4464386/capturing-key-press-event-in-my-wpf-application-which-do-not-have-focus
        private void AgreementCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Enter)
            {
                AgreementCheckBox.IsChecked = true;
            }
        }
    }
}
