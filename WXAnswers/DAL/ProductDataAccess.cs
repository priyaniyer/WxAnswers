using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXAnswers.Models;
using WXAnswers.ProductAPIs;

namespace WXAnswers.DAL
{
    class ProductDataAccess
    {
        public string GetTokenFromRequest(HttpRequest httpReq)
        {
            string requestBody = new StreamReader(httpReq.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string token = data?.token;
            return token;

        }
        public async Task<IEnumerable<Product>> GetProductsSortedAsync(string sortOption)
        {
            IEnumerable<Product> products = Enumerable.Empty<Product>();

            IEnumerable<ShopperHistory> shopperHistory = Enumerable.Empty<ShopperHistory>();

            ProductAPIRequests productAPIRequests = new ProductAPIRequests();

            //get products
            if (sortOption == "recommended")
            {
                shopperHistory = await productAPIRequests.GetShopperHistoryAsync();
                products = shopperHistory.SelectMany(sh => sh.Products)
                                    .GroupBy(p => p.Name)
                                    .Select(g => new Product
                                    {
                                        Name = g.First().Name,
                                        Price = g.First().Price,
                                        Quantity = g.Sum(p => p.Quantity)
                                    }).OrderByDescending(p => p.Quantity);
            }
            else
            {
                products = await productAPIRequests.GetProductsAsync();

                //sort high
                switch (sortOption)
                {
                    case "high":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    case "low":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "descending":
                        products = products.OrderByDescending(p => p.Name);
                        break;
                    case "ascending":
                        products = products.OrderBy(p => p.Name);
                        break;
                    default:
                        products = null;
                        break;
                }
            }

            return products;
        }

        public async Task<double> GetTrolleyTotalsAsync(string jsonBody)
        {
            ProductAPIRequests productAPIRequests = new ProductAPIRequests();
            return await Task.Run(() => 
            {
                return productAPIRequests.GetTrolleyTotals(jsonBody);
            });
        }
    }
}
