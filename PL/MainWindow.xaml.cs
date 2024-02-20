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
        public MainWindow(bool isManeger)
        {
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
            new TaskListWindow().Show();
        }
    }
}
