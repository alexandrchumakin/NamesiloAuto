using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NamesiloAuto
{
    public class DriverManager
    {
        private IWebDriver _chromeDriver;

        public DriverManager()
        {
            //chromeDriver = new CustomRemoteWebDriver(new Uri(url));
            //AutoChrome();
            InitDriver();            
        }

        public void Login(string mainUrl, string user, string pw)
        {
            _chromeDriver.Navigate().GoToUrl(mainUrl + "login.php");
            SetValueWithJS(user, _chromeDriver.FindElement(By.Name("username")));
            SetValueWithJS(pw, _chromeDriver.FindElement(By.Name("password")));
            _chromeDriver.FindElement(By.XPath("//input[@src='/images/button_submit.gif']")).Click();
        }

        public void FillInTheBid(string url, string value)
        {
            try
            {
                _chromeDriver.Navigate().GoToUrl(url);
                WaitForPageSourceLoading();
                IWebElement field = _chromeDriver.FindElement(By.Name("bid"));
                SetValueWithJS(value, field);
                IWebElement checkBox = _chromeDriver.FindElement(By.Id("terms"));
                checkBox.Click();
                //ClickWithJS(checkBox);
                IWebElement submitButton =
                    _chromeDriver.FindElement(By.XPath("//input[@src='/images/button_place_your_bid.gif']"));
                submitButton.Click();
                _chromeDriver.SwitchTo().Alert().Accept();
                //ClickWithJS(submitButton);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set bid '{1}' for url '{0}': {2}.", url, value, ex.Message);
            }
        }

        void InitDriver()
        {
            var options = new ChromeOptions();
            options.AddArguments("test-type");
            options.AddArguments("start-maximized");
            _chromeDriver = new ChromeDriver(Environment.CurrentDirectory, options);
            
            ////ChromeOptions options = new ChromeOptions();
            ////options.BinaryLocation = Environment.CurrentDirectory + "/chromedriver.exe";
            
            ////options.AddArgument("test-type");            
            //options.AddArgument("--no-experiments");
            //options.AddArgument("--disable-translate");
            //options.AddArgument("--disable-plugins");
            //options.AddArgument("--disable-extensions");
            //options.AddArgument("--no-default-browser-check");
            //options.AddArgument("--clear-token-service");
            //options.AddArgument("--disable-default-apps");
            //options.AddArgument("--enable-logging");
            
            //chromeDriver = new ChromeDriver(options);
            //chromeDriver = new RemoteWebDriver(new Uri(url),DesiredCapabilities.Chrome());
            //SwitchWindow();
            
        }


        void SetValueWithJS(string value, IWebElement elem)
        {
            ((IJavaScriptExecutor)_chromeDriver).ExecuteScript(string.Format("arguments[0].setAttribute('value','{0}')", value), elem);
        }

        void ClickWithJS(IWebElement elem)
        {
            ((IJavaScriptExecutor)_chromeDriver).ExecuteScript("arguments[0].click();)", elem);
        }

        void WaitForPageSourceLoading()
        {
            for (int i = 0; i < 20; i++)
            {
                string oldSource = _chromeDriver.PageSource;
                Thread.Sleep(100);
                string currentSource = _chromeDriver.PageSource;
                if (currentSource.Equals(oldSource))
                    return;
            }
        }

        /*    
        void SwitchWindow()
        {
            //String winTitle = NativeWin32.GetText(NativeWin32.GetForegroundWindow()).Substring(0, NativeWin32.GetText(NativeWin32.GetForegroundWindow()).LastIndexOf('-')).Trim();



            string currentWindow = chromeDriver.CurrentWindowHandle;
            List<string> availableWindows = chromeDriver.WindowHandles.ToList();
            if (availableWindows.Count > 0)
            {

                foreach (var window in availableWindows)
                {
                    try
                    {
                        chromeDriver.SwitchTo().Window(window);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        chromeDriver.SwitchTo().Window(currentWindow);
                    }
                }
            }
            else
            {
                Console.WriteLine("Cannot find opened Chrome instance");
            }
        }*/


        /*
        void AutoChrome()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }

                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);
                AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
                AutomationElement newTab = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "New Tab"));
                System.Windows.Point p = newTab.GetClickablePoint();
                //Mouse.Move((int)p.X, (int)p.Y);
                NativeMethods.ClickLeftMouse((int)p.X, (int)p.Y);
                //Mouse.Click(MouseButton.Left);


                // if it can be found, get the value from the URL bar
                if (elmUrlBar != null)
                {
                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        Console.WriteLine("Chrome URL found: " + val.Current.Value);
                    }
                }
                //
                if (newTab != null)
                {
                    AutomationPattern[] patterns = newTab.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)newTab.GetCurrentPattern(patterns[0]);                        
                        Console.WriteLine("Chrome URL found: " + val.Current.Value);
                    }
                }
            }
        }*/

    }
}
