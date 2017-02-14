using System;
using System.IO;

namespace ChocolateStore
{
    class Program
    {

        static void Main(string[] args)
        {

            PackageCacher cacher = new PackageCacher();

            cacher.SkippingFile += cacher_SkippingFile;
            cacher.DownloadingFile += cacher_DownloadingFile;
            cacher.DownloadFailed += cacher_DownloadFailed;

            try
            {
                var arguments = ParseArguments(args);

                if (arguments != null)
                {
                    cacher.CachePackage(arguments.Directory, arguments.Url, arguments.UseRelativePaths);
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

            if (args.Length != 2 && args.Length != 3)
            {
                WriteError("USAGE: ChocolateStore <directory> <url> [options]");
                WriteError("options:");
                WriteError(" -r,   use relative paths instead of absolute paths in package file");
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

            if (args.Length > 2 && args[2].Equals("-r"))
            {
                arguments.UseRelativePaths = true;
            }

            return arguments;

        }

        private static void cacher_SkippingFile(string fileName)
        {
            WriteWarning("Skipped: {0} - File already exists on disk.", fileName);
        }

        private static void cacher_DownloadingFile(string fileName)
        {
            WriteInfo("Downloading: {0}", fileName);
        }

        private static void cacher_DownloadFailed(string url, Exception ex)
        {
            WriteError("Download Failed: {0}", url);
            Console.WriteLine(ex.ToString());
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
