using FunctionTestHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WXAnswers;
using WXAnswers.Models;

namespace WxAnswersUnitTests
{
    [TestClass]
    public class UnitTestFuncUser: FunctionTest
    {
        [TestMethod]
        public void TestUser()
        {
            var query = new Dictionary<string, StringValues>();
            var body = string.Empty;

            var result = WxAnswers.User(req: HttpRequestSetup(query, body), log: log);
            var resultObject = (OkObjectResult) result;
            
            Assert.IsNotNull(resultObject.Value);
            var userResponse = (UserResponse) resultObject.Value;
            Assert.IsInstanceOfType(userResponse, typeof(UserResponse));
            Assert.AreEqual("test", userResponse.Name);
            Assert.AreEqual("1234-455662-22233333-3333", userResponse.Token);
        }
    }
}
