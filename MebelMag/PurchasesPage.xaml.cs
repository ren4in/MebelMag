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
    /// Логика взаимодействия для PurchasesPage.xaml
    /// </summary>
    public partial class PurchasesPage : Page
    {
        public PurchasesPage(Customer customer)
        {
            InitializeComponent();
            _current_Customer_Id = customer.IdCustomer;
           _currentCustomer= customer;
            LoadPurchases();
        }
#pragma warning disable IDE0044 // Добавить модификатор только для чтения
        int? _current_Customer_Id;
#pragma warning restore IDE0044 // Добавить модификатор только для чтения
#pragma warning disable IDE0044 // Добавить модификатор только для чтения
        Customer _currentCustomer;
#pragma warning restore IDE0044 // Добавить модификатор только для чтения

        public async void LoadPurchases()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/Purchases/id_customer?id_customer=" + _current_Customer_Id);

            if (response.IsSuccessStatusCode)
            {
                var purchasesJson = await response.Content.ReadAsStringAsync();
                var purchases = JsonConvert.DeserializeObject<List<Purchase>>(purchasesJson);

                DgridStore.ItemsSource = purchases;

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }
#pragma warning disable IDE0051 // Удалите неиспользуемые закрытые члены
     private void Page_Loaded()
#pragma warning restore IDE0051 // Удалите неиспользуемые закрытые члены
        {
            LoadPurchases();
        }
     private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable IDE0017 // Упростите инициализацию объекта
#pragma warning disable IDE0090 // Используйте "new(...)".
            Purchase newpurchase = new Purchase();
#pragma warning restore IDE0090 // Используйте "new(...)".
#pragma warning restore IDE0017 // Упростите инициализацию объекта
            newpurchase.PurchaseDateTime = DateTime.Now;
           newpurchase.IdCustomer = _current_Customer_Id;
         //   newpurchase.IdCustomerNavigation = _currentCustomer;
            newpurchase.IdUser = Store.userId;
            newpurchase.Status = "Не оплачен";

             
            if (MessageBox.Show($"Оформить доставку?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                newpurchase.Delivery = true;
                Manager.MainFrame.Navigate(new DeliveryPurchase(newpurchase, _currentCustomer));
            }
            else
            { 
                newpurchase.Delivery = false;

            

                var purchaseJson = JsonConvert.SerializeObject(newpurchase);
                var content = new StringContent(purchaseJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Store.client.PostAsync(Store.APP_PATH + "/api/purchases", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Заказ успешно добавлен");
                    LoadPurchases();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении заказа");

                }
            }

        }

        private void BtnContent_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            Manager.MainFrame.Navigate(new PurchaseContentPage((sender as Button).DataContext as Purchase));
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            int purchaseId = (int)btn.CommandParameter;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            if (MessageBox.Show($"Вы точно хотите удалить этот заказ?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
#pragma warning disable IDE0059 // Ненужное присваивание значения
                try
                {


                    HttpResponseMessage response = await Store.client.DeleteAsync(($"{Store.APP_PATH}/api/Purchases/{purchaseId}"));

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Заказ успешно удален.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Заказ не найден.");
                    }
                    LoadPurchases();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления!");
                }
#pragma warning restore IDE0059 // Ненужное присваивание значения

            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы хотите изменить условия доставки?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                Manager.MainFrame.Navigate(new DeliveryPurchase((sender as Button).DataContext as Purchase, _currentCustomer));
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
        }
        private async void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Number.Text != "")

            {
                HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/Purchases/" + _current_Customer_Id + "/" + Number.Text);

                if (response.IsSuccessStatusCode)
                {
                    var purchasesJson = await response.Content.ReadAsStringAsync();
                    var purchases = JsonConvert.DeserializeObject<List<Purchase>>(purchasesJson);

                    DgridStore.ItemsSource = purchases;

                }
                else
                {
                    MessageBox.Show("Ошибка сервера!");
                }
            }
            else
                LoadPurchases();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

