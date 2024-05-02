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
    /// Логика взаимодействия для DeliveryPurchase.xaml
    /// </summary>
    public partial class DeliveryPurchase : Page
    {
        private Purchase _currentPurchase = new Purchase();
        private Customer _currentCustomer = new Customer();

        public DeliveryPurchase(Purchase _selectedtpurchase, Customer _selectedCustomer)
    {
        InitializeComponent();
        _currentPurchase = _selectedtpurchase;
            _currentCustomer = _selectedCustomer;
        DataContext = _currentPurchase;

    }



        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentPurchase.Street))
                errors.AppendLine("Укажите улицу клиента!");
            if (string.IsNullOrWhiteSpace(_currentPurchase.House))
                errors.AppendLine("Укажите дом!");
            if ((_currentPurchase.Apartment == null))
                errors.AppendLine("Укажите квартиру!");




            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!");
                return;
 
            }
            try {
                var purchaseJson = JsonConvert.SerializeObject(_currentPurchase);
                var content = new StringContent(purchaseJson, Encoding.UTF8, "application/json");


                if (_currentPurchase.IdCustomer ==null)
                {
                    HttpResponseMessage response = await Store.client.PostAsync(Store.APP_PATH + "/api/purchases", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Заказ успешно добавлен");
                        Manager.MainFrame.GoBack();

                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении заказа");

                    }

                }
                else
                {

                    HttpResponseMessage response = await Store.client.PutAsync(Store.APP_PATH + "/api/purchases/" + _currentPurchase.IdCustomer, content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Данные успешно изменены!");
                        Manager.MainFrame.Navigate(new PurchasesPage(_currentCustomer));

                    }
                    else
                    {
                        MessageBox.Show("Ошибка при изменении данных");
                    }

           
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}