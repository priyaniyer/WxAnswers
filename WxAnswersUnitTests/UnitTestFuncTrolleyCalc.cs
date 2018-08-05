using FunctionTestHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WXAnswers;

namespace WxAnswersUnitTests
{
    [TestClass]
    public class UnitTestFuncTrolleyCalc : FunctionTest
    {
        [TestMethod]
        public void TestTrolleyTotal()
        {
            var query = new Dictionary<string, StringValues>();
            var body = LoadJson("Mockdata\\trolleyTotal.json");

            var result = WxAnswers.TrolleyCalculator(req: HttpRequestSetup(query, body), log: log).Result;
            Assert.IsNotInstanceOfType(result, typeof(BadRequestResult));

            var resultObject = (OkObjectResult) result;
            Assert.AreEqual(resultObject.Value, 100.0);


        }

        private string LoadJson(string filename)
        {
            using (StreamReader r = new StreamReader(filename))
            {
                return r.ReadToEnd();
            }
        }
    }
}
