using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat_Course_Project
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public List<User> contacts;
        public User user_To_write;
        public User current_User;
        public Window3(User curr_user)
        {
            InitializeComponent();
            current_User = curr_user;
            tmp();
        }
        public void tmp()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    contacts = (List<User>)formatter.Deserialize(fs);
                    foreach(var el in contacts)
                    {
                        Contacts.Items.Add(el.name);
                    }
                    fs.Close();
                }


            }
            catch { }
        }

        private void Contacts_Click(object sender, MouseButtonEventArgs e)
        {
            string firstSelectedItem = (string)Contacts.SelectedItems[0];
            foreach(var el in contacts)
            {
                if(el.name==firstSelectedItem)
                {
                    user_To_write = el;
                    break;
                }
            }
            Window4 direct = new Window4(current_User, user_To_write);
            this.Close();
            direct.ShowDialog();
           


        }
    }
}
