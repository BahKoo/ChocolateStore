using System;
using System.IO;

namespace ChocolateStore
{
    class Program
    {

        static void Main(string[] args)
        {

            PackageCacher cacher = new PackageCacher();

            cacher.DownloadingFile += cacher_DownloadingFile;
            cacher.SkippingFile += cacher_SkippingFile;

            try
            {
                var arguments = ParseArguments(args);

                if (arguments != null)
                {
                    cacher.CachePackage(arguments.Directory, arguments.Url);
                }

            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
            }

        }

        private static Arguments ParseArguments(string[] args)
        {

            Arguments arguments = new Arguments();

            if (args.Length != 2)
            {
                WriteError("USAGE: ChocolateStore <directory> <url>");
                return null;
            }

            arguments.Directory = args[0];

            if (!Directory.Exists(arguments.Directory))
            {
                WriteError("Directory '{0}' does not exist.", arguments.Directory);
                return null;
            }

            arguments.Url = args[1];

            if (!Uri.IsWellFormedUriString(arguments.Url, UriKind.Absolute))
            {
                WriteError("URL '{0}' is invalid.", arguments.Url);
                return null;
            }

            return arguments;

        }

        private static void cacher_DownloadingFile(string fileName)
        {
            WriteInfo("Downloading: {0}", fileName);
        }

        private static void cacher_SkippingFile(string fileName)
        {
            WriteWarning("Skipped: {0} - File already exists on disk.", fileName);
        }

        private static void WriteInfo(string format, params object[] arg)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(format, arg);
            Console.ResetColor();
        }

        private static void WriteWarning(string format, params object[] arg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, arg);
            Console.ResetColor();
        }

        private static void WriteError(string format, params object[] arg)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(format, arg);
            Console.ResetColor();
        }

    }
}
