using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WXAnswers.Models;
using WXAnswers.ProductAPIs;

namespace WXAnswers.DAL
{
    class ProductDataAccess
    {
        public static string GetTokenFromRequest(HttpRequest httpReq)
        {
            string requestBody = new StreamReader(httpReq.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string token = data?.token;
            return token;

        }
        public static IEnumerable<Product> GetProductsSorted(string sortOption)
        {
            IEnumerable<Product> products = Enumerable.Empty<Product>();

            IEnumerable<ShopperHistory> shopperHistory = Enumerable.Empty<ShopperHistory>();

            //get products
            if (sortOption == "Recommended")
            {
                shopperHistory = ProductAPIRequests.GetShopperHistory();
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
                products = ProductAPIRequests.GetProducts();

                //sort high
                switch (sortOption)
                {
                    case "High":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    case "Low":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "Descending":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    case "Ascending":
                        products = products.OrderBy(p => p.Price);
                        break;
                }
            }

            return products;
        }

        public static double GetTrolleyTotals(string jsonBody)
        {
            return ProductAPIRequests.GetTrolleyTotals(jsonBody);
        }
    }
}
