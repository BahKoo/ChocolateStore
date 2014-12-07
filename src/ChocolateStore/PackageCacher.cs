using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace ChocolateStore
{
	class PackageCacher
	{

		private const string INSTALL_FILE = "tools/chocolateyInstall.ps1";

		public delegate void FileHandler(string fileName);
		public event FileHandler DownloadingFile = delegate {};
		public event FileHandler SkippingFile = delegate { };

		public void CachePackage(string dir, string url)
		{
			var packagePath = DownloadFile(url, dir);

			using (var zip = ZipFile.Read(packagePath))
			{
			    var entry = zip.FirstOrDefault(x => string.Equals(x.FileName, INSTALL_FILE, StringComparison.OrdinalIgnoreCase));

				if (entry != null) {
					string content = null;
					var packageName = Path.GetFileNameWithoutExtension(packagePath);

					using (MemoryStream ms = new MemoryStream()) {
						entry.Extract(ms);
						content = Encoding.UTF7.GetString(ms.ToArray());
					}

					content = CacheUrlFiles(Path.Combine(dir, packageName), content);
					zip.UpdateEntry(INSTALL_FILE, content);
					zip.Save();

				}

			}

		}

		private string CacheUrlFiles(string folder, string content)
		{

			const string pattern = "(?<=['\"])http\\S*(?=['\"])";

			if (!Directory.Exists(folder)) {
				Directory.CreateDirectory(folder);
			}

			return Regex.Replace(content, pattern, new MatchEvaluator(m => DownloadFile(m.Value, folder)));

		}

		private string DownloadFile(string url, string destination)
		{

			var request = WebRequest.Create(url);
			var response = request.GetResponse();
			var fileName = Path.GetFileName(response.ResponseUri.LocalPath);
			var filePath = Path.Combine(destination, fileName);

			if (File.Exists(filePath))
			{
				SkippingFile(fileName);
			}
			else
			{
				DownloadingFile(fileName);
				using (var fs = File.Create(filePath))
				{
					response.GetResponseStream().CopyTo(fs);
				}
			}

			return filePath;

		}

	}
}