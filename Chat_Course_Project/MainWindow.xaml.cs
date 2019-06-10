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
        string User_Name="Anonymous"; // ім`я користувача, по дефолту - анонім
        public MainWindow()
        {
            Window1 win = new Window1();
            win.Show();
            System.Windows.Threading.DispatcherTimer DesTimer = new System.Windows.Threading.DispatcherTimer(); // таймер
            DesTimer.Tick += new EventHandler(DesTimer_Tick);
            DesTimer.Interval = new TimeSpan(0, 0, 1);
            DesTimer.Start();
        }

        private void Send_Btn_Clicked(object sender, RoutedEventArgs e)// Записує чат в файл, з добавленням імені, якшо імені не вказано, наш юзр - анонім
        {
            Main_Chat.Content += $"\n{User_Name}: {Send_Box.Text}";
            string chat =(string)Main_Chat.Content;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("../../Chat_Content.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, chat);
                fs.Close();
            }
            Send_Box.Text = "";

        }
        private void DesTimer_Tick(object sender, EventArgs e) // тік таймера, кожну секунду десереалізує файл в scrollviewer
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream("../../Chat_Content.dat", FileMode.OpenOrCreate))
                {
                    Main_Chat.Content = (string)formatter.Deserialize(fs);
                    fs.Close();
                }
            }
            catch { }
        }
    }

}
