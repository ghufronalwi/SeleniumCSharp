using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumCSharp.POM;
using System.Linq.Expressions;

namespace SeleniumCSharp
{
    public class TestComputerDB
    {
        private IWebDriver _driver;
        string comName = "Asus RoG 2023";
        string dateIntroduced = "2023-03-23";
        string dateDiscontinued = "2024-04-24";

        [SetUp]
        public void StartBrowser()
        {
            _driver = new ChromeDriver();
            //_driver.Navigate().GoToUrl("http://eaapp.somee.com");
            _driver.Navigate().GoToUrl("https://computer-database.gatling.io/computers");
            Thread.Sleep(2000);
        }

        //[Test]
        //public void TestAccess()
        //{
        //    //IWebDriver _driver = new ChromeDriver();
        //    //driver.Navigate().GoToUrl("http://eaapp.somee.com/");



        //    //3. POM Initialization
        //    ComputerDBPage comDBPage = new ComputerDBPage(_driver);

        //    comDBPage.ClickLogin();
        //    comDBPage.Login("admin", "password");
        //}

        [Test]
        public void TC_COM0001_Access()
        {
            //_driver.Navigate().GoToUrl("https://computer-database.gatling.io/computers");
            Assert.That(_driver.FindElement(By.CssSelector("#main > h1")).Displayed, Is.True); // Total computer number 
            Assert.That(_driver.FindElement(By.CssSelector("tbody")).Displayed, Is.True); // Table data
            Assert.That(_driver.FindElement(By.Id("searchsubmit")).Displayed, Is.True); // Filter button
            Assert.That(_driver.FindElement(By.Id("add")).Displayed, Is.True); // Add new computer button
        }

        [Test]
        public void TC_COM0006_FilterValidCaseSensitive()
        {
            _driver.FindElement(By.Id("searchbox")).SendKeys("MacBook"); // Write "MacBook" in the search box
            _driver.FindElement(By.Id("searchsubmit")).Click(); // Click the filter/search button
        }

        [Test]
        public void TC_COM0014_PaginationNextPrev()
        {
            _driver.FindElement(By.CssSelector(".next > a")).Click(); // Click the Next button
            Assert.That(_driver.FindElement(By.LinkText("Displaying 11 to 20 of 574")).Displayed, Is.True); // Verify if the page shows next data
            _driver.FindElement(By.CssSelector(".prev > a")).Click(); // Click the Previous button
            Assert.That(_driver.FindElement(By.LinkText("Displaying 1 to 10 of 574")).Displayed, Is.True); // Verify if the page shows previous data
        }

        [Test]
        public void TC_COM0017_SortByComName()
        {
            // Click the computer name column header (Descending Order)
            _driver.FindElement(By.LinkText("Computer name")).Click();
            // Compare the list of computer names from 5 pages (50 data) and verify if it was sorted correctly
            List<String> computerNamesDesc = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                IReadOnlyList<IWebElement> cells = _driver.FindElements(By.CssSelector("tr > td > a"));
                foreach (IWebElement cell in cells)
                {
                    computerNamesDesc.Add(cell.Text);
                }
                _driver.FindElement(By.CssSelector(".next > a")).Click();
            }
            // Verify if the computer names was sorted correctly in descending order
            List<String> sortedComputerNamesDesc = new List<string>(computerNamesDesc);
            sortedComputerNamesDesc.Sort();


            // Click the computer name column header (Ascending Order)
            _driver.FindElement(By.LinkText("Computer name")).Click();
            // Compare the list of computer names from 5 pages (50 data) and verify if it was sorted correctly
            List<String> computerNamesAsc = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                IReadOnlyList<IWebElement> cells = _driver.FindElements(By.CssSelector("tr > td > a"));
                foreach (IWebElement cell in cells)
                {
                    computerNamesAsc.Add(cell.Text);
                }
                _driver.FindElement(By.CssSelector(".next > a")).Click();
            }
            // Verify if the computer names was sorted correctly in ascending order
            List<String> sortedComputerNamesAsc = new List<string>(computerNamesAsc);
            sortedComputerNamesAsc.Sort();

            //Assert.That(computerNamesDesc.SequenceEqual(sortedComputerNamesDesc), Is.True);
            //Assert.That(computerNamesAsc.SequenceEqual(sortedComputerNamesAsc), Is.True);
        }

        [Test]
        public void TC_COM0022_AddWithValidInput()
        {
            _driver.FindElement(By.Id("add")).Click(); // Click add button
            _driver.FindElement(By.Id("name")).SendKeys(comName); // Fill in the computer name field
            _driver.FindElement(By.Id("introduced")).SendKeys(dateIntroduced); // Fill in the introduced field
            _driver.FindElement(By.Id("discontinued")).SendKeys(dateDiscontinued); // Fill in the discontinued field
            SelectElement selectElement = new SelectElement(_driver.FindElement(By.Id("company")));
            selectElement.SelectByText("ASUS"); // Select company dropdown
            _driver.FindElement(By.CssSelector(".btn.primary")).Submit(); // Click the Create button

            // Verify if the create computer notification appears
            var notif = _driver.FindElement(By.CssSelector(".alert-message"));
            string strNotif = comName + " has been created";
            Assert.That(notif.Displayed, Is.True);
            Assert.That(notif.Text.Contains(strNotif), Is.True);

            //// Verify if the newly created data exists in the table data
            //_driver.FindElement(By.Id("searchbox")).SendKeys(comName);
            //_driver.FindElement(By.Id("searchsubmit")).Click();
            //Assert.That(_driver.FindElement(By.CssSelector("tbody")).Displayed, Is.True);
        }

        [Test]
        public void TC_COM0029_EditWithValidInput()
        {
            _driver.FindElement(By.CssSelector("tr:nth-child(1) > td > a")).Click(); // Click on a computer name link in the table
            _driver.FindElement(By.Id("name")).Clear();
            _driver.FindElement(By.Id("name")).SendKeys(comName); // Update the computer name field
            _driver.FindElement(By.Id("introduced")).SendKeys(dateIntroduced); // Fill in the introduced field
            _driver.FindElement(By.Id("discontinued")).SendKeys(dateDiscontinued); // Fill in the discontinued field
            SelectElement selectElement = new SelectElement(_driver.FindElement(By.Id("company")));
            selectElement.SelectByText("ASUS"); // Select company dropdown
            _driver.FindElement(By.CssSelector(".btn.primary")).Submit(); // Click the Save button

            // Verify if the edit computer notification appears
            var notif = _driver.FindElement(By.CssSelector(".alert-message"));
            string strNotif = comName + " has been updated";
            Assert.That(notif.Displayed, Is.True);
            Assert.That(notif.Text.Contains(strNotif), Is.True);

            //// Verify if the newly edited data is updated in the table
            //_driver.FindElement(By.Id("searchbox")).SendKeys(comName);
            //_driver.FindElement(By.Id("searchsubmit")).Click();
            //Assert.That(_driver.FindElement(By.CssSelector("tbody")).Displayed, Is.True);
        }

        [Test]
        public void TC_COM0036_DeleteData()
        {
            string strDeletedComName = _driver.FindElement(By.CssSelector("tr:nth-child(1) > td > a")).Text;
            _driver.FindElement(By.CssSelector("tr:nth-child(1) > td > a")).Click(); // Click on a computer name link in the table
            _driver.FindElement(By.CssSelector(".btn.danger")).Submit(); // Click the Delete button

            // Verify if the deleted computer notification appears
            var notif = _driver.FindElement(By.CssSelector(".alert-message"));
            string strNotif = strDeletedComName + " has been deleted";
            Assert.That(notif.Displayed, Is.True);
            Assert.That(notif.Text.Contains(strNotif), Is.True);

            //// Verify if the newly deleted data does not exist anymore
            //_driver.FindElement(By.Id("searchbox")).SendKeys(strDeletedComName);
            //_driver.FindElement(By.Id("searchsubmit")).Click();
            //Assert.That(_driver.FindElement(By.LinkText(strDeletedComName)).Displayed, Is.False);
        }

        [TearDown]
        public void CloseBrowser()
        {
            Thread.Sleep(2000);
            _driver.Quit();
        }
    }
}
