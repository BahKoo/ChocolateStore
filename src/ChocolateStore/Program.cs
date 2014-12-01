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
                Console.Write(ex.ToString());
            }

        }

        private static Arguments ParseArguments(string[] args)
        {

            Arguments arguments = new Arguments();

            if (args.Length != 2)
            {
                Console.WriteLine("USAGE: ChocolateStore <directory> <url>");
                return null;
            }

            arguments.Directory = args[0];

            if (!Directory.Exists(arguments.Directory))
            {
                Console.WriteLine("Directory '{0}' does not exist.", arguments.Directory);
                return null;
            }

            arguments.Url = args[1];

            if (!Uri.IsWellFormedUriString(arguments.Url, UriKind.Absolute))
            {
                Console.WriteLine("URL '{0}' is invalid.", arguments.Url);
                return null;
            }

            return arguments;

        }

        private static void cacher_DownloadingFile(string fileName)
        {
            Console.WriteLine("Storing: {0}", fileName);
        }

        private static void cacher_SkippingFile(string fileName)
        {
            Console.WriteLine("Skipping: {0}", fileName);
        }

    }
}
