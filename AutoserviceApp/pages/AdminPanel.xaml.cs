using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AutoserviceApp.pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        private bool initComplete = false;
        private Frame mainFrame;
        private int fullClientsCount;
        private int foundedClientsCount;
        private int outElement = 10;
        private int pageNumber = 0;
        private int pageCount = 0;
        private int codeGender = 0;
        private int sortCode = 0;
        private string search = "";
        private List<Client> clients;

        public AdminPanel(Frame main)
        {
            InitializeComponent();

            this.mainFrame = main;

            clients = DataBase.GetContext().Client.ToList();
            fullClientsCount = clients.Count;
            
            initComplete = true;
        }

        public void Load()
        {
            try
            {
                IQueryable<Client> res = DataBase.GetContext().Client;

                fullClientsCount = res.ToList().Count;

                res = FilterService.Filters(res, search, codeGender);

                foundedClientsCount = res.ToList().Count;

                res = SortService.Sort(res, sortCode);

                clients = res.Skip(pageNumber * outElement).Take(outElement).ToList();

                textBlockCount.Text = "Вывелось записей " + foundedClientsCount + " из " + fullClientsCount;
                clientDataGrid.ItemsSource = clients;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            int ost = foundedClientsCount % outElement;
            pageCount = (foundedClientsCount - ost) / outElement;
            if (ost > 0) pageCount++;

            panelPage.Children.Clear();

            for (int i = 0; i < pageCount; i++)
            {
                Button btn = new Button();

                btn.Height = 30;
                btn.Content = (i + 1).ToString();
                btn.Width = 20;
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.Tag = i;
                btn.Click += new RoutedEventHandler(PanelPageButto_Click);
                panelPage.Children.Add(btn);
            }

            TurnButton();
        }

        private void TurnButton()
        {
            if (pageNumber == 0) { ButtonBack.IsEnabled = false; }
            else { ButtonBack.IsEnabled = true; };

            if ((pageNumber + 1) * outElement >= foundedClientsCount) { ButtonNext.IsEnabled = false; }
            else { ButtonNext.IsEnabled = true; };
        }

        private void ButtonOut10_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber = 0;
            outElement = 10;
            Load();
        }

        private void ButtonOut50_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber = 0;
            outElement = 50;
            Load();
        }

        private void ButtonOut200_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber = 0;
            outElement = 200;
            Load();
        }

        private void ButtonOutAll_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber = 0;
            outElement = fullClientsCount;
            Load();
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            Load();
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            pageNumber--;
            Load();
        }
        private void PanelPageButto_Click(object sender, RoutedEventArgs e)
        {
            pageNumber = Convert.ToInt32(((Button)sender).Tag.ToString());
            Load();
        }

        private void ComboBoxGender_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            codeGender = Convert.ToInt32(((ComboBoxItem)comboBoxGender.SelectedItem).Tag);

            if (initComplete)
            {
                Load();
            }
        }

        private void sortCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sortCode = Convert.ToInt32(((ComboBoxItem)sortCombobox.SelectedItem).Tag);

            if (initComplete)
            {
                Load();
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            search = ((TextBox)sender).Text;
            Load();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var context = DataBase.GetContext();

            var selected = (Client)clientDataGrid.SelectedItem;

            if (selected == null)
                return;

            var countOfVisit = context.Client.Where(o => o.ID == selected.ID).Select(o => new { clientService = o.ClientService }).ToList().First().clientService.Count;

            if (countOfVisit != 0)
            {
                MessageBox.Show("Этого пользователя нельзя удалить!");
                return;
            }

            context.Client.Remove(selected);
            context.SaveChanges();
            Load();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = (Client)clientDataGrid.SelectedItem;

            if (selectedClient == null)
            {
                MessageBox.Show("Сперва выбирете пользователя, которого хотите отредактировать!");
                return;
            }

            var win = new ClientAddEdit(selectedClient);
            win.Show();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new ClientAddEdit();
            win.Show();
        }
    }
}
