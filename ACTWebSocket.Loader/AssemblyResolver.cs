using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ACTWebSocket_Plugin
{
    class AssemblyResolver : IDisposable
    {
        static readonly Regex assemblyNameParser = new Regex(
            @"(?<name>.+?), Version=(?<version>.+?), Culture=(?<culture>.+?), PublicKeyToken=(?<pubkey>.+)",
            RegexOptions.Compiled);

        public List<string> Directories { get; set; }

        public AssemblyResolver(IEnumerable<string> directories)
        {
            Directories = new List<string>();
            if (directories != null)
            {
                Directories.AddRange(directories);
            }
            AppDomain.CurrentDomain.AssemblyResolve += CustomAssemblyResolve;
        }

        public AssemblyResolver()
            : this(null)
        {

        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CustomAssemblyResolve;
        }

        private Assembly CustomAssemblyResolve(object sender, ResolveEventArgs e)
        {
            // Directories プロパティで指定されたディレクトリを基準にアセンブリを検索する
            foreach (var directory in this.Directories)
            {
                var asmPath = "";
                var match = assemblyNameParser.Match(e.Name);
                if (match.Success)
                {
                    var asmFileName = match.Groups["name"].Value + ".dll";
                    if (match.Groups["culture"].Value == "neutral")
                    {
                        asmPath = Path.Combine(directory, asmFileName);
                    }
                    else
                    {
                        asmPath = Path.Combine(directory, match.Groups["culture"].Value, asmFileName);
                    }
                }
                else
                {
                    asmPath = Path.Combine(directory, e.Name + ".dll");
                }

                if (File.Exists(asmPath))
                {
                    var asm = Assembly.LoadFile(asmPath);
                    OnAssemblyLoaded(asm);
                    return asm;
                }
            }

            return null;
        }

        private Assembly GetAssembly(string path)
        {
            try
            {
                var result = Assembly.LoadFrom(path);
                return result;
            }
            catch (Exception e)
            {
                OnExceptionOccured(e);
            }

            return null;
        }

        protected void OnExceptionOccured(Exception exception)
        {
            if (ExceptionOccured != null)
            {
                ExceptionOccured(this, new ExceptionOccuredEventArgs(exception));
            }
        }

        protected void OnAssemblyLoaded(Assembly assembly)
        {
            if (AssemblyLoaded != null)
            {
                AssemblyLoaded(this, new AssemblyLoadEventArgs(assembly));
            }
        }

        public event EventHandler<ExceptionOccuredEventArgs> ExceptionOccured;
        public event EventHandler<AssemblyLoadEventArgs> AssemblyLoaded;

        public class ExceptionOccuredEventArgs : EventArgs
        {
            public Exception Exception { get; set; }
            public ExceptionOccuredEventArgs(Exception exception)
            {
                Exception = exception;
            }
        }
    }
}