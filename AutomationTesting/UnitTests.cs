using OpenQA.Selenium.Chrome;

namespace AutomationTesting
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StartupTest()
        {
            var driver = new ChromeDriver();

            driver.Navigate().GoToUrl("http://localhost:5052/");

            var title = driver.Title;

            Assert.Equals(title, "Koduleht - Registreerimissüsteem");
        }
    }
}
