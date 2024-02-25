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
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));


        public TaskWindow(int id=0)
        {
            InitializeComponent();
            if (id == 0) 
            {
                Task=new BO.Task();
            }
            else
            {
                Task = s_bl.Task.Read(id);
            }
        }

        private void btnAddOrUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                if (s_bl.Task.ReadTasks().FirstOrDefault(w => w.Id == Task.Id) == null)
                {
                    s_bl.Task.AddTask(Task);
                    MessageBox.Show("The operation of adding a task was performed successfully");
                    this.Close();
                    new TaskListWindow().Show();
                }
                else
                {
                    s_bl.Task.UpdateTask(Task);
                    MessageBox.Show("The operation of updating a task was performed successfully");
                    this.Close();
                    new TaskListWindow().Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DependencyTasks(object sender, RoutedEventArgs e)
        {
            new DependencyWindow(Task.Id).Show();
            this.Close();
        }

        private void btnDeleteDependency(object sender, RoutedEventArgs e)
        {
            BO.TaskInList _task = (sender as Button)?.CommandParameter as BO.TaskInList;
            if (MessageBox.Show("Are you sure you want to delete the dependency?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.TaskInList.Remove(Task.Id, _task.Id);
                    MessageBox.Show("Dependency deleted successfully!");
                    this.Close();
                    new TaskWindow(Task.Id).Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAddDependency(object sender, RoutedEventArgs e)
        {
            new DependencyWindow(Task.Id).Show();
        }
    }
}
