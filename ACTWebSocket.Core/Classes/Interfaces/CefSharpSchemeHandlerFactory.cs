using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTWebSocket.Core.Classes.Interfaces
{
    public class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "rsrc";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (schemeName == SchemeName)
            {
                String extension = Path.GetExtension(request.Url.ToString());
                String mimeType = MimeTypes.MimeTypeMap.GetMimeType(extension);
                Uri Uri = new Uri(request.Url);

                var name = typeof(ACTWebSocket_Plugin.ACTWebSocketCore).Module.Name;
                var assem = typeof(ACTWebSocket_Plugin.ACTWebSocketCore).Module.Assembly;
                string[] resources = assem.GetManifestResourceNames();
                string list = "";

                // Build the string of resources.
                foreach (string resource in resources)
                    list += resource + "\r\n";
                // example : ACTWebSocket.Core.Resources.img.copy.svg

                String path = Uri.AbsolutePath.ToString();
                path = Path.GetDirectoryName(path).Substring(1) + "/" + Path.GetFileNameWithoutExtension(path);
                path = path.Replace("/", ".").Replace("\\", ".");
                path = Path.GetFileNameWithoutExtension(name) + "." + path;
                path = path + extension;
                Stream stream = typeof(ACTWebSocket_Plugin.ACTWebSocketCore).Module.Assembly.GetManifestResourceStream(path);
                return ResourceHandler.FromStream(stream, mimeType);
            }
            return new CefSharpSchemeHandler();
        }
    }
}
