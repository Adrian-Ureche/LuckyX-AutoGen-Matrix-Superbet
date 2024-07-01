using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckyX__AutoGen
{
    class BrowserView
    {
        private IWebDriver driver;

        public BrowserView(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void pariaza(string cost, string urmatoarele)
        {
            try
            {
                IWebElement adauga = this.driver.FindElement(By.XPath("//div[@class='betting-buttons-fixed clearfix']//button[@class='control-button betting-button-add']"));

                IList<IWebElement> unordedBetting = this.driver.FindElements(By.XPath("//div[@class='betting-balls-wrap clearfix']//a[@class='betting-balls-ball']"));
                List<IWebElement> betting = new List<IWebElement>();
                for (int bi = 0; bi < 10; bi++)
                {
                    betting.Add(unordedBetting[bi]);
                    betting.Add(unordedBetting[bi + 10]);
                    betting.Add(unordedBetting[bi + 20]);
                    betting.Add(unordedBetting[bi + 30]);
                    betting.Add(unordedBetting[bi + 40]);
                }

                incarcaBilete(betting, adauga, cost, urmatoarele);
            }
            catch
            {
                MessageBox.Show("Nu se pot plasa biletele. Verificati daca ati apasat butonul Refresh si ca v-ati autentificat contul.");
            }
        }

        private void incarcaBilete(IList<IWebElement> betting, IWebElement adauga, string cost, string urmatoarele)
        {
            if (Directory.Exists("Fisiere"))
            {
                string[] files = Directory.GetFiles("Fisiere");

                foreach (string filePath in files)
                {
                    string line;
                    string[] bet;

                    System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                    while ((line = file.ReadLine()) != null)
                    {
                        bet = line.Split(',');
                        foreach (string b in bet)
                            if (b != "")
                                betting[Convert.ToInt32(b) - 1].Click();

                        adauga.Click();
                    }

                    file.Close();

                    plaseazaBilet(cost, urmatoarele);
                    System.Threading.Thread.Sleep(2800);
                }
            }
        }

        private void plaseazaBilet(string cost, string urm)
        {
            try
            {
                IWebElement urmatoarele = this.driver.FindElement(By.XPath("//div[@class='content-column-sidebar nospacing']//input[@seven-integer-parser='true']"));
                if (urm != "") while (urmatoarele.GetAttribute("value") != urm)
                {
                    // urmatoarele.Click();
                    urmatoarele.Clear();
                    System.Threading.Thread.Sleep(10);
                    urmatoarele.SendKeys(urm);
                }

                IWebElement miza = this.driver.FindElement(By.XPath("//div[@class='row clearfix']//input[@placeholder='Miză']"));
                if (cost != "") while (miza.GetAttribute("value") != cost)
                {
                    // miza.Click();
                    miza.Clear();
                    miza.SendKeys(cost);
                }
                // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.5);
                System.Threading.Thread.Sleep(1000);

                IWebElement pariaza = this.driver.FindElement(By.XPath("//div[@class='controls clearfix']//span[text()='Plătire']"));
                System.Threading.Thread.Sleep(200);
                pariaza.Click();
                System.Threading.Thread.Sleep(1000);
                pariaza.Click();
            }
            catch
            {
                MessageBox.Show("Nu se pot plasa biletele. Verificati daca ati apasat butonul Refresh si ca v-ati autentificat contul.");
            }
        }
    }
}
