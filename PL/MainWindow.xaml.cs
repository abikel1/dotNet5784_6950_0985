using BlApi;
using PL.Task;
using PL.Worker;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));



        public bool isMennager
        {
            get { return (bool)GetValue(isMennagerProperty); }
            set { SetValue(isMennagerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isMennager.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isMennagerProperty =
            DependencyProperty.Register("isMennager", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));


        public BO.User User
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow(BO.User _user)
        {
            isMennager=_user.isMennager;
            User = _user;
            CurrentTime = s_bl.Clock;
            InitializeComponent();
        }

        private void btnWorkers_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();
        }

        private void btnInitialization_click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to initialization the data",
                            "Initialization",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                s_bl.InitializeDB();
            }
        }

        private void btnReset_click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to reset the data",
                            "Reset",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow(User).Show();
        }

        private void btnSchedule(object sender, RoutedEventArgs e)
        {
            new StartDateWindow().ShowDialog();
        }

        private void AdvanceHour(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.AdvanceByHour();
        }

        private void AdvanceDay(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.AdvanceByDay();
        }
        private void AdvanceYear(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.AdvanceByYear();
        }

        private void ResetTime(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.ResetClock();
        }
    }
}
