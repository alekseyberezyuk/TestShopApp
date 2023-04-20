using System;
using System.Net;
using Newtonsoft.Json;
using TestShopAppAutomation.Extensions;
using TestShopApplication.Shared.Dtos;

namespace TestShopAppAutomation.API
{
    internal class AuthApiTests
    {
        private string Url => $"{ApiSettings.baseUrl}/{ApiSettings.auth}";
        private TestUserAccountCreds UserCreds => Configuration.Instance.testUserAccountCreds;
        private ApiSettings ApiSettings => Configuration.Instance.apiSettings;

        [SetUp]
        public void Setup()
        {
            Configuration.Load();
        }

        [Test]
        public async Task WithIncorrectCreds_Returns200_WithError()
        {
            LoginDataPresentation requestBody = new()
            {
                Username = "test@test.com",
                Password = "password"
            };

            HttpResponseMessage response = await new HttpClient().Post(Url, requestBody);
            string json = await response.Content.ReadAsStringAsync();
            AuthResponsePresentation responseObj = JsonConvert.DeserializeObject<AuthResponsePresentation>(json);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseObj.IsSuccess, Is.False);
            Assert.True(string.IsNullOrEmpty(responseObj.Token));
        }

        [Test]
        public async Task WithCorrectCreds_Returns200_WithToken()
        {
            LoginDataPresentation requestBody = new()
            {
                Username = UserCreds.username,
                Password = UserCreds.password
            };

            HttpResponseMessage response = await new HttpClient().Post(Url, requestBody);

            string json = await response.Content.ReadAsStringAsync();
            AuthResponsePresentation responseObj = JsonConvert.DeserializeObject<AuthResponsePresentation>(json);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseObj.IsSuccess, Is.True);
            Assert.True(!string.IsNullOrEmpty(responseObj.Token));
        }
    }
}
