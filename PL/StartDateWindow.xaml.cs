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

namespace PL
{
    /// <summary>
    /// Interaction logic for StartDateWindow.xaml
    /// </summary>
    public partial class StartDateWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(StartDateWindow), new PropertyMetadata(null));


        public StartDateWindow()
        {
            InitializeComponent();
            StartDate=DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.SetStartProjectDate(StartDate);
                s_bl.Task.autoSchedule();
                MessageBox.Show("The operation of updating a date of start prokect was performed successfully");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
