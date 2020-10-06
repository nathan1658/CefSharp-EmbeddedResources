using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp_Example
{
    public class CustomSchemeHandlerFactory : ISchemeHandlerFactory
    {

        public const string SchemeName = "CustomScheme";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new CustomResourceHandler();
        }
    }
}
