using FunctionTestHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WXAnswers;
using WXAnswers.Models;

namespace WxAnswersUnitTests
{
    [TestClass]
    public class UnitTestFuncSort : FunctionTest
    {
        [TestMethod]
        public void TestSort_OptionEmpty()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", string.Empty);
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (BadRequestObjectResult) result;

            Assert.IsInstanceOfType(resultObject, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void TestSort_OptionInvalid()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", "abcedfg");
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (NotFoundObjectResult)result;

            Assert.IsInstanceOfType(resultObject, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void TestSort_OptionHigh()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", "high");
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (OkObjectResult)result;
            var productsSortHigh = (List<Product>) resultObject.Value;

            Assert.IsNotNull(productsSortHigh);
            Assert.IsTrue(productsSortHigh.Count > 0);
            Assert.IsTrue(productsSortHigh[0].Price == 999999999999);
        }

        [TestMethod]
        public void TestSort_OptionLow()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", "LOW");
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (OkObjectResult)result;
            var productsSort = (List<Product>)resultObject.Value;

            Assert.IsNotNull(productsSort);
            Assert.IsTrue(productsSort.Count > 0);
            Assert.IsTrue(productsSort[0].Price == 5);
        }

        [TestMethod]
        public void TestSort_OptionAscending()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", "Ascending");
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (OkObjectResult)result;
            var productsSort = (List<Product>)resultObject.Value;

            Assert.IsNotNull(productsSort);
            Assert.IsTrue(productsSort.Count > 0);
            Assert.IsTrue(productsSort[0].Name == "Test Product A");
            Assert.IsTrue(productsSort[1].Name == "Test Product B");

        }

        [TestMethod]
        public void TestSort_OptionDescending()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", "Descending");
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (OkObjectResult)result;
            var productsSort = (List<Product>)resultObject.Value;

            Assert.IsNotNull(productsSort);
            Assert.IsTrue(productsSort.Count > 0);
            Assert.IsTrue(productsSort[0].Name == "Test Product F");
            Assert.IsTrue(productsSort[1].Name == "Test Product D");
        }

        [TestMethod]
        public void TestSort_OptionRecommended()
        {
            var query = new Dictionary<string, StringValues>();
            query.Add("sortOption", "Recommended");
            var body = string.Empty;

            var result = WxAnswers.Sort(req: HttpRequestSetup(query, body), log: log).Result;
            var resultObject = (OkObjectResult)result;
            var productsSort = (List<Product>)resultObject.Value;

            Assert.IsNotNull(productsSort);
            Assert.IsTrue(productsSort.Count > 0);
            Assert.IsTrue(productsSort[0].Quantity == 6);
        }
    }
}
