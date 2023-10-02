using AutoserviceApp.pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoserviceApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new AdminPanel(MainFrame);
        }

        private void FrameOnLoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                AdminPanel mf = (AdminPanel)e.Content;
                mf.Load();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
            }
            
        }
    }

    

    public class DataBase
    {
        public static AutoserviceEntities ent;

        public static AutoserviceEntities GetContext()
        {
            if (ent == null) ent = new AutoserviceEntities();
            
            return ent;
        }
    }
}
