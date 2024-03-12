using BO;
using DalApi;
using PL.Worker;
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
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TasksForWorkerWindow.xaml
    /// </summary>
    public partial class TasksForWorkerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TasksForWorkerWindow), new PropertyMetadata(null));


        public TasksForWorkerWindow(int id)
        {
            TaskList = s_bl.Worker.GetAssociatedTasksForWorker(id).ToObservableCollection();
            InitializeComponent();
        }

        private void btnStartTask(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var task = (BO.TaskInList)button.CommandParameter;
                if (MessageBox.Show("Are you sure you want to start the task?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    s_bl.Task.UpdateBeginDateOfTask(task.Id);
                    MessageBox.Show("Task startsd successfully!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteTask(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var task = (BO.TaskInList)button.CommandParameter;
                if (MessageBox.Show("Are you sure you want to delete the task?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    s_bl.Task.DeleteWorkerFromTask(task.Id);
                    TaskList.Remove(task);
                    MessageBox.Show("Task deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
