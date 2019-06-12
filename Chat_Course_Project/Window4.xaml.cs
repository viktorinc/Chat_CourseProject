using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chat_Course_Project
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        User curr;
        User write;
        List<User> all_users;
        public Window4(User curr_user,User user_to_write)
        {
            InitializeComponent();
            Timer_creation();
            curr = curr_user;
            write = user_to_write;             

               
        }
        public void send_direct()
        {

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
            {
                all_users = (List<User>)formatter.Deserialize(fs);

                fs.Close();
            }
            foreach (var el in all_users)
            {
                if (el.name == curr.name)
                {
                    curr = el;
                }
                if (el.name == write.name)
                {
                    write = el;
                }
            }
            try
            {
                string message = "";
                message += $"{curr.name}: {usr_message.Text}\n";
                curr.direct_messages[write.name] += message;
                write.direct_messages[curr.name] += message;
            }
            catch
            {
                try
                {
                    curr.direct_messages.Add(write.name, "");
                    write.direct_messages.Add(curr.name, "");
                }
                catch { }
            }
            using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
            {
                for (int i = 0; i < all_users.Count(); i++)
                {
                    if (all_users[i].name == curr.name)
                    {
                        all_users[i] = curr;
                    }
                    if (all_users[i].name == write.name)
                    {
                        all_users[i] = write;
                    }
                }
                formatter.Serialize(fs, all_users);

                fs.Close();
            }
            Direct.Content = curr.direct_messages[write.name];





        }
        public void Timer_creation()
        {
            System.Windows.Threading.DispatcherTimer DesTimer = new System.Windows.Threading.DispatcherTimer();
            DesTimer.Tick += new EventHandler(DesTimer_Tick);
            DesTimer.Interval = new TimeSpan(0, 0, 1);
            DesTimer.Start();
        }
        private void DesTimer_Tick(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
            {
                all_users = (List<User>)formatter.Deserialize(fs);

                fs.Close();
            }
            foreach (var el in all_users)
            {
                if (el.name == curr.name)
                {
                    curr = el;
                }
                if (el.name == write.name)
                {
                    write = el;
                }
            }
            using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
            {
                for (int i = 0; i < all_users.Count(); i++)
                {
                    if (all_users[i].name == curr.name)
                    {
                        all_users[i] = curr;
                    }
                    if (all_users[i].name == write.name)
                    {
                        all_users[i] = write;
                    }
                }
                formatter.Serialize(fs, all_users);

                fs.Close();
            }
            try
            {
                Direct.Content = write.direct_messages[curr.name];
            }
            catch
            {
                try
                {
                    curr.direct_messages.Add(write.name, "");
                    write.direct_messages.Add(curr.name, "");
                }
                catch
                {

                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            send_direct();
        }
    }
}
