using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCSharp
{
    public class TestJSAlerts
    {
        private IWebDriver _driver;

        [SetUp]
        public void StartBrowser()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            Thread.Sleep(2000);
        }

        [Test]
        public void TC_JS0001_Access()
        {
            Assert.That(_driver.FindElement(By.CssSelector("li:nth-child(1) > button")).Displayed, Is.True);
            Assert.That(_driver.FindElement(By.CssSelector("li:nth-child(2) > button")).Displayed, Is.True);
            Assert.That(_driver.FindElement(By.CssSelector("li:nth-child(3) > button")).Displayed, Is.True);
        }

        [Test]
        public void TC_JS0002_JSAlert()
        {
            _driver.FindElement(By.CssSelector("li:nth-child(1) > button")).Click();
            Assert.That(_driver.SwitchTo().Alert().Text, Is.EqualTo("I am a JS Alert"));
            _driver.SwitchTo().Alert().Accept();
            Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You successfully clicked an alert"));
        }


        [Test]
        public void TC_JS0003_JSConfirmOk()
        {
            _driver.FindElement(By.CssSelector("li:nth-child(2) > button")).Click();
            Assert.That(_driver.SwitchTo().Alert().Text, Is.EqualTo("I am a JS Confirm"));
            _driver.SwitchTo().Alert().Accept();
            Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You clicked: Ok"));
        }

        [Test]
        public void TC_JS0004_JSConfirmCancel()
        {
            _driver.FindElement(By.CssSelector("li:nth-child(2) > button")).Click();
            Assert.That(_driver.SwitchTo().Alert().Text, Is.EqualTo("I am a JS Confirm"));
            _driver.SwitchTo().Alert().Dismiss();
            Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You clicked: Cancel"));
        }

        [Test]
        public void TC_JS0005_JSPromptOk()
        {
            string str = "Lorem ipsum";
            _driver.FindElement(By.CssSelector("li:nth-child(3) > button")).Click();
            Assert.That(_driver.SwitchTo().Alert().Text, Is.EqualTo("I am a JS prompt"));
            _driver.SwitchTo().Alert().SendKeys(str);
            _driver.SwitchTo().Alert().Accept();
            Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You entered: " + str));
        }

        [Test]
        public void TC_JS0006_JSPromptOkEmpty()
        {
            _driver.FindElement(By.CssSelector("li:nth-child(3) > button")).Click();
            Assert.That(_driver.SwitchTo().Alert().Text, Is.EqualTo("I am a JS prompt"));
            _driver.SwitchTo().Alert().Accept();
            Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You entered:"));
        }

        [Test]
        public void TC_JS0007_JSPromptCancel()
        {
            _driver.FindElement(By.CssSelector("li:nth-child(3) > button")).Click();
            Assert.That(_driver.SwitchTo().Alert().Text, Is.EqualTo("I am a JS prompt"));
            _driver.SwitchTo().Alert().Dismiss();
            Assert.That(_driver.FindElement(By.Id("result")).Text, Is.EqualTo("You entered: null"));
        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Quit();
        }
    }
}
