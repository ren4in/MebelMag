﻿using Newtonsoft.Json;
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
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public ProductListPage()
        {
            InitializeComponent();
            LoadProducts();
            LoadCategories();
        }
        private List<Product> currentproducts;
        private List<Product> allproducts;
    
        private void PageLoaded()
        {
            LoadProducts();
        }
        public async void LoadCategories()
        {

            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/ProductCategories");

            if (response.IsSuccessStatusCode)
            {
                var CategoryJson = await response.Content.ReadAsStringAsync();
                var currentcategories = JsonConvert.DeserializeObject<List<ProductCategory>>(CategoryJson);
                currentcategories.Insert(0, new ProductCategory
                {
                    CategoryName = "Все типы"
                }
                );
                ComboCategory.ItemsSource = currentcategories;
            }
        }
        
        public async void UpdateProducts()
        {
            currentproducts = allproducts;
            if (ComboCategory.SelectedIndex > 0)
            {
                currentproducts = currentproducts.Where(p => p.IdProductCategoryNavigation.CategoryName == ComboCategory.Text).ToList();
            }

            currentproducts = currentproducts.Where(p => p.ProductName.ToLower().Contains(Name.Text.ToLower())).ToList();
            if (MinPrice.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Price >= Convert.ToDecimal(MinPrice.Text)).ToList();

            }

            if (MaxPrice.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Price <= Convert.ToDecimal(MaxPrice.Text)).ToList();

            }
            if (WidthMin.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Width >= Convert.ToInt32(WidthMin.Text)).ToList();

            }

            if (WidthMax.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Width <= Convert.ToInt32(WidthMax.Text)).ToList();

            }

            if (LengthMin.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Length >= Convert.ToInt32(LengthMin.Text)).ToList();

            }

            if (LengthMax.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Length <= Convert.ToInt32(LengthMax.Text)).ToList();

            }

            if (HeightMin.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Height >= Convert.ToInt32(HeightMin.Text)).ToList();

            }

            if (HeightMax.Text != "")
            {
                currentproducts = currentproducts.Where(p => p.Height <= Convert.ToInt32(HeightMax.Text)).ToList();

            }
            DgridProducts.ItemsSource = currentproducts;

        }

         

        public async void LoadProducts()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/products/api/GetProducts");

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
        

        private void WidthMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts();
        }

        private void WidthMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void LehgthMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void HeightMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void HeightMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void MinPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void MaxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //   UpdateProducts();
        }

        private void ComboCategory_DropDownClosed(object sender, EventArgs e)
        {
            UpdateProducts();

        }

        private void LengthMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

