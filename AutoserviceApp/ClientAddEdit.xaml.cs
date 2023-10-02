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

namespace AutoserviceApp
{
    /// <summary>
    /// Логика взаимодействия для ClientAddEdit.xaml
    /// </summary>
    public partial class ClientAddEdit : Window
    {
        Client client;

        public ClientAddEdit(Client client=null)
        {
            this.client = client;

            InitializeComponent();
        }
    }
}
