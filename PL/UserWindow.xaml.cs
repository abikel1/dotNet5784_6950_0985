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

namespace PL
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.User user
        {
            get { return (BO.User)GetValue(userProperty); }
            set { SetValue(userProperty, value); }
        }

        // Using a DependencyProperty as the backing store for user.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty userProperty =
            DependencyProperty.Register("user", typeof(BO.User), typeof(UserWindow), new PropertyMetadata(null));


        public bool _isUpdate
        {
            get { return (bool)GetValue(_isUpdateProperty); }
            set { SetValue(_isUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _isUpdate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _isUpdateProperty =
            DependencyProperty.Register("_isUpdate", typeof(bool), typeof(UserWindow), new PropertyMetadata(null));


        public int IdUser
        {
            get { return (int)GetValue(IdUserProperty); }
            set { SetValue(IdUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdUserProperty =
            DependencyProperty.Register("IdUser", typeof(int), typeof(UserWindow), new PropertyMetadata(null));





        public UserWindow(bool isUpdate,int id=0)
        {
            IdUser = id;
            user=new BO.User();
            _isUpdate=isUpdate;
            user.Id = id;
            InitializeComponent();
        }

        private void btnAddUser(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!_isUpdate)
                {
                    s_bl.User.Create(user);
                    MessageBox.Show("The operation of adding a user was performed successfully");
                }
                else
                {
                    s_bl.User.Update(user);
                    MessageBox.Show("The operation of update a user was performed successfully");
                }
                //if(s_bl.User.checkWorkers(user)==true)//the user doesnt exist in the worker list
                //{
                //    if (MessageBox.Show("the id is not of worker, Do you want to create a mennager?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //    {
                //        s_bl.User.checkMennager(user);
                //        MessageBox.Show("The operation of adding a mennager was performed successfully");
                //    }
                //}
                //else
                //{
                //    s_bl.User.Create(user);
                //    
                //}
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
