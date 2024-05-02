using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace MebelMag
{
    /// <summary>
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
            LoadCustomers();
        }
        public async void LoadCustomers()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/customers");

            if (response.IsSuccessStatusCode)
            {
                var customersJson = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);

                DgridStore.ItemsSource = customers;

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            Manager.MainFrame.Navigate(new PurchasesPage((sender as Button).DataContext as Customer));
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            int customerId = (int)btn.CommandParameter;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            if (MessageBox.Show($"Вы точно хотите удалить этого клиента?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
#pragma warning disable IDE0059 // Ненужное присваивание значения
                try
                {


                    HttpResponseMessage response = await Store.client.DeleteAsync(($"{Store.APP_PATH}/api/Customers/{customerId}"));

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Клиент успешно удален.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Клиент не найден.");
                    }
                    LoadCustomers();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("");
                }
#pragma warning restore IDE0059 // Ненужное присваивание значения
            }
        }


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            Manager.MainFrame.Navigate(new AddEditCustomerPage((sender as Button).DataContext as Customer));
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditCustomerPage(null));
        }

        private void DgridStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            HttpResponseMessage response;
            if (Email.Text != "")
                response = await Store.client.GetAsync($"{Store.APP_PATH}/api/customers/email?email={Email.Text}");
            else
                response = await Store.client.GetAsync(Store.APP_PATH + "/api/customers");


            if (response.IsSuccessStatusCode)
            {
                var customersJson = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);
                DgridStore.ItemsSource = customers;

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }
    }
}


