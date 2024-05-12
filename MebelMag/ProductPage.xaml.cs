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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            LoadProducts();

        }


        private List<Product> currentproducts;
        private List<Product> allproducts;

        public async void UpdateProducts()
        {
            currentproducts = allproducts;

            if (Name.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.ProductName.ToLower().Contains(Name.Text.ToLower())).ToList();

            }
            DgridStore.ItemsSource = currentproducts;

        }

        public async void LoadProducts()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/products/api/GetProducts");

            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                allproducts = JsonConvert.DeserializeObject<List<Product>>(productJson);
                DgridStore.ItemsSource = allproducts;
            }

            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Product));

        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));

        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();

        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int productId = (int)btn.CommandParameter;
            if (MessageBox.Show($"Вы точно хотите удалить эту позицию??", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {


                    HttpResponseMessage response = await Store.client.DeleteAsync(($"{Store.APP_PATH}/api/Products/{productId}"));


                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Товар успешно удален.");
                        LoadProducts();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Товар не найден.");
                    }



                }
                catch
                {
                    MessageBox.Show("Ошибка сервера!");
                }
            }

        }
    }
}

