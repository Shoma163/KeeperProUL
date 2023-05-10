using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
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
using static System.Net.Mime.MediaTypeNames;

namespace KeeperProUL
{
    class Time
    {
        public int Value { get; set; }
        public string ValuseString { get; set; }
    }

    public partial class ApplicationsInfo : Page
    {
        public KeeperProULEntities database;
        public ApplicationsInfo()
        {
            InitializeComponent();

            database = new KeeperProULEntities();

            BindingUserEditingPages();

            Time();

            BindingCbEditingStatus();
        }

        public void BindingCbEditingStatus()
        {
            ObservableCollection<Status> statuses = new ObservableCollection<Status>(database.Status.ToList());

            cbEditStatus.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = statuses
            });
        }

        public void Time()
        {
            List<Time> hours = new List<Time>();
            List<Time> minutes = new List<Time>();

            cbEditTimeHour.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = hours,
            });
            cbeditTimeMinuts.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = minutes,
            });


            for (int i = 0; i < 24; i++)
            {
                hours.Add(new Time() {  Value = i, ValuseString = string.Format("{0:00}", i)});
            }
            for (int i = 0; i < 60; i++)
            {
                minutes.Add(new Time() { Value = i, ValuseString = string.Format("{0:00}", i) });
            }

            //TimeSpan time = new TimeSpan((cbEditTimeHour.SelectedItem as Time).Value, (cbeditTimeMinuts.SelectedItem as Time).Value,0);

        }
        public void BindingUserEditingPages()
        {
            ObservableCollection<Application> applications = new ObservableCollection<Application>(database.Applications.ToList());

            lvApplicationsPageInfo.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = applications,
            });


        }

        private void bcSaveEdit(object sender, RoutedEventArgs e)
        {
            database.SaveChanges();
        }

        private void lvApplicationsPageInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var UserInfo = lvApplicationsPageInfo.SelectedItem as Application;

            spEditInfoUser.DataContext = UserInfo;

        }
    }
}
