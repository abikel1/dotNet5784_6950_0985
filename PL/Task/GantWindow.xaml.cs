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
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GantWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.Gant> TaskOfList
        {
            get { return (IEnumerable<BO.Gant>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskOfList", typeof(IEnumerable<BO.Gant>), typeof(GantWindow), new PropertyMetadata(null));
        public GantWindow()
        {
            InitializeComponent();
            TaskOfList = s_bl?.Task.GetDetailsToGantt()!;
        }
    }
}
