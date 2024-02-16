using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace MebelMag
{
    public class Store
    {
        public const string APP_PATH = "http://localhost:62928";
        public static readonly HttpClient client = new HttpClient();
         public static async Task<Uri> CreateRoleAsync(Role role)
        {
            //     MessageBox.Show(role.Role_Name + " " + role.id_Role);
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Roles", role);

            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }
        public static void SetBaseAddress(string address)
        {
            client.BaseAddress = new Uri(address);
        }

        public static async Task<Role> GetRoleAsync(string path)
        {
            Role role = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                role = await response.Content.ReadAsAsync<Role>();
            }
            return role;
        }









        public static async Task<User> UserAuthAsync(string email, string password)
        {
 
            HttpResponseMessage response = await client.GetAsync($"api/users/auth?email={email}&password={password}");
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();
                return user;
            }

             
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("Unauthorized", null, response.StatusCode);
            }
            else
                return null;

        }

    }
}
