using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CefSharp_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //For CefSharp
            //Add Custom assembly resolver
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            InitializeCefSharp();
            InitializeComponent();

        }
        private void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // the browser control is initialized, now load the html

            //browser.LoadHtml("<html><head></head><body><h1>Hello, World!</h1></body></html>", "http://www.example.com/");
            browser.Load(CustomSchemeHandlerFactory.SchemeName+ "://def/");
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            try
            {
                if (!Cef.IsInitialized)
                {//CEF broser set path
                    var settings = new CefSettings();

                    settings.RegisterScheme(

                        new CefCustomScheme
                        {
                            SchemeName = CustomSchemeHandlerFactory.SchemeName,
                            SchemeHandlerFactory = new CustomSchemeHandlerFactory(
                            //rootFolder: AssemblyDirectory + "/Test1",
                            //schemeName: "abc",
                            //hostName: "def",
                            //defaultPage: "index2.html"
                        )
                        }
                      );

                    // Set BrowserSubProcessPath based on app bitness at runtime
                    settings.BrowserSubprocessPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                           Environment.Is64BitProcess ? "x64" : "x86",
                                                           "CefSharp.BrowserSubprocess.exe");

                    // Make sure you set performDependencyCheck false
                    Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
                }
            }
            catch (Exception ex)
            {
                //log.Error($"Error in setting CEF, mgs {ex.ToString()}");
                throw ex;
            }
        }
        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }

    }
}
