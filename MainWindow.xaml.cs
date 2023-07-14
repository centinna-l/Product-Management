using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;

namespace ProductManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ApiBaseUrl = "http://localhost:8000";
        private const string TokenEndpoint = "/api/auth/login";
        private static readonly string TokenFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token.txt");
        public MainWindow()
        {
            InitializeComponent();
        }
        private async Task<string?> GetJwtTokenAsync(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                
                
               // Prepare the token request data
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(username), "username");
                content.Add(new StringContent(password), "password");

                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var response = await client.PostAsync($"{ApiBaseUrl}{TokenEndpoint}", content);
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the response to get the token
                    var tokenResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { token = "" });
                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.token))
                    {
                        return tokenResponse.token;
                    }
                    else
                    {
                        // Token request failed or response is in unexpected format
                        MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                }
                else
                {
                    // Token request failed
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var accessToken = await GetJwtTokenAsync(username, password);

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Authentication successful, proceed to the main window or other functionality
                // MessageBox.Show("Login successful!");
                SaveAuthToken(accessToken);

                //Close();
                this.Hide();
                // Open the dashboard window
                DashboardWindow dashboard = new DashboardWindow();
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void SaveAuthToken(string token)
        {
            File.WriteAllText(TokenFilePath, token);
        }

    }
}
