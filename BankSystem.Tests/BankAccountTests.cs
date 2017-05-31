using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System;

namespace BankSystem.Tests
{
    public class BankSystemTests
    {

        [Test]
        public void CreateAccountFirstScenario()
        {
            string accountNumber = "20001";
            string accountName = "Mikaella Stone";
            string balance = "200";
            string deposit = "200";
            string withdraw = "100";
            string transfer = "50";
            string totalBalance = "₱250.00";

            Assert.AreEqual(totalBalance, CreateBankAccountOne(accountNumber, accountName, balance, deposit, withdraw, transfer));
        }

        public string CreateBankAccountOne(string accountNumber, string accountName, string balance, string deposit, string withdraw, string transfer)
        {
            string _balanceHolder;
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Manage().Window.Maximize();

                driver.Navigate().GoToUrl("http://localhost:63698/");

                IWebElement _createAccountLink = driver.FindElement(By.XPath("/html/body/p/a"));
                _createAccountLink.Click();
                WaitForJSToLoad(driver);

                IWebElement _accountNumber = driver.FindElement(By.Id("AccountNumber"));
                _accountNumber.Clear();
                _accountNumber.SendKeys(accountNumber);

                IWebElement _accountName = driver.FindElement(By.Id("AccountName"));
                _accountName.Clear();
                _accountName.SendKeys(accountName);

                IWebElement _password = driver.FindElement(By.Id("Password"));
                _password.Clear();
                _password.SendKeys("password");

                IWebElement _balance = driver.FindElement(By.Id("Balance"));
                _balance.Clear();
                _balance.SendKeys(balance);

                IWebElement _createdDate = driver.FindElement(By.Id("CreatedDate"));
                _createdDate.Clear();
                _createdDate.SendKeys("2017-01-01");

                IWebElement _createButton = driver.FindElement(By.XPath("/html/body/form/div/div[6]/div/input"));
                _createButton.Click();
                WaitForJSToLoad(driver);

                ProcessTransaction(driver,"D", deposit);

                ProcessTransaction(driver, "W", withdraw);

                ProcessTransaction(driver, "F", transfer);

                IWebElement _balanceLabel = driver.FindElement(By.XPath("/html/body/table/tbody/tr[4]/td[4]"));

                _balanceHolder = _balanceLabel.Text;

                IWebElement _deleteLink = driver.FindElement(By.XPath("/html/body/table/tbody/tr[4]/td[6]/a[3]"));
                _deleteLink.Click();
                WaitForJSToLoad(driver);

                IWebElement _deleteButton = driver.FindElement(By.XPath("/html/body/div/form/div/input"));
                _deleteButton.Click();
                WaitForJSToLoad(driver);

                return _balanceHolder;
            }
        }

        public void ProcessTransaction(IWebDriver driver, string TransactionType, string amount)
        {
            IWebElement _detailsLink = driver.FindElement(By.XPath("/html/body/table/tbody/tr[4]/td[6]/a[2]"));
            _detailsLink.Click();
            WaitForJSToLoad(driver);

            IWebElement _transactionLink = driver.FindElement(By.XPath("/html/body/p/a[2]"));
            _transactionLink.Click();
            WaitForJSToLoad(driver);

            IWebElement _transactionType = driver.FindElement(By.Id("TransactionType_Id"));
            _transactionType.SendKeys(TransactionType);
            WaitForJSToLoad(driver);
            WaitForJSToLoad(driver);

            IWebElement _amount = driver.FindElement(By.Id("Amount"));
            _amount.Clear();
            _amount.SendKeys(amount);

            IWebElement _makeButton = driver.FindElement(By.Id("Make"));
            _makeButton.Click();
            WaitForJSToLoad(driver);
            
        }

        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public Boolean WaitForJSToLoad(IWebDriver driver, int timeout = 30)
        {
            Sleep(2000);

            //Additional code for handling webdriver and js

            return true;
        }


    }
}
