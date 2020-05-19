using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testss2
{
    public sealed class FileService : IFileService
    {
        private string RemoveFile = "ToRemove.txt";
        private string Log = "errors.log";

        public int RemoveTemporaryFiles(string dir)
        {
            if (!Directory.Exists(dir))
                throw new InvalidOperationException();

            string remove = dir + "\\" + RemoveFile;//;Path.Combine(dir, RemoveFile);

            if (!File.Exists(remove))
                throw new FileNotFoundException();

            List<string> errorList = new List<string>();
            List<long> sizeList = new List<long>();

            foreach (var line in File.ReadAllLines(remove))
                if (!File.Exists(line))
                {
                    string fileName = Path.GetFileName(line);

                    errorList.Add($"{fileName} does not exist");
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(line);
                    sizeList.Add(fileInfo.Length);

                    File.Delete(line);
                }

            if (errorList.Any())
            {
                string log = dir + "\\" + Log; // Path.Combine(dir, Log);

                using (File.Create(log)) { }

                File.WriteAllLines(log, errorList);
            }

            var sum = sizeList.Sum(item => item);

            return (int)sum;
        }
    }
}
