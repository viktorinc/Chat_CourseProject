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
    public partial class Window1 : Window
    {
        public string username { get; set; }
        public string password { get; set; }
        Dictionary<string, string> dict;
        public Window1()
        {
            
        }
        private void Log_In_Btn(object sender, RoutedEventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream("../../User_Base.dat", FileMode.OpenOrCreate))
                {
                    dict = (Dictionary<string, string>)formatter.Deserialize(fs);
                    fs.Close();
                }
                foreach (var el in dict)
                {
                    if (el.Key==username && el.Value==password)
                    {
                        this.Close();
                    }
                }
                 MessageBox.Show("Error", "Incorrect login or password"); 
            }
            catch { }
        }
        private void Reg_Btn(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
