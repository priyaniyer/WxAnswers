using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WXAnswers.Models;

namespace WXAnswers.ProductAPIs
{
    class ProductAPIRequests
    {
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ProductsURL);
                string urlParameters = "?token=" + Constants.Token;
                var products = Enumerable.Empty<Product>();

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = await client.GetAsync(urlParameters);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                }

                return products;
            }
        }

        public async Task<IEnumerable<ShopperHistory>> GetShopperHistoryAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ShopperHistoryURL);
                string urlParameters = "?token=" + Constants.Token;
                var shopperProducts = Enumerable.Empty<ShopperHistory>();

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = await client.GetAsync(urlParameters);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    shopperProducts = await response.Content.ReadAsAsync<IEnumerable<ShopperHistory>>();
                }

                return shopperProducts;
            }


        }

        public double GetTrolleyTotals(string jsonBody)
        {
            Order order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(jsonBody);
            double prodTotals = 0.0, specialTotal = 0.0, lowestTotal = 0.0;
            if (order != null)
            {
                IEnumerable<Product> products = order.Products;
                IEnumerable<Special> specials = order.Specials;
                IEnumerable<Quantity> quantities = order.Quantities;
                if (products.Count() == 0 || specials.Count() == 0 || quantities.Count() == 0)
                {
                    Console.WriteLine("BadRequest");
                    return lowestTotal;
                }

                prodTotals = products.Sum(p => p.Price);
                if(prodTotals <= 0.0)
                {
                    Console.WriteLine("BadRequest");
                    return lowestTotal;
                }

                specialTotal = specials.Min(s => s.Total);
                var totalQty = quantities.First().quantity;
                lowestTotal = (specialTotal == 0.0 || prodTotals == 0.0)
                                    ? Math.Max(prodTotals, specialTotal) * totalQty
                                    : Math.Min(prodTotals, specialTotal) * totalQty;
                Console.WriteLine($"LowestTotal: {lowestTotal}");
            }           

            return lowestTotal;
        }

        public async Task<double> GetTrolleyTotalsAsyncNew(string jsonBody)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUri = Constants.TrolleyCalcURL + "?token=" + Constants.Token;
                double total = 0;

                StringContent content = new StringContent(jsonBody,
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

                HttpResponseMessage response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    string stringTotal = await response.Content.ReadAsAsync<string>();
                    total = Double.TryParse(stringTotal, out double value) ? value : total;
                }

                return total;
            }


        }
    }
}
