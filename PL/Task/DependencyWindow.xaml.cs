using BO;
using Microsoft.VisualBasic;
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
    /// Interaction logic for DependencyWindow.xaml
    /// </summary>
    public partial class DependencyWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private event Action<TaskInList> _updateDependency;
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(DependencyWindow), new PropertyMetadata(null));
        int _id;
        public DependencyWindow(int id, Action<TaskInList> updateDependency)
        {
            _updateDependency = updateDependency;
            InitializeComponent();
            TaskList = s_bl.Task.ReadPossibleDependencies(id);
            _id = id;
        }


        private void AddDependency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BO.TaskInList? task = (sender as DataGrid)?.SelectedItem as BO.TaskInList;    ;

                _updateDependency(task);

                s_bl.TaskInList.Add(_id, task!.Id);
                this.Close();
                MessageBox.Show("Dependency added successfully!");
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message,"ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
