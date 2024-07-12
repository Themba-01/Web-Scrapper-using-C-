using System;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;

namespace Webscrapping_
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up Selenium WebDriver (ChromeDriver in this example)
            using (IWebDriver driver = new ChromeDriver())
            {
                try
                {
                    // Navigate to the website
                    driver.Navigate().GoToUrl("https://betfred.co.za/sports/football/south-africa");

                    // Wait for the page to load and the JavaScript to execute
                    System.Threading.Thread.Sleep(5000); // Adjust the wait time as needed

                    // Get the page source after JavaScript execution
                    var pageSource = driver.PageSource;

                    // Load the page source into HtmlAgilityPack
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(pageSource);

                    // Use XPath to select all elements containing win odds
                    var winElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, '//*[@id=\"app\"]/div[1]/div[2]/div/div[2]/div/div/div[2]/div[5]/div/div[2]/div[1]/div[4]/div[2]/div[1]/a/div[3]/div/div/div[1]/div/span')]");

                    if (winElements != null)
                    {
                        foreach (var winElement in winElements)
                        {
                            // Extract and trim the text content
                            var winText = winElement.InnerText.Trim();

                            // Try to parse the winText as a float
                            if (float.TryParse(winText, NumberStyles.Any, CultureInfo.InvariantCulture, out float winOdd))
                            {
                                // Check if the win odd is greater than or equal to 1.6
                                if (winOdd >= 1.6f)
                                {
                                    Console.WriteLine($"Win Odd: {winOdd}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No win elements found.");
                    }
                }
                catch (WebDriverException e)
                {
                    Console.WriteLine($"WebDriver error: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e.Message}");
                }
                finally
                {
                    // Close the browser
                    driver.Quit();
                }
            }
        }
    }
}