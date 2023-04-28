using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace KeeperProUL.Pages
{
    public partial class ListApplications : Page
    {
        public KeeperProULEntities database;

        public ListApplications()
        {
            InitializeComponent();

            database = new KeeperProULEntities();

            BindingUser();
        }

        public void BindingUser()
        {
            ObservableCollection<Application> applications = new ObservableCollection<Application>(database.Applications.ToList());

            lvApplications.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = applications,
            });
        }
    }
}
