
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using WXAnswers.Models;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXAnswers.ProductAPIs;
using WXAnswers.DAL;
using System.Threading.Tasks;

namespace WXAnswers
{
    public static class WxAnswers
    {   

        [FunctionName("user")]
        public static IActionResult User([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wxAnswers/user")]HttpRequest req, TraceWriter log)
        {
            log.Info("User Request");            

            return 
                (ActionResult) new OkObjectResult(new UserResponse()
                {
                    Name = "test",
                    Token = "1234-455662-22233333-3333"
                });
        }

        [FunctionName("sort")]
        public static async Task<IActionResult> Sort([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wxAnswers/sort")]HttpRequest req, TraceWriter log)
        {
            log.Info("sort Request");

            string sortOption = HttpUtility.UrlDecode(req.Query["sortOption"]);
            ProductDataAccess productDataAccess = new ProductDataAccess();

            if (String.IsNullOrEmpty(sortOption))
            {
                return new BadRequestObjectResult("Invalid sort option.");
            }

            var products = await productDataAccess.GetProductsSortedAsync(sortOption.ToLower());

            if(products == null || products.Count() == 0)
            {
                return new NotFoundObjectResult("No products found to sort");
            }

            return new OkObjectResult(products.ToList());
        }

        [FunctionName("trolleyCalculator")]
        public static async Task<IActionResult> TrolleyCalculator([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wxAnswers/trolleyCalculator")]HttpRequest req, TraceWriter log)
        {
            log.Info("trolleyCalculator Request");

            string requestBody = req.Body.ToString();
            requestBody = new StreamReader(req.Body).ReadToEnd();
            ProductDataAccess productDataAccess = new ProductDataAccess();

            var totals = await productDataAccess.GetTrolleyTotalsAsync(requestBody);

            if(totals == 0)
            {
                return new BadRequestResult();
            }
            
            return new OkObjectResult(totals);
        }
        
    }
}
