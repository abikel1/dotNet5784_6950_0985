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

namespace PL.Worker
{
    /// <summary>
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private event Action<int, bool> _onAddOrUpdate;
        private bool _isUpdate;

        public BO.Worker worker
        {
            get { return (BO.Worker)GetValue(workerProperty); }
            set { SetValue(workerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for worker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty workerProperty =
            DependencyProperty.Register("worker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));


        public WorkerWindow(Action<int, bool> onAddOrUpdate, int id=0)
        {
            InitializeComponent();
            if(id== 0)
            {
                 worker = new BO.Worker();
                _isUpdate = false;
            }
            else
            {
               worker= s_bl.Worker.Read(id);
                _isUpdate = true;
            }
            _onAddOrUpdate = onAddOrUpdate;
        }

        private void btnAddOrUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                if(_isUpdate)
                {
                    s_bl.Worker.UpdateWorker(worker);
                    _onAddOrUpdate(worker.Id, true);
                    MessageBox.Show("The operation of updating a worker was performed successfully");
                    this.Close();
                }
                else
                {
                    int id=s_bl.Worker.AddWorker(worker);
                    _onAddOrUpdate(worker.Id, false);
                    MessageBox.Show("The operation of adding a worker was performed successfully");
                    this.Close();
                }
                //if (s_bl.Worker.ReadWorkers().FirstOrDefault(w=>w.Id==worker.Id)==null)
                //{
                //    s_bl.Worker.AddWorker(worker);
                //    _onAddOrUpdate(worker.Id, false);
                //    MessageBox.Show("The operation of adding a worker was performed successfully");
                //    this.Close();
                //}
                //else
                //{
                //    s_bl.Worker.UpdateWorker(worker);
                //    _onAddOrUpdate(worker.Id, true);
                //    MessageBox.Show("The operation of updating a worker was performed successfully");
                //    this.Close();
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
