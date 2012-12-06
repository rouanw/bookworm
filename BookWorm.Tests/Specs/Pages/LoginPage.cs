﻿using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class LoginPage {
    private static IWebDriver driver;

        public LoginPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }
        
        public bool IsCurrentPage()
        {
            return driver.Title == "Log in - My ASP.NET MVC Application";
        }

    }

}
