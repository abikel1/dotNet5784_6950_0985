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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Rank Rank { get; set; } = BO.Rank.None;

        public BO.Status Status { get; set; } = BO.Status.None;

        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


        public bool isMennager
        {
            get { return (bool)GetValue(isMennagerProperty); }
            set { SetValue(isMennagerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isMennager.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isMennagerProperty =
            DependencyProperty.Register("isMennager", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));



        public TaskListWindow(BO.User? user=null)
        {
            if (user!=null&&user.isMennager==false)
            {
                TaskList=s_bl.Task.relevantTask(s_bl.Worker.Read(user.Id)).ToObservableCollection();
                isMennager = false;
            }
            else
            {
                TaskList = s_bl.Task.ReadTaskInList().ToObservableCollection();
                isMennager = true;
            }
            InitializeComponent();
        }
        private void onAddOrUpdate(int id, bool isUpdate)
        {
            BO.TaskInList taskInLIst = new BO.TaskInList()
            {
                Id = id,
                StatusTask = s_bl.Task.Read(id)!.StatusTask,
                Alias = s_bl.Task.Read(id)!.Alias,
                Description = s_bl.Task.Read(id)!.TaskDescription,
            };
            if(isUpdate)
            {
                var oldTask= TaskList.FirstOrDefault(t=>t.Id==id);
                TaskList.Remove(oldTask!);
                TaskList.OrderBy(t => t.Id).ToObservableCollection();
            }
            TaskList.Add(taskInLIst);
        }
        private void cbSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Status==BO.Status.None&&Rank==BO.Rank.None)
            {
                return;
            }
            if(Status==BO.Status.None)
            {
                TaskList = s_bl?.Task.ReadTaskInList(item => s_bl.Task.Read(item.Id).Difficulty == Rank).ToObservableCollection()!;
                return;
            }
            if (Rank == BO.Rank.None)
            {
                TaskList = s_bl?.Task.ReadTaskInList(item => item.StatusTask == Status).ToObservableCollection()!;
                return;
            }
            else
            {
                TaskList = s_bl?.Task.ReadTaskInList(item =>(item.StatusTask==Status)&&(s_bl.Task.Read(item.Id).Difficulty == Rank)).ToObservableCollection()!;
            }
        }

        private void UpdateTask(object sender, MouseButtonEventArgs e)
        {
                BO.TaskInList? task = (sender as DataGrid)?.SelectedItem as BO.TaskInList;
                new TaskWindow(onAddOrUpdate, task!.Id).Show();
        }
        private void AddTask(object sender, RoutedEventArgs e)
        {
            new TaskWindow(onAddOrUpdate).Show();
        }

        private void btnDeleteTask(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var task = (BO.TaskInList)button.CommandParameter;
            if (MessageBox.Show("Are you sure you want to delete the task?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Task.RemoveTask(task.Id);
                    TaskList.Remove(task);
                    MessageBox.Show("Task deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
