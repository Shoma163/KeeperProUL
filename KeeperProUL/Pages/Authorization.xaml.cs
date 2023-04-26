using System;
using System.Collections.Generic;
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
    public partial class Authorization : Page
    {
        public KeeperProULEntities database;
        public Authorization()
        {
            InitializeComponent();

            database = new KeeperProULEntities();
        }

        private void btnSingIn(object sender, RoutedEventArgs e)
        {
            int codeStaff = int.Parse(tbCodeStaff.Text);

            Staff staff = database.Staffs.Where(u => u.CodeStaff == codeStaff).FirstOrDefault();
            if (staff == null)
            {
                MessageBox.Show("Неверный код сотрудника");
                return;
            }
        }
    }
}
