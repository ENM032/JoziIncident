using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for CreateServiceRequest.xaml
    /// </summary>
    public partial class CreateServiceRequest : Page
    {
        private readonly Guid UserId;
        public CreateServiceRequest(Guid userId)
        {
            InitializeComponent();
            UserId = userId;
        }

        private void BackButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigateToServiceRequestPage("Service Request");
        }

        private void NavigateToServiceRequestPage(string headerText)
        {
            // Navigate to the home page after submission
            var mainWindow = (MainWindow)App.Current.Windows[0];
            mainWindow.homeFrame.Navigate(new ServiceRequestPage1(UserId));
            mainWindow.currentPageHeader.Text = headerText;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
