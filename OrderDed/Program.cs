using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using CommandLine;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace OrderDed
{
    internal static class Program
    {
        public class Options
        {
            [Option('u', "user", Required = true, HelpText = "Specify the user name.")]
            public string User { get; set; }

            [Option('p', "password", Required = true, HelpText = "Specify the user password.")]
            public string Password { get; set; }

            [Option('d', "dish", Required = true, HelpText = "Specify the dish.")]
            public string Dish { get; set; }
        }

        private static readonly By HideBtn = By.CssSelector(".time-limit-cont-button");

        private static void Main(string[] args)
        {
            Console.WriteLine("Dedushka ordering tool.");

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(
                    o =>
                    {
                        Console.WriteLine("Ordering the dishes with the following parameters:");
                        Console.WriteLine($"    Dish:  {o.Dish}");
                        Console.WriteLine($"    User:  {o.User}");
                        Console.WriteLine($"Password:  hidden :)");

                        using ChromeDriver driver = CreateDriver();
                        driver
                            .GoToDed()
                            .GoToLogin()
                            .Login(o.User, o.Password)
                            .GoToOrder()
                            .AddDish(o.Dish)
                            .GoToCart()
                            .CreateOrder();

                        Console.WriteLine("Done.");
                    });
        }

        private static ChromeDriver CreateDriver()
        {
            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("headless");

            var driver = new ChromeDriver(chromeOptions);
            driver.Manage()
                .Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage()
                .Window.Size = new Size(900, 600);
            return driver;
        }

        private static ChromeDriver GoToDed(this ChromeDriver driver)
        {
            driver.Navigate().GoToUrl("http://www.dedushka.delivery/");
            driver.HideTimeLimit();
            return driver;
        }

        private static ChromeDriver HideTimeLimit(this ChromeDriver driver)
        {
            try
            {
                driver.FindElement(By.ClassName("time-limit-cont-button")).Click();
            }
            catch (ElementNotInteractableException)
            {
                Console.WriteLine("Skipping hiding the timeout pop-up since is is already hidden");
            }

            return driver;
        }

        private static ChromeDriver GoToLogin(this ChromeDriver driver)
        {
            driver.FindElement(By.ClassName("lines")).Click();
            driver.FindElement(By.XPath("//*[@title='Вход']")).Click();
            return driver;
        }

        private static ChromeDriver Login(this ChromeDriver driver, string name, string pwd)
        {
            driver.FindElement(By.Name("username")).SendKeys(name);
            driver.FindElement(By.Name("password")).SendKeys(pwd);
            driver.FindElement(By.Name("login")).Click();
            return driver;
        }

        private static ChromeDriver GoToOrder(this ChromeDriver driver)
        {
            driver.FindElement(By.ClassName("lines")).Click();
            driver.FindElement(By.XPath("//*[@title='Сделать заказ']")).Click();
            return driver;
        }

        private static ChromeDriver AddDish(this ChromeDriver driver, string name)
        {
            driver.HideTimeLimit();

            ReadOnlyCollection<IWebElement> allDishes = driver.FindElements(By.ClassName("tmb-woocommerce"));
            IWebElement neededDish =
                allDishes.FirstOrDefault(d => d.FindElement(By.ClassName("t-entry-title")).Text == name);

            if (neededDish != null)
            {
                IWebElement addBtn = neededDish.FindElement(By.ClassName("add_to_cart_button"));
                new Actions(driver)
                    .MoveToElement(addBtn)
                    .Click(addBtn)
                    .Perform();
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            return driver;
        }

        private static ChromeDriver GoToCart(this ChromeDriver driver)
        {
            driver.FindElement(By.ClassName("fa-bag")).Click();
            return driver;
        }

        private static ChromeDriver CreateOrder(this ChromeDriver driver)
        {
            driver.HideTimeLimit();
            IWebElement confirmBtn = driver.FindElement(By.Name("woocommerce_checkout_place_order"));
/*
            new Actions(driver)
                .MoveToElement(confirmBtn)
                .Click(confirmBtn)
                .Perform();
*/
            return driver;
        }
    }
}