using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
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
using Microsoft.Extensions.Primitives;
using System.Net;

namespace MebelMag
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }


        static async Task Authorization(string email, string password)
        {

            // Update port # in the following line.
            Store.client.BaseAddress = new Uri(Store.APP_PATH);
            Store.client.DefaultRequestHeaders.Accept.Clear();
            Store.client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
 
            /*    HttpResponseMessage response = await Store.client.GetAsync("api/Roles");
                 if (response.IsSuccessStatusCode)
                 {
                     var roles = await response.Content.ReadAsAsync<IEnumerable<Role>>();
                     foreach (var role in roles)
                     {
                         MessageBox.Show($"{role.IdRole} - {role.RoleName}");
                     }

                 }*/
            //User user = Store.UserAuthAsync(email, password);
            //MessageBox.Show(user.FirstName);

        }

        private async void AuthHandler()
        {
            try
            {
                if (string.IsNullOrEmpty(LoginBox.Text) || string.IsNullOrEmpty(PasswordB.Password))
                    {
                    throw new Exception("Введите логин и пароль!");
                }
               
           await  Store.UserAuthAsync(LoginBox.Text, PasswordB.Password);
 
                switch (Store.role)// определение роли при авторизации. Временно просто выводит название роли, потом будет направлять на соответсвующую страницу
                {
                    case ("Администратор"):
                        Manager.MainFrame.Navigate(new UsersPage());
                        break;
                    case ("Продавец"):
                        Manager.MainFrame.Navigate(new SellerPage());
                        break;
                    case ("Кладовщик"):
                        MessageBox.Show("Кладовщик");
                        break;
                    case ("Приемщик поставок"):
                        MessageBox.Show("Приемщик поставок");
                        break;
                    case ("Кассир"):
                        MessageBox.Show("Кассир");
                        break;
                }
             //   return task;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Неправильный логин или пароль!");
             //   return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            //    return null;
            }
        }
            private async void AuthButton_Click(object sender, RoutedEventArgs e)
        {

            AuthHandler();
        }

       
        }
    }
   
