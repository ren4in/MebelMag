using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
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
    /// Логика взаимодействия для AddEditCustomerPage.xaml
    /// </summary>
    public partial class AddEditCustomerPage : Page
    {
        private Customer _currentCustomer = new Customer();

        public AddEditCustomerPage(Customer selectedCustomer)
        {
            InitializeComponent();
            if (selectedCustomer != null)
            {
                _currentCustomer = selectedCustomer;
            }
            DataContext = _currentCustomer;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrWhiteSpace(_currentCustomer.LastName))
                    errors.AppendLine("Укажите фамилию клиента!");
                if (string.IsNullOrWhiteSpace(_currentCustomer.FirstName))
                    errors.AppendLine("Укажите имя клиента!");
                if (string.IsNullOrWhiteSpace(_currentCustomer.MiddleName))
                    errors.AppendLine("Укажите отчество клиента!");
                if (string.IsNullOrWhiteSpace(_currentCustomer.EMail))
                    errors.AppendLine("Укажите электронную почту клиента!");
                if (string.IsNullOrWhiteSpace(_currentCustomer.PhoneNumber))
                    errors.AppendLine("Укажите телефонный номер клиента!");
                if (Check.CheckEmail(_currentCustomer.EMail) == false)
                    errors.AppendLine("Адрес электронной почты не соответствует формату!");
                if (Check.CheckPhone(_currentCustomer.PhoneNumber) == false)
                    errors.AppendLine("Номер телефона не соответствует формату!");



                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Внимание!");
                    return;
                }

                   
                    var customerJson = JsonConvert.SerializeObject(_currentCustomer);
                    customerJson = customerJson.Replace(@",""IdCustomer"": null", "");

                var content = new StringContent(customerJson, Encoding.UTF8, "application/json");
                
                if (_currentCustomer.IdCustomer == null)
                {

                    HttpResponseMessage response = await Store.client.PostAsync(Store.APP_PATH + "/api/customers", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Клиент успешно добавлен");
                        Manager.MainFrame.Navigate(new CustomersPage());

                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении клиента");

                    }
                }
                else
                {
                    HttpResponseMessage response = await Store.client.PutAsync(Store.APP_PATH + "/api/customers/" + _currentCustomer.IdCustomer, content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Данные успешно изменены!");
                        Manager.MainFrame.Navigate(new CustomersPage());

                    }
                    else
                    {
                        MessageBox.Show("Ошибка при изменении данных");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Внимание!");
            }
        }
    }
}