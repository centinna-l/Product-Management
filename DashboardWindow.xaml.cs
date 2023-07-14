using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;

namespace ProductManagement
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private const string ApiBaseUrl = "http://localhost:8000";
        private const string ProductEndpoint = "/api/product/admin";
        private static readonly string TokenFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token.txt");
        public DashboardWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            // Retrieve the token from the file
            string token = File.ReadAllText(TokenFilePath);

            using (HttpClient client = new HttpClient())
            {
                // Set the authorization header with the token
                client.DefaultRequestHeaders.Add("x-auth-token", token);

                try
                {
                    // Send the GET request to retrieve the products
                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}{ProductEndpoint}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        // Deserialize the response to get the list of products
                        var products = responseObject.data.ToObject<List<Product>>();

                        // Bind the products to the GridView
                        ProductsListView.ItemsSource = products;

                        
                       
                            // Log other product properties as needed
                        
                    }
                    else
                    {
                        // API request failed
                        MessageBox.Show("Failed to retrieve products.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Error occurred during the API request
                    MessageBox.Show("An error occurred while retrieving products: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }



        
    }

