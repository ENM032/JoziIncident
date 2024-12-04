using MaterialDesignThemes.Wpf;
using ST10091324_PROG7312_Part1.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using MaterialDesignThemes.Wpf;

namespace ST10091324_PROG7312_Part1.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private readonly string Id;

        public Home(Guid userID)
        {
            InitializeComponent();

            var incidents = GetIncidentsByUserId(userID);
            BindListView(incidents);
        }

        public Home(string id, Guid userID)
        {
            InitializeComponent();

            Id = id;


           var incidents = GetIncidentsByFilters(userID, Id);
           if (incidents.Any())
           {
               BindListView(incidents);
           }
           else
           {
               ShowNoIncidentsFoundMessage();
           }

        }

        private List<Incident> GetIncidentsByUserId(Guid userId)
        {
            using (var context = new UserDbContext())
            {
                return context.Incidents
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }

        private List<Incident> GetIncidentsByFilters(Guid userId, string id)
        {
            using (var context = new UserDbContext())
            {
                IQueryable<Incident> query = context.Incidents
                    .Where(x => x.UserId == userId);  // Filter by userID

                // If Id is provided, filter by the incident ID
                if (!string.IsNullOrEmpty(id))
                {
                    query = query.Where(x => x.Id.Contains(id));
                }

                return query.Include(x => x.User).ToList();
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowNoIncidentsFoundMessage()
        {
            homeHeader.Text = !string.IsNullOrEmpty(Id)
                ? $"We could not find an incident report with the id \"{Id}\"."
                : "No incidents available for this user.";

            HomeHeaderIcon.Kind = PackIconKind.EmoticonSad;
        }

        private void BindListView(List<Incident> incidents)
        {
            if (incidents.Count >= 1)
            {
                CustomerGrid.ItemsSource = incidents;

                homeHeader.Text = "Thank you for choosing to report incidents! Here is a list of your incidents:";
                HomeHeaderIcon.Kind = PackIconKind.EmoticonHappy;
                homeHeader.Foreground = System.Windows.Media.Brushes.Green;
                HomeHeaderIcon.Foreground = System.Windows.Media.Brushes.Green;

                homeHeaderRowDef.Height = new GridLength(35);
            }
            else if (incidents == null)
            {
                homeHeader.Text = $"We could not find an incident report with id the \"{Id}\".";
                HomeHeaderIcon.Kind = PackIconKind.EmoticonSad;
            }
        }
    }
}
