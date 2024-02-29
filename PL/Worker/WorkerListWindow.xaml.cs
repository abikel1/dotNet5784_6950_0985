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

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for WorkerListWindow.xaml
    /// </summary>
    public partial class WorkerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Rank Rank { get; set; } = BO.Rank.None;
        public ObservableCollection<BO.Worker> WorkerList
        {
            get { return (ObservableCollection<BO.Worker>)GetValue(WorkerListProperty); }
            set { SetValue(WorkerListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkerList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkerListProperty =
            DependencyProperty.Register("WorkerList", typeof(ObservableCollection<BO.Worker>), typeof(WorkerListWindow), new PropertyMetadata(null));


        public WorkerListWindow()
        {
            InitializeComponent();
            WorkerList = s_bl?.Worker.ReadWorkers().ToObservableCollection()!;
        }
        private void onAddOrUpdate(int id, bool isUpdate)
        {
            BO.Worker workers = new BO.Worker()
            {
                Id = id,
                Name=s_bl.Worker.Read(id).Name,
                Email=s_bl.Worker.Read(id).Email,
                RankWorker=s_bl.Worker.Read(id).RankWorker,
                HourPrice=s_bl.Worker.Read(id).HourPrice,
                CurrentTask= s_bl.Worker.Read(id).CurrentTask
            };
            if (isUpdate)
            {
                var oldWorker = WorkerList.FirstOrDefault(t => t.Id == id);
                WorkerList.Remove(oldWorker!);
                WorkerList.OrderBy(t => t.Id).ToObservableCollection();
            }
            WorkerList.Add(workers);
        }
        private void cbRankSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkerList = (Rank == BO.Rank.None) ? s_bl?.Worker.ReadWorkers().ToObservableCollection()! : s_bl?.Worker.ReadWorkers(item => item.RankWorker == Rank).ToObservableCollection()!;
        }

        private void AddWorker(object sender, RoutedEventArgs e)
        {
            new WorkerWindow(onAddOrUpdate).ShowDialog();
        }
        private void UpdateWorker(object sender, MouseButtonEventArgs e)
        {
            BO.Worker? worker = (sender as DataGrid)?.SelectedItem as BO.Worker;
            new WorkerWindow(onAddOrUpdate,worker!.Id).ShowDialog();
        }

        private void RemoveWorker(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var worker = (BO.Worker)button.CommandParameter;
            //var dataGridRow = (DataGridRow)((Button)sender).Parent;
            //BO.Worker worker = (BO.Worker)dataGridRow.DataContext;
            if (MessageBox.Show("Are you sure you want to delete the worker?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Worker.RemoveWorker(worker.Id);
                    WorkerList.Remove(worker);
                    MessageBox.Show("Worker deleted successfully!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
