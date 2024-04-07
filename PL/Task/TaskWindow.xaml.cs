using BlApi;
using PL.Worker;
using BO;
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
        private event Action<int, bool> _onAddOrUpdate;
        private bool _isUpdate;
        private int _id;

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow));


        public bool isMennager
        {
            get { return (bool)GetValue(isMennagerProperty); }
            set { SetValue(isMennagerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isMennager.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isMennagerProperty =
            DependencyProperty.Register("isMennager", typeof(bool), typeof(TaskWindow));


        public bool StatusTask
        {
            get { return (bool)GetValue(StatusTaskProperty); }
            set { SetValue(StatusTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusTaskProperty =
            DependencyProperty.Register("StatusTask", typeof(bool), typeof(TaskWindow), new PropertyMetadata(null));





        public TaskWindow(int id = 0, bool _isMennager = true, Action<int, bool> onAddOrUpdate = null)
        {
            if (id == 0)
            {
                Task = new BO.Task();
                Task.DependencyTasks = new List<TaskInList>();
                Task.Worker = new();
                _isUpdate = false;
            }
            else
            {
                Task = s_bl.Task.Read(id);
                _isUpdate = true;
                if (isMennager)
                    StatusTask = false;
                else
                {
                    StatusTask = Task.StatusTask == Status.Started ? true : false;
                }
                if (Task.DependencyTasks is null)
                    Task.DependencyTasks = new List<TaskInList>();
                if (Task.Worker is null)
                    Task.Worker = new();

            }
            isMennager = _isMennager;
            _onAddOrUpdate = onAddOrUpdate;
            InitializeComponent();
        }

        private void btnAddOrUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isUpdate)
                {
                    s_bl.Task.UpdateTask(Task);
                    _onAddOrUpdate(Task.Id, true);
                    MessageBox.Show("The operation of updating a task was performed successfully");
                    this.Close();
                }
                else
                {
                    _id = s_bl.Task.AddTask(Task);
                    _onAddOrUpdate(_id, false);
                    MessageBox.Show("The operation of adding a task was performed successfully");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DependencyTasks(object sender, RoutedEventArgs e)
        {
            new DependencyWindow(Task.Id, UpdateDependendy).Show();
            this.Close();
        }

        private void UpdateDependendy(TaskInList depTask)
        {

            BO.Task tmp = new BO.Task()
            {
                Id = Task.Id,
                Alias = Task.Alias,
                TaskDescription = Task.TaskDescription,
                Difficulty = Task.Difficulty,
                StatusTask = Task.StatusTask,
                Worker = Task.Worker,
                CreateTask = Task.CreateTask,
                BeginWork = Task.BeginWork,
                BeginTask = Task.BeginTask,
                TimeTask = Task.TimeTask,
                DeadLine = Task.DeadLine,
                EndWorkTime = Task.EndWorkTime,
                Remarks = Task.Remarks,
                Product = Task.Product,
                DependencyTasks = Task.DependencyTasks.Append(depTask)
            };

            Task = null;
            Task = tmp;
        }

        private void btnDeleteDependency(object sender, RoutedEventArgs e)
        {
            BO.TaskInList? _task = (sender as Button)?.CommandParameter as BO.TaskInList;
            if (MessageBox.Show("Are you sure you want to delete the dependency?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.TaskInList.Remove(Task.Id, _task.Id);
                    MessageBox.Show("Dependency deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAddDependency(object sender, RoutedEventArgs e)
        {
            new DependencyWindow(Task.Id, UpdateDependendy).Show();
        }

        private void newOpenTask(object sender, EventArgs e)
        {
            Task = (_isUpdate ? s_bl.Task.Read(Task.Id) : new BO.Task());
        }

        private void btnFinishTask(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to finish the task?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Task.FinishTask(Task);
                    MessageBox.Show("Task finished successfully!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
