using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WXAnswers.Models;

namespace WXAnswers.ProductAPIs
{
    class ProductAPIRequests
    {
        public static IEnumerable<Product> GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ProductsURL);
                string urlParameters = "?token=" + Constants.Token;
                var products = Enumerable.Empty<Product>();

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    products = response.Content.ReadAsAsync<IEnumerable<Product>>().Result;
                }

                return products;
            }
        }

        public static IEnumerable<ShopperHistory> GetShopperHistory()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ShopperHistoryURL);
                string urlParameters = "?token=" + Constants.Token;
                var shopperProducts = Enumerable.Empty<ShopperHistory>();

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    shopperProducts = response.Content.ReadAsAsync<IEnumerable<ShopperHistory>>().Result;
                }

                return shopperProducts;
            }


        }

        public static double GetTrolleyTotals(string jsonRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                //Uri requestUri = new Uri(Constants.TrolleyCalcURL);
                string requestUri = Constants.TrolleyCalcURL + "?token=" + Constants.Token;
                double total = 0;
                //client.BaseAddress = new Uri();
                //client.DefaultRequestHeaders
                //      .Accept
                //      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                StringContent content = new StringContent(jsonRequest,
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

                HttpResponseMessage response = client.PostAsync(requestUri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    string stringTotal = response.Content.ReadAsAsync<string>().Result;
                    total = Double.TryParse(stringTotal, out double value) ? value : total;
                }

                return total;
            }


        }
    }
}
