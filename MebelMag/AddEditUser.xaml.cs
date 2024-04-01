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
    /// Логика взаимодействия для AddEditUser.xaml
    /// </summary>
    public partial class AddEditUser : Page
    {
        //private User _currentuser = new User();

        public AddEditUser(User _selecteduser)
        {
            if (_selecteduser != null)
                _currentuser = _selecteduser;
            DataContext = _currentuser;
            LoadRoles();
            LoadDepartments();
            LoadPositions();
            InitializeComponent();
        }

        public async void LoadPositions()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/positions");

            if (response.IsSuccessStatusCode)
            {
                var positionsJson = await response.Content.ReadAsStringAsync();
                var positions = JsonConvert.DeserializeObject<List<Position>>(positionsJson);
                ComboPosition.ItemsSource = positions.ToList();

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }
        public async void LoadRoles()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/roles");

            if (response.IsSuccessStatusCode)
            {
                var rolesJson = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<Role>>(rolesJson);
                ComboRole.ItemsSource = roles.ToList();

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }

        public async void LoadDepartments()
        {
            HttpResponseMessage response = await Store.client.GetAsync(Store.APP_PATH + "/api/departments");

            if (response.IsSuccessStatusCode)
            {
                var departmentsJson = await response.Content.ReadAsStringAsync();
                var departments = JsonConvert.DeserializeObject<List<Department>>(departmentsJson);
                ComboDepartment.ItemsSource = departments.ToList();

            }
            else
            {
                MessageBox.Show("Ошибка сервера!");
            }
        }
        StringBuilder errors = new StringBuilder();

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            errors = new StringBuilder();
             if (string.IsNullOrWhiteSpace(_currentuser.LastName))
                errors.AppendLine("Укажите фамилию пользователя!");
            if (string.IsNullOrWhiteSpace(_currentuser.FirstName))
                errors.AppendLine("Укажите имя пользователя!");
            if (string.IsNullOrWhiteSpace(_currentuser.MiddleName))
                errors.AppendLine("Укажите отчество пользователя!");
            if (string.IsNullOrWhiteSpace(_currentuser.Login))
                errors.AppendLine("Укажите логин пользователя!");
            if (string.IsNullOrWhiteSpace(_currentuser.PhoneNumber))
                errors.AppendLine("Укажите номер телефона!");
            if (string.IsNullOrWhiteSpace(_currentuser.Password))
            {
                errors.AppendLine("Укажите пароль!");
            }

            if (ComboPosition.SelectedIndex < 0)
                errors.AppendLine("Укажите должность!");
            if (ComboDepartment.SelectedIndex < 0)
                errors.AppendLine("Укажите отдел!");
            if (ComboRole.SelectedIndex < 0)
                errors.AppendLine("Укажите роль!");
            if (_currentuser.Salary == null)
                errors.AppendLine("Укажите зарплату !");


            if (Check.CheckEmail(_currentuser.Login) == false)
                errors.AppendLine("Адрес электронной почты не соответствует формату!");
            if (Check.CheckPhone(_currentuser.PhoneNumber) == false)
                errors.AppendLine("Номер телефона не соответствует формату!");

            if (Check.CheckPassword(_currentuser.Password) == false)
                errors.AppendLine("Пароль должен состоять минимум из 8 латинских букв и цифр, из них как минимум одна прописная и одна строчная буква и одна цифра. Максильная длина пароля - 15 символов. ");


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание");

                return;

            }
            else
            {

                if (Sex.Text == "Мужской")
                    _currentuser.Sex = "м";
                else
                    _currentuser.Sex = "ж";
                if (_currentuser.IdUser == null)
                {
                     if (string.IsNullOrWhiteSpace(_currentuser.FirstName))
                        errors.AppendLine("Укажите фамилию пользователя!");
                    _currentuser.IdPosition = _currentuser.IdPositionNavigation.IdPosition;
                    _currentuser.IdPositionNavigation = null;
                    _currentuser.IdRole = _currentuser.IdRoleNavigation.IdRole;
                    _currentuser.IdRoleNavigation = null;
                    _currentuser.IdDepartment = _currentuser.IdDepartmentNavigation.IdDepartment;
                    _currentuser.IdDepartmentNavigation = null;
                    var userJson = JsonConvert.SerializeObject(_currentuser);
                    var content = new StringContent(userJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await Store.client.PostAsync(Store.APP_PATH + "/api/users", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Пользователь успешно добавлен");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении пользователя");
                    }

                }
            }
        }
    }

}