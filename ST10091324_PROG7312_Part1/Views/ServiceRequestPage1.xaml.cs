using MaterialDesignThemes.Wpf;
using ST10091324_PROG7312_Part1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for ServiceRequestPage1.xaml
    /// </summary>
    public partial class ServiceRequestPage1 : Page
    {
        public ObservableCollection<ServiceRequest> ServiceRequests { get; set; }

        private ServiceRequestBST _serviceRequestBST;
        private ServiceRequestGraph _serviceRequestGraph;
        private User CurrentUser;
        private string CurrentUserRole;

        private readonly ObservableCollection<String> StatusOptions = new ObservableCollection<string>
        {
            "Completed",
            "In Progress",
            "Pending"
        };

        // Property to control if the status can be edited or not
        public bool IsEditableStatus => CurrentUserRole == "Admin" || CurrentUserRole == "Technician";

        public ServiceRequestPage1(Guid userId)
        {
            InitializeComponent();
            
            // Initialize async loading
            _ = InitializePageAsync(userId);
        }
        
        private async Task InitializePageAsync(Guid userId)
        {
            try
            {
                CurrentUser = await GetUserFromDatabaseAsync(userId).ConfigureAwait(false);
                
                // Switch back to UI thread for UI updates
                await Dispatcher.InvokeAsync(() =>
                {
                    CurrentUserRole = CurrentUser?.Role ?? "Guest";
                    
                    _serviceRequestBST = new ServiceRequestBST();
                    _serviceRequestGraph = new ServiceRequestGraph();
                    
                    // Initialize data
                    InitializeServiceRequests();
                    
                    // Get the sorted list of service requests from the BST
                    List<ServiceRequest> sortedRequests = _serviceRequestBST.InOrderTraversal(CurrentUser.Id);
                    
                    // Convert to ObservableCollection for data binding
                    ServiceRequests = new ObservableCollection<ServiceRequest>(sortedRequests);
                    
                    // Bind the ListBox to the ObservableCollection
                    ServiceRequestsList.ItemsSource = ServiceRequests;
                    
                    ChangeMessageHeaderTitle();
                });
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                await Dispatcher.InvokeAsync(() =>
                {
                    ShowToast("ERROR", "Failed to load user data. Please try again.");
                });
            }
        }

        private async Task<User> GetUserFromDatabaseAsync(Guid userIdentifier)
        {
            using (var context = new UserDbContext())
            {
                try
                {
                    // Query to find the user by user id using async method
                    return await context.Users
                                  .FirstOrDefaultAsync(u => u.Id == userIdentifier)
                                  .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Log error and return null for graceful handling
                    System.Diagnostics.Debug.WriteLine($"Database error in GetUserFromDatabaseAsync: {ex.Message}");
                    return null;
                }
            }
        }

        // Initialize some sample data
        private void InitializeServiceRequests()
        {
            // Street Maintenance - Pothole Repair
            ServiceRequest streetRepair1 = new ServiceRequest(
                "Pothole repair on Main Street near Elm Avenue.",
                "In Progress",
                45.0,
                false,
                ConvertStringToGuid("e0ee88eb-0ac1-44fe-9c19-0ef9c754d772")
            );

            // Street Maintenance - Road Marking
            ServiceRequest streetRepair2 = new ServiceRequest(
                "Repainting of lane markings on Oak Street.",
                "Pending",
                0.0,
                true,
                ConvertStringToGuid("7ad1e24c-5a38-48e2-9121-6d295cd4a33b")
            );

            // Public Safety - Streetlight Outage
            ServiceRequest streetlightOutage = new ServiceRequest(
                "Streetlight outage reported at the intersection of 5th Ave and Pine St.",
                "Completed",
                100.0,
                false,
                ConvertStringToGuid("e0ee88eb-0ac1-44fe-9c19-0ef9c754d772")
            );

            // Waste Management - Missed Garbage Collection
            ServiceRequest missedGarbage = new ServiceRequest(
                "Missed garbage collection at Riverside Park.",
                "Pending",
                0.0,
                true,
                ConvertStringToGuid("e0ee88eb-0ac1-44fe-9c19-0ef9c754d772")
            );

            // Environmental - Tree Trimming
            ServiceRequest treeTrimming = new ServiceRequest(
                "Tree trimming required on 2nd Avenue due to overhanging branches.",
                "In Progress",
                30.0,
                false,
                ConvertStringToGuid("c89b2600-0b2b-4252-9702-d7b34af1bebc")
            );

            // Environmental - Park Maintenance
            ServiceRequest parkMaintenance = new ServiceRequest(
                "Park maintenance request for cleaning the playground area.",
                "Completed",
                100.0,
                false,
                ConvertStringToGuid("c89b2600-0b2b-4252-9702-d7b34af1bebc")
            );

            // Public Health - Pest Control
            ServiceRequest pestControl = new ServiceRequest(
                "Request for pest control services due to rodent sightings in the community center.",
                "In Progress",
                60.0,
                false,
                ConvertStringToGuid("7ad1e24c-5a38-48e2-9121-6d295cd4a33b")
            );

            // Housing - Emergency Housing
            ServiceRequest emergencyHousing = new ServiceRequest(
                "Emergency housing request for a family affected by a recent fire.",
                "Pending",
                0.0,
                true,
                ConvertStringToGuid("7ad1e24c-5a38-48e2-9121-6d295cd4a33b")
            );

            // Public Safety - Traffic Signal Repair
            ServiceRequest trafficSignalRepair = new ServiceRequest(
                "Repair needed for malfunctioning traffic signal at Main Street and Maple Ave.",
                "In Progress",
                75.0,
                false,
                ConvertStringToGuid("e0ee88eb-0ac1-44fe-9c19-0ef9c754d772")
            );

            // Environmental - Pollution Complaint
            ServiceRequest pollutionComplaint = new ServiceRequest(
                "Report of illegal dumping of industrial waste near the river.",
                "Pending",
                0.0,
                true,
                ConvertStringToGuid("c89b2600-0b2b-4252-9702-d7b34af1bebc")
            );


            // Insert into BST
            _serviceRequestBST.Insert(streetRepair1);
            _serviceRequestBST.Insert(streetRepair2);
            _serviceRequestBST.Insert(streetlightOutage);
            _serviceRequestBST.Insert(missedGarbage);
            _serviceRequestBST.Insert(treeTrimming);
            _serviceRequestBST.Insert(parkMaintenance);
            _serviceRequestBST.Insert(pestControl);
            _serviceRequestBST.Insert(emergencyHousing);
            _serviceRequestBST.Insert(trafficSignalRepair);
            _serviceRequestBST.Insert(pollutionComplaint);

            // Add dependencies to graph
            _serviceRequestGraph.AddDependency(streetRepair1, streetlightOutage);
            _serviceRequestGraph.AddDependency(streetRepair1, trafficSignalRepair);
            _serviceRequestGraph.AddDependency(streetRepair1, missedGarbage);
            _serviceRequestGraph.AddDependency(parkMaintenance, treeTrimming);

            _serviceRequestGraph.AddDependency(pestControl, emergencyHousing);
        }

        private void requestSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = requestSearch.Text.Trim().ToLower();

            // Clear the current service requests in the ListBox
            ServiceRequests.Clear();

            if (string.IsNullOrEmpty(query))
            {
                // If the search box is empty, show all items from the BST
                var allRequests = _serviceRequestBST.InOrderTraversal(CurrentUser.Id);
                foreach (var request in allRequests)
                {
                    request.SearchRequestType = "N"; // Mark as 'N' for None
                    ServiceRequests.Add(request);  // Add all requests to the ObservableCollection
                }
            }
            else
            {
                // Search for all matching requests by RequestID
                var allMatches = _serviceRequestBST.SearchAll(query, CurrentUser.Id);
                if (allMatches.Count > 0)
                {
                    List<ServiceRequest> relatedRequests = new List<ServiceRequest>();

                    foreach (var match in allMatches)
                    {
                        // Mark the matched service request as 'S' for Search Result
                        match.SearchRequestType = "S";
                        relatedRequests.Add(match);

                        // Get the related service requests (dependencies) from the graph
                        var dependencies = _serviceRequestGraph.GetDependencies(match);

                        // Mark all related requests as related (not search results)
                        foreach (var relatedRequest in dependencies)
                        {
                            relatedRequest.SearchRequestType = "R";  // Related request
                            relatedRequests.Add(relatedRequest);
                        }
                    }

                    // Remove duplicates from the relatedRequests list
                    var uniqueRelatedRequests = relatedRequests.Distinct().ToList();

                    // Add the unique related requests to the ServiceRequests ObservableCollection
                    foreach (var request in uniqueRelatedRequests)
                    {
                        ServiceRequests.Add(request);
                    }
                }
                else
                {
                    ShowToast("INFO", $"Service request with ID \"{query}\" not found.");
                }
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

        private void ServiceRequestsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected ServiceRequest from the ListBox
            ServiceRequest selectedRequest = ServiceRequestsList.SelectedItem as ServiceRequest;

            if (selectedRequest != null)
            {
                // Access the RequestID or perform other actions
                string selectedRequestId = selectedRequest.RequestID;
                string selectedRequestStatus = selectedRequest.Status;

                //ChangeServiceRequestStatus(selectedRequestId, selectedRequestStatus);
            }
        }

        private void ChangeServiceRequestStatus(string requestId, string newStatus)
        {
            bool success = _serviceRequestBST.UpdateStatus(requestId, newStatus);

            if (success)
            {
                ShowToast("INFO", "Service request successfully changed.");
                //MessageBox.Show($"Status of service request {requestId} updated to {newStatus}.", "Success");
            }
            else
            {
                ShowToast("INFO", "Something went wrong! Could not change service request.");
                //MessageBox.Show($"Service request {requestId} not found.", "Error");
            }
        }

        private void ChangeMessageHeaderTitle()
        {
            if (!_serviceRequestBST.IsEmpty())
            {
                int size = _serviceRequestBST.Size(); // Get the size of the list once for efficiency

                if (size == 1)
                {
                    // Case when the list has exactly 1 element
                    MessageHeaderServiceRequest.Text = "We have received your service request. We will regularly update its progress as we complete it.";
                    MessageHeaderServiceRequest.Foreground = new SolidColorBrush(Colors.Green);
                    MessagetHeaderIconServiceRequest.Kind = PackIconKind.EmojiHappy;
                    MessagetHeaderIconServiceRequest.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (size >= 2 && size <= 3)
                {
                    // Case when the list size is between 2 and 3
                    MessageHeaderServiceRequest.Text = "We have received your service requests. We will regularly update their progress as we complete them.";
                    MessageHeaderServiceRequest.Foreground = new SolidColorBrush(Colors.Green);
                    MessagetHeaderIconServiceRequest.Kind = PackIconKind.EmojiHappy;
                    MessagetHeaderIconServiceRequest.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (size >= 4)
                {
                    // Case when the list size is 4 or more
                    MessageHeaderServiceRequest.Text = "We are sorry to hear that you're having so many issues, but rest assured that we are working hard to resolve all the issues you are experiencing.";
                    MessageHeaderServiceRequest.Foreground = new SolidColorBrush(Colors.DarkOrange);
                    MessagetHeaderIconServiceRequest.Kind = PackIconKind.EmojiConfused;
                    MessagetHeaderIconServiceRequest.Foreground = new SolidColorBrush(Colors.DarkOrange);
                }
            }
        }

        private void CreateServiceRequestBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Navigate to the home page after submission
            // Retrieve Current Application Instance
            // This line gets the current application instance and casts it to the App class.
            //var mainWindow = (MainWindow)App.Current.Windows[0];

            //// Set a new page
            //// This sets the frame property of the application to a new instance of the Create Service Request page.
            //mainWindow.homeFrame.Navigate(new CreateServiceRequest(CurrentUser.Id));
            //mainWindow.currentPageHeader.Text = "Create Service Request";
            ShowToast("INFO", "This feature is coming soon.");
        }

        // Modified from StackOverflow post
        // Titled: How can I convert string to Guid? [duplicate]
        // Answered by: Morgan, M
        // Available at: https://stackoverflow.com/questions/33953985/how-can-i-convert-string-to-guid
        private Guid ConvertStringToGuid(string id)
        {
            return Guid.Parse(id);
        }
    }
}
