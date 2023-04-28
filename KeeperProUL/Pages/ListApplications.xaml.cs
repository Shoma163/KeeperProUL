using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

            database.Purposes.Select(p => p.Group).Distinct().ToList();

            SortDescriptions = new List<SortItem>()
            {
                new SortItem()
                {
                    Name = "Сортировка по статусу",
                    Sort = new SortDescription("Status", ListSortDirection.Ascending),
                },
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
            view.Filter = new Predicate<object>(o =>
            {
                Application application = o as Application;
                if (application == null) { return false; }

                bool isVisible = true;
                if (searchString.Length > 0)
                {
                    isVisible = application.Status.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 ||
                    application.User1.LastName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 ||
                    application.Purpose1.Staff1.Division.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1;
                }

                return isVisible;
            });
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
    }
}
