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
    /// Логика взаимодействия для PurchaseContentPage.xaml
    /// </summary>
    public partial class PurchaseContentPage : Page
    {
        public PurchaseContentPage(Purchase _selectedPurcchase)


        {
            InitializeComponent();
            _currentPurchase= _selectedPurcchase;
            LoadProducts();
            LoadContent();

        }
#pragma warning disable IDE0044 // Добавить модификатор только для чтения
        Purchase _currentPurchase;
#pragma warning restore IDE0044 // Добавить модификатор только для чтения
#pragma warning disable IDE0044 // Добавить модификатор только для чтения
#pragma warning disable IDE0051 // Удалите неиспользуемые закрытые члены
        private List<Product> currentproducts;
#pragma warning restore IDE0051 // Удалите неиспользуемые закрытые члены
#pragma warning restore IDE0044 // Добавить модификатор только для чтения
        private List<Product> allproducts;




        public async void LoadContent()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/m2mProductPurchase/id?id=" + _currentPurchase.IdPurchase);

            if (response.IsSuccessStatusCode)
            {
                var contentJson = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<List<M2mProductPurchase>>(contentJson);

                DgridStore.ItemsSource = content;

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        
        }
        public async void LoadProducts()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/products/api/GetProductsNoPhoto");

            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                allproducts = JsonConvert.DeserializeObject<List<Product>>(productJson);
                DgridProducts.ItemsSource = allproducts;
            }

            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Amount.Text, out int n) == true)
            {
                Button btn = sender as Button;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                int productId = (int)btn.CommandParameter;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
                M2mProductPurchase _newPosition = new M2mProductPurchase();
                _newPosition.IdProduct= productId;
                _newPosition.IdPurchase = (int)_currentPurchase.IdPurchase;
                _newPosition.Amount = Convert.ToInt32(Amount.Text);

                var positionJson = JsonConvert.SerializeObject(_newPosition);
                var content = new StringContent(positionJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Store.client.PostAsync(Store.APP_PATH + "/api/M2mProductPurchase", content);
                if (response.IsSuccessStatusCode)
                {

                    LoadContent();
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении товара");

                }


            }
            else
            {
                MessageBox.Show("Введите количество!");
            }
            }

            private  async void Edit_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable IDE0059 // Ненужное присваивание значения
            if (int.TryParse(Amount.Text, out int n) == true)
            {
                Button btn = sender as Button;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                int productId = (int)btn.CommandParameter;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

                var purchaseJson = JsonConvert.SerializeObject(_currentPurchase);
                var content = new StringContent(purchaseJson);
                HttpResponseMessage response = await Store.client.PutAsync(Store.APP_PATH + "/api/m2mProductPurchase?idProduct=" + productId + "&idPurchase=" + _currentPurchase.IdPurchase + "&amount=" + Convert.ToInt32(Amount.Text), content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Данные успешно изменены!");
                    LoadContent();
                    LoadProducts();

                }
                else
                {
                    MessageBox.Show("Ошибка при изменении данных");
                }
            }
            else
                MessageBox.Show("Введите количество!");
#pragma warning restore IDE0059 // Ненужное присваивание значения

        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            int productId = (int)btn.CommandParameter;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            if (MessageBox.Show($"Вы точно хотите удалить эту позицию??", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
#pragma warning disable IDE0059 // Ненужное присваивание значения
                try
                {


                    HttpResponseMessage response = await Store.client.DeleteAsync(($"{Store.APP_PATH}/api/M2mProductPurchase?idProduct=" + productId + "&idPurchase=" + _currentPurchase.IdPurchase));

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Позиция успешно удалена.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Позиция не найдена.");
                    }
                   LoadContent();
                    LoadProducts();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("");
                }
#pragma warning restore IDE0059 // Ненужное присваивание значения
            }
        }

        private async void Print_Click(object sender, RoutedEventArgs e)
        {

            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/m2mProductPurchase/id?id=" + _currentPurchase.IdPurchase);

            if (response.IsSuccessStatusCode)
            {
                var contentJson = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<List<M2mProductPurchase>>(contentJson);
                string invoice = "Накладная по заказу №" + _currentPurchase.IdPurchase;
                invoice += "\n" + "| Код товара | Название | Количество | Цена |";
                decimal currentprice;
                decimal fullprice = 0;
                for (int i = 0; i < content.Count; i++)
                {
                    currentprice = (decimal)(content[i].Amount * content[i].IdProductNavigation.Price);
                    invoice += "\n" + "| " + content[i].IdProduct + " | " + content[i].IdProductNavigation.ProductName + " | " + content[i].Amount + " | " + currentprice + " руб. |";
                    fullprice += currentprice;
                }
                invoice += "\n" + "Итоговая цена: " + fullprice + " руб.";
                PrintDialog printDialog = new PrintDialog();
                TextBlock visual = new TextBlock();
                visual.Text = invoice;
                visual.FontSize = 20;
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(visual, "Накладная");
                }
                DgridStore.ItemsSource = content;

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }

        }

        private void Amount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
