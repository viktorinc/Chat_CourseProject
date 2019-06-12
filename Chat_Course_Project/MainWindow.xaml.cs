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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat_Course_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User current_user;// поточний юзернейм
        List<User> all_users;// всі юзери
        public MainWindow()
        {
            InitializeComponent();

            First_Load();
        }
        public void Timer_creation()
        {
            System.Windows.Threading.DispatcherTimer DesTimer = new System.Windows.Threading.DispatcherTimer(); // таймер
            DesTimer.Tick += new EventHandler(DesTimer_Tick);
            DesTimer.Interval = new TimeSpan(0, 0, 1);
            DesTimer.Start();
        }// створення таймера
        public void First_Load()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
            Login login = new Login();
            login.ShowDialog();
            BinaryFormatter formatter = new BinaryFormatter();
            current_user = login.curr_user;
            try
            {
                using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    all_users = (List<User>)formatter.Deserialize(fs);
                    string stats = "";
                    foreach (var el in all_users)
                    {
                        stats += el.ToString();
                    }
                    this.Scroll_Users.Content = stats;
                    fs.Close();
                }
            }
            catch { }
            try
            {
                using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, all_users);
                    fs.Close();
                }
            }
            catch { }
            try
            {
                Cur_user_text.Text = $"Current user: {current_user.name}";
            }
            catch { }
            string hist = $"{current_user.name}: {DateTime.Now.ToShortDateString()}";
            using (StreamWriter w = File.AppendText("../../Connection_History.txt"))
            {
               w.WriteLine($"{current_user.name}: {DateTime.Now}");
               w.Close();
            }
            Timer_creation();
        }// перший запуск, оновлення поточного юзера, історії і т.д
        private void Send_Btn_Clicked(object sender, RoutedEventArgs e)// Записує чат в файл, з добавленням імені, якшо імені не вказано, наш юзр - анонім
        {
            try
            {
                Main_Chat.Content += $"\n{current_user.name}: {Send_Box.Text}\t\t   \t{DateTime.Now.ToShortTimeString()}";
            }
            catch
            {
                MessageBox.Show("Login first please");
            }
            string chat =(string)Main_Chat.Content;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("../../Chat_Content.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, chat);
                fs.Close();
            }
            Send_Box.Text = "";
            for(int i=0; i<=all_users.Count;i++)
            {
                if (all_users[i].name==current_user.name)
                {
                    all_users[i].count++;
                    break;
                }
            }
            using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, all_users);
                    fs.Close();
                }

        }
        private void DesTimer_Tick(object sender, EventArgs e) // тік таймера, кожну секунду десереалізує файл в scrollviewer
        {
            Time.Content = $"{DateTime.Now.ToShortTimeString()}";
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream("../../Chat_Content.dat", FileMode.OpenOrCreate))
                {
                    Main_Chat.Content = (string)formatter.Deserialize(fs);
                    fs.Close();
                }
                using (FileStream fs = new FileStream("../../Users.dat", FileMode.OpenOrCreate))
                {
                    all_users = (List<User>)formatter.Deserialize(fs);
                    string stats="";
                    foreach(var el in all_users)
                    {
                        stats += el.ToString();

                    }
                    this.Scroll_Users.Content = stats;
                    
                    fs.Close();
                }
                

            }
            catch { }

        }
        private void Login_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
            current_user= login.curr_user;
            Cur_user_text.Text = $"Current user: {current_user.name}";
        }// міняємо юзера

        private void History_Clicked(object sender, RoutedEventArgs e)
        {
            Window2 history = new Window2();
            history.ShowDialog();
        }

        private void Contacts_Clicked(object sender, RoutedEventArgs e)
        {
            Window3 contacts_menu = new Window3(current_user);
            contacts_menu.ShowDialog();
        }
    }
    [Serializable]
    public class User
    {
       public string name { get; set; }
       public int count { get; set; }
        public Dictionary<string, string> direct_messages = new Dictionary<string, string>();
       public User(string n, int c)
        {
            name = n;count = c;
        }
        public override string ToString()
        {
            return $"{name}: {count.ToString()}\n";
        }
    }

}
