using CefSharp;
using CefSharp.Callback;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp_Example
{
    public class CustomResourceHandler : ResourceHandler
    {

        private  const string PREFIX= "CefSharp_Example.Test1.";
        public CustomResourceHandler(string initial = "index2.html")
        {
            StatusCode = 200;
            StatusText = "OK";
            Stream = readFromStream(initial);
        }


        private Stream readFromStream(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream result = assembly.GetManifestResourceStream(string.Concat(PREFIX, fileName));
            return result;
        }

        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;
            fileName = fileName.Replace('/', '.');
            if (fileName !=".")
            {

                if (fileName[0] == '.')
                    fileName = fileName.Substring(1);
                Stream = readFromStream(fileName);
                var tmp = readFromStream(fileName);
                if (tmp != null)
                    Stream = tmp;
            }
            return CefReturnValue.Continue;
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            throw new NotImplementedException();
        }

        public bool Open(IRequest request, out bool handleRequest, ICallback callback)
        {
            throw new NotImplementedException();
        }

        public bool ProcessRequest(IRequest request, ICallback callback)
        {
            throw new NotImplementedException();
        }

        public bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
        {
            throw new NotImplementedException();
        }

        public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            throw new NotImplementedException();
        }

        public bool Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}
