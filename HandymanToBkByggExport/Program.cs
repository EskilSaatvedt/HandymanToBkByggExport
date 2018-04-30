using System;
using System.IO;
using System.Linq;
using System.Net;
using HandymanToBkByggExport.Properties;

namespace HandymanToBkByggExport
{
    class Program
    {
        static void Main(string[] args)
        {
			// Copy all new files from eksport folder to destination folder
	        Program.Copy(Settings.Default.SourcePath, Settings.Default.DestinationPath);
			var client = new WebClient();
            string reply = client.DownloadString(new Uri(Settings.Default.BKByggURL));
            Console.WriteLine(reply);
		}

	    public static void Copy(string sourceDir, string targetDir)
	    {
		    Directory.CreateDirectory(targetDir);

		    // Files on the form eOrdr001.BYT
		    var files = Directory.EnumerateFiles(sourceDir, "*.*", SearchOption.TopDirectoryOnly)
			    .Where(s => s.EndsWith(Settings.Default.FileExtention) && Path.GetFileName(s).StartsWith(Settings.Default.FilePreFix));

		    foreach (var file in files)
		    {
			    FileInfo fi = new FileInfo(file);
			    FileInfo destFile = new FileInfo(Path.Combine(targetDir, fi.Name));
			    if (destFile.Exists)
			    {
				    if (fi.LastWriteTime > destFile.LastWriteTime)
				    {
					    // now you can safely overwrite it
					    File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)),true);
				    }
			    }
			    else
			    {
					File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
				}
			}
	    }
	}
}