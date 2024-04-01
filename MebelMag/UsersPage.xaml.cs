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
            LoadUsers();
        }
        
        public async void LoadUsers()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/users");

            if (response.IsSuccessStatusCode)
            {
                var usersJson = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(usersJson);
                DgridStore.ItemsSource = users;
               
            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }
        

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        
                int userId = (int)btn.CommandParameter;
                 if (MessageBox.Show($"Вы точно хотите удалить этого пользователя?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    
                        HttpResponseMessage response = await Store.client.DeleteAsync(($"{Store.APP_PATH}/api/users/{userId}"));
                           
                        if (response.IsSuccessStatusCode)
                        {
                        MessageBox.Show("Пользователь успешно удален.");
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                        MessageBox.Show("Пользователь не найден.");
                        }
                    LoadUsers();
                     
                }
                catch (Exception ex)
                {
                    MessageBox.Show("");
                }
            }
        }



        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditUser(null));

        }

        private  async void Email_TextChanged(object sender, TextChangedEventArgs e)
        {

            HttpResponseMessage response = await Store.client.GetAsync($"{Store.APP_PATH}/api/users/email?email={Email.Text}") ;

            if (response.IsSuccessStatusCode)
            {
                var usersJson = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(usersJson);
                DgridStore.ItemsSource = users;

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }

    }
}
 