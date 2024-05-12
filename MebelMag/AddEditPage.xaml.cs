using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        Product _currentProduct =new Product();
        public AddEditPage(Product _selectedProduct)
        {
            InitializeComponent();
            if (_selectedProduct != null)
            {
                _currentProduct = _selectedProduct;
            }
            DataContext = _currentProduct;
            LoadCategories();
        }

        private byte[] ImageToByteArray(Image image)
        {
            BitmapSource bitmapSource = (BitmapSource)((ImageSource)image.Source);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

            public async void LoadCategories()
        {

            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/ProductCategories");

            if (response.IsSuccessStatusCode)
            {
                var CategoryJson = await response.Content.ReadAsStringAsync();
                var currentcategories = JsonConvert.DeserializeObject<List<ProductCategory>>(CategoryJson);
                Category.ItemsSource = currentcategories.ToList();
            
            if (_currentProduct!= null)
                {
                    ProductCategory currentCategory = currentcategories.FirstOrDefault(d => d.IdProductCategory == _currentProduct.IdProductCategory);
                    Category.SelectedItem = currentCategory;
                }
                Category.ItemsSource = currentcategories;
            }
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductName))
                errors.AppendLine("Укажите название товара!");
            if (string.IsNullOrWhiteSpace(_currentProduct.Description))
                errors.AppendLine("Укажите описание товара!");
            if (_currentProduct.Width == null)
                errors.AppendLine("Укажите ширину товара!");
            if (_currentProduct.Length == null)
                errors.AppendLine("Укажите длину товара!");
            if (_currentProduct.Height == null)
                errors.AppendLine("Укажите высоту товара!");
            if (_currentProduct.Price == 0)
                errors.AppendLine("Укажите цену товара!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;

            }
            _currentProduct.IdProductCategory = _currentProduct.IdProductCategoryNavigation.IdProductCategory;
            _currentProduct.IdProductCategoryNavigation = null;
            _currentProduct.Photo = ImageToByteArray(Photo);
            var productJson = JsonConvert.SerializeObject(_currentProduct);
     //       productJson = productJson.Replace(@",""IdProduct"": null", "");
            var content = new StringContent(productJson, Encoding.UTF8, "application/json");

            if (_currentProduct.IdProduct == null)
            {

                HttpResponseMessage response = await Store.client.PostAsync(Store.APP_PATH + "/api/products", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Товар успешно добавлен");
                    Manager.MainFrame.Navigate(new ProductPage());

                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении товара");

                }


            }
            else
            {
                HttpResponseMessage response = await Store.client.PutAsync(Store.APP_PATH + "/api/products/" + _currentProduct.IdProduct, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Данные успешно изменены!");
                    Manager.MainFrame.Navigate(new ProductPage());

                }
                else
                {
                    MessageBox.Show("Ошибка при изменении данных");
                }
            }

        }
            private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));
                Photo.Source = bitmap;
            }
        }

        }
}
