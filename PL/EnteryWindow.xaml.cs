using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for EnteryWindow.xaml
    /// </summary>
    public partial class EnteryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public BO.User User
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
      DependencyProperty.Register("User", typeof(BO.User), typeof(EnteryWindow));


        public EnteryWindow()
        {
            User=new BO.User();
            InitializeComponent();
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.User.checkInvalid(User);
                User = s_bl.User.Read(User.userName!)!;
                new MainWindow(User).Show();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddUser(object sender, RoutedEventArgs e)
        {
            new UserWindow().ShowDialog();
        }
    }
}
