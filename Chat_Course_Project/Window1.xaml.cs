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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Shapes;

namespace Chat_Course_Project
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
       public List<User> users=new List<User>();
        public User curr_user;
        public Login()
        {
            InitializeComponent();
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        private void Log_In_Btn(object sender, RoutedEventArgs e)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    users = (List<User>)formatter.Deserialize(fs);
                }
            }
            catch { }
            try
            {
                foreach (var el in users)
                {
                    if (usrname.Text == el.name)
                    {
                        curr_user = el;
                    }
                }
            }
            catch { }
            if (curr_user == null)
            {
                User new_user = new User(usrname.Text, 0);
                users.Add(new_user);
                curr_user = new_user;
                using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, users);
                }
            }
            this.Close();

        }

    }

}
