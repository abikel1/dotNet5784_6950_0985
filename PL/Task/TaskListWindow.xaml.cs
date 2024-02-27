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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Rank Rank { get; set; } = BO.Rank.None;

        public BO.Status Status { get; set; } = BO.Status.None;

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


        public TaskListWindow()
        {
            InitializeComponent();
            TaskList=s_bl.Task.ReadTasks();
        }
        private void cbRankSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            TaskList = (Status == BO.Status.None) ? s_bl?.Task.ReadTasks()! : s_bl?.Task.ReadTasks(item => item.StatusTask == Status)!;
        }

        private void UpdateTask(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task=(sender as DataGrid)?.SelectedItem as BO.TaskInList;
            new TaskWindow(task!.Id).ShowDialog();
            this.Close();
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            new TaskWindow().Show();
            this.Close();
        }
    }
}
