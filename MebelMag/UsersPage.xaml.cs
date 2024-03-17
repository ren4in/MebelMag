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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Store.APP_PATH+"/api/users");

                if (response.IsSuccessStatusCode)
                {
                    var usersJson = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<User>>(usersJson);
                    DgridStore.ItemsSource = users;
                    foreach (var user in users)
                    {
                        MessageBox.Show($"User ID: {user.IdUser}, Name: {user.FirstName}, Role: {user.IdDepartmentNavigation.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Unable to retrieve users.");
                }
            }
        }
    


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
