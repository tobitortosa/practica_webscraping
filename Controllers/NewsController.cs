using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebScrapingApp.Models;

namespace WebScrapingApp.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            List<Noticia> noticias = GetNewsFromWebsite();
            return View(noticias);
        }

        public IActionResult NoticiasGuardadas()
        {
            return View();
        }
        private List<Noticia> GetNewsFromWebsite()
        {
            var noticias = new List<Noticia>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // ejecutar el navegador en segundo plano
            options.AddArgument("--disable-gpu"); // deshabilitar aceleración gráfica
            options.AddArgument("--window-size=1920x1080"); // definir tamaño de la ventana

            IWebDriver driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl("https://news.ycombinator.com/");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var elements = wait.Until(d => d.FindElements(By.CssSelector(".titleline > a")));

                foreach (var element in elements)
                {

                    Noticia noticia = new Noticia();

                    noticia.titulo= element.Text;
                    noticia.url = element.GetAttribute("href");
                    noticias.Add(noticia);
                }
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine($"Error de Selenium: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }

            return noticias;
        }
    }
}
