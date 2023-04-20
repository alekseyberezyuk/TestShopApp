using System;
using System.Net;

namespace TestShopAppAutomation.API
{
    internal class BaseTests
    {
        [SetUp]
        public void Setup()
        {
            Configuration.Load();
        }

        [Test]
        public async Task HealthCheck()
        {
            string url = Configuration.Instance.apiSettings.baseUrl + "/healthcheck";

            HttpResponseMessage response = await new HttpClient().GetAsync(url);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
    }
}
