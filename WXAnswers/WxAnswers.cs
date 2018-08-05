
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

namespace WXAnswers
{
    public static class WxAnswers
    {   

        [FunctionName("user")]
        public static IActionResult User([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wxAnswers/user")]HttpRequest req, TraceWriter log)
        {
            log.Info("User Request");

            var token = ProductDataAccess.GetTokenFromRequest(req);

            return String.IsNullOrEmpty(token)
                ? new BadRequestObjectResult("Invalid token")
                : (ActionResult) new OkObjectResult(new UserResponse()
                {
                    Name = "test",
                    Token = token
                });
        }

        [FunctionName("sort")]
        public static IActionResult Sort([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wxAnswers/sort")]HttpRequest req, TraceWriter log)
        {
            log.Info("sort Request");

            string sortOption = HttpUtility.UrlDecode(req.Query["sortOption"]);            

            if (String.IsNullOrEmpty(sortOption))
            {
                return new BadRequestObjectResult("Invalid sort option.");
            }

            var products = ProductDataAccess.GetProductsSorted(sortOption);

            if(products.Count() == 0)
            {
                return new NotFoundObjectResult("No products found to sort");
            }

            return new OkObjectResult(products.ToList());
        }

        [FunctionName("trolleyCalculator")]
        public static IActionResult TrolleyCalculator([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wxAnswers/trolleyCalculator")]HttpRequest req, TraceWriter log)
        {
            log.Info("trolleyCalculator Request");

            string requestBody = req.Body.ToString();
            requestBody = new StreamReader(req.Body).ReadToEnd();

            var totals = ProductDataAccess.GetTrolleyTotals(requestBody);

            if(totals == 0)
            {
                return new BadRequestResult();
            }
            
            return new OkObjectResult(totals);
        }
        
    }
}
