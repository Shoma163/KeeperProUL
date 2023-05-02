using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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

namespace KeeperProUL.Pages
{
    public partial class ListApplications : Page
    {
        public class SortItem
        {
            public string Name { get; set; }
            public SortDescription Sort { get; set; }
        }
        public KeeperProULEntities database;

        public List<SortItem> SortDescriptions { get; set; }
        public SortItem SelectedSortDescription { get; set; }
        public ListApplications()
        {
            InitializeComponent();

            database = new KeeperProULEntities();

            BindingUser();

            LoadTypeApplication();

            ObservableCollection<Division> divisions= new ObservableCollection<Division>(database.Divisions.ToList());

            cbDivision.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = divisions
            });

            ObservableCollection<Status> statuses = new ObservableCollection<Status>(database.Status.ToList());

            cbStatus.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = statuses
            });


            SortDescriptions = new List<SortItem>()
            {
                new SortItem()
                {
                    Name = "Сортировка заявок по возрастанию",
                    Sort = new SortDescription("User1.Position", ListSortDirection.Ascending),
                },
                new SortItem()
                {
                    Name = "Сортировка заявок по убыванию",
                    Sort = new SortDescription("User1.Position", ListSortDirection.Descending),
                },
            };

            DataContext = this;
        }

        public void LoadTypeApplication()
        {
            List<string> applicationTypes = new List<string>() { "Индивидуальное посещение" };
            var types = database.Purposes.Select(p => p.Group).Distinct().ToList();
            foreach (var type in types)
            {
                if (type != null)
                {
                    applicationTypes.Add(type.ToString());
                }
            }

            cbType.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = applicationTypes
            });
        }

        public void BindingUser()
        {
            ObservableCollection<Application> applications = new ObservableCollection<Application>(database.Applications.ToList());

            lvApplications.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = applications,
            });


        }

        private void Filter()
        {
            string searchString = Search.Text.Trim();

            var view = CollectionViewSource.GetDefaultView(lvApplications.ItemsSource);
            view.Filter = o => 
            {
                Application application = o as Application;
                if (application == null) { return false; }

                bool isVisible = true;

                if (cbType.SelectedItem != null)
                {
                    if (cbType.SelectedIndex == 0)
                    {
                        isVisible = application.Purpose1.Group == null;
                    }
                    else
                    {
                        isVisible = application.Purpose1.Group == (string)cbType.SelectedItem;
                    }
                }

                if(cbDivision.SelectedItem != null)
                {
                    isVisible = application.Purpose1.Staff1.Division1 == cbDivision.SelectedItem;
                }

                if (cbStatus.SelectedItem != null)
                {
                    isVisible = application.Status1 == cbStatus.SelectedItem;
                }

                if (searchString.Length > 0)
                {
                    isVisible = isVisible && (application.Status.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 ||
                    application.User1.LastName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 ||
                    application.Purpose1.Staff1.Division.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
                }

                return isVisible;
            };
        }


        private void ApplySort()
        {
            var view = CollectionViewSource.GetDefaultView(lvApplications.ItemsSource);
            if (view == null || SelectedSortDescription == null) return;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(SelectedSortDescription.Sort);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySort();
        }

        private void cbTypeApplication(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbSelectedDivision(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbSelectedStatus(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void BtnClickReset(object sender, RoutedEventArgs e)
        {
            cbSort.SelectedIndex = -1;
            cbType.SelectedIndex = -1;
            cbDivision.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;

        }
    }
}
