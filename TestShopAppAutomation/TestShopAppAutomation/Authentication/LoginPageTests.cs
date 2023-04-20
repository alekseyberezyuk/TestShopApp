using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestShopAppAutomation.Authentication
{
    public class LoginPageTests
    {
        private Configuration Config => Configuration.Instance;
        private string LoginPageUrl => $"{Config.baseUrl}/{Config.loginPageUrl}";

        [SetUp]
        public void Setup()
        {
            Configuration.Load();
        }

        [Test]
        public async Task WithIncorrectCreds_RemainsOnTheSamePage()
        {
            /// AAA - arrange, act, assert

            using ChromeDriver driver = new ChromeDriver();
            driver.Url = LoginPageUrl;
            IWebElement login = driver.FindElement(By.CssSelector("input[type=email]"));
            IWebElement password = driver.FindElement(By.CssSelector("input[type=password]"));
            
            login.Click();
            login.Clear();
            login.SendKeys("test@test.com");
            password.Click();
            password.Clear();
            password.SendKeys("test");
            IWebElement btn = driver.FindElement(By.TagName("button"));
            btn.Click();

            await Task.Delay(2000);

            Assert.That(driver.Url, Is.EqualTo(LoginPageUrl));
        }

        [Test]
        public async Task WithCorrectCreds_GoesToTheNextPage()
        {
            using ChromeDriver driver = new ChromeDriver();
            string mainPageFullUrl = $"{Config.baseUrl}/{Config.mainPageUrl}";
            driver.Url = LoginPageUrl;
            IWebElement login = driver.FindElement(By.CssSelector("input[type=email]"));
            IWebElement password = driver.FindElement(By.CssSelector("input[type=password]"));

            login.Click();
            login.Clear();
            login.SendKeys(Config.testUserAccountCreds.username);
            password.Click();
            password.Clear();
            password.SendKeys(Config.testUserAccountCreds.password);
            IWebElement btn = driver.FindElement(By.TagName("button"));
            btn.Click();

            await Task.Delay(2000);

            Assert.That(driver.Url, Is.EqualTo(mainPageFullUrl));
        }
    }
}