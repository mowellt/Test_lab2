using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Testss2.Tests
{
    [TestFixture]
    public class FileServiceTests
    {
        public string path = "D:\\tests";

        private void Setup(List<string> filesToDelete, Dictionary<string, int> initFiles, bool createToRemove
        )
        {
            string[] delete = Directory.GetFiles(path);

            foreach (string f in delete)
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
            }

            foreach (var pair in initFiles)
            {
                string pathToFile = path + "\\" + pair.Key;

                if (File.Exists(pathToFile))
                {
                    File.Delete(pathToFile);
                }

                using (File.Create(pathToFile)) { }

                File.WriteAllBytes(pathToFile, new byte[pair.Value]);
            }

            if (!createToRemove)
                return;

            string removePath = path + "\\" + "ToRemove.txt";

            using (File.Create(removePath)) { }

            if (filesToDelete.Count == 0)
                return;

            List<string> fileNames = new List<string>();

            foreach (string file in filesToDelete)
            {
                var pathToFile = path + "\\" + file;
                fileNames.Add(pathToFile);
            }

            File.WriteAllLines(removePath, fileNames);
        }

        [Test]
        public void ShouldThrowhenNoToRemoveFile()
        {
            Setup(new List<string>(), new Dictionary<string, int>()
                {{ "file.png", 200 }, { "file.txt", 300 } }, false);

            FileService fileService = new FileService();

            Assert.Throws<FileNotFoundException>(() => fileService.RemoveTemporaryFiles(path));
        }

        [Test]
        public void NotThrowsWhenNoToRemoveFiles()
        {
            Setup(new List<string>(),new Dictionary<string, int>()
                {{ "file.txt", 100 },{ "file.png", 250 }},true);

            FileService fileService = new FileService();

            Assert.DoesNotThrow(() => fileService.RemoveTemporaryFiles(path));
        }

        [Test]
        public void DoesntDeleteFilesWhenNoFilesListed()
        {
            Setup(new List<string>(),new Dictionary<string, int>()
            {{ "file.txt", 100 },{ "file.png", 250 }},true);

            string[] beforeRemovalFiles = Directory.GetFiles(path);

            FileService fileService = new FileService();
            fileService.RemoveTemporaryFiles(path);

            string[] afterRemovalFiles = Directory.GetFiles(path);

            CollectionAssert.AreEquivalent(beforeRemovalFiles, afterRemovalFiles);
        }

        [Test]
        public void SizeShouldBeZero()
        {
            Setup(new List<string>(),new Dictionary<string, int>()
            {{ "file.txt", 100 },{ "file.png", 250 }},true);

            FileService fileService = new FileService();

            int size = fileService.RemoveTemporaryFiles(path);

            Assert.AreEqual(0, size);
        }

        [Test]
        public void AllListedFilesShouldBeDeleted()
        {
            Setup(new List<string>{"file.txt"},new Dictionary<string, int>()
                {{ "file.txt", 100 },{ "file.png", 250 }},true);

            FileService fileService = new FileService();

            fileService.RemoveTemporaryFiles(path);

            string[] afterRemovalFiles = Directory.GetFiles(path);

            CollectionAssert.DoesNotContain($"{path}\\file.txt", afterRemovalFiles);
        }

        [Test]
        public void NotListedFilesShouldNotBeDeleted()
        {
            Setup(new List<string>{"file.txt"},new Dictionary<string, int>()
                {{ "file.txt", 100 },{ "file.png", 250 }}, true);

            FileService fileService = new FileService();

            fileService.RemoveTemporaryFiles(path);

            string[] afterRemovalFiles = Directory.GetFiles(path);

            CollectionAssert.Contains(afterRemovalFiles, $"{path}\\file.png");
        }

        [Test]
        public void ShouldReturnCorrectSize()
        {
            Setup(new List<string>{"file.txt","file.png"},new Dictionary<string, int>()
                {{ "file.txt", 100 },{ "file.png", 250 }},true);

            FileService fileService = new FileService();

            int size = fileService.RemoveTemporaryFiles(path);

            Assert.AreEqual(100 + 250, size);
        }

        [Test]
        public void LogFileIsNotCreated()
        {
            Setup(new List<string>(), new Dictionary<string, int>()
            {{ "file.txt", 100 },{ "file.png", 250 }}, true);

            FileService fileService = new FileService();

            fileService.RemoveTemporaryFiles(path);

            Assert.IsFalse(File.Exists($"{path}\\errors.log"));
        }

        [Test]
        public void LogFileIsNotCreated2()
        {
            Setup(new List<string> {"file.txt","file.png"},
                new Dictionary<string, int>(){{ "file.txt", 100 },{ "file.png", 250 }}, true);

            FileService fileService = new FileService();

            fileService.RemoveTemporaryFiles(path);

            Assert.IsFalse(File.Exists($"{path}\\errors.log"));
        }

        [Test]
        public void LogFileShouldBeCreated2()
        {
            Setup(new List<string>{"someanbotherfile.txt","file.png"},
                new Dictionary<string, int>(){{ "file.txt", 100 },{ "file.png", 250 }},true);

            FileService fileService = new FileService();

            fileService.RemoveTemporaryFiles(path);

            Assert.IsTrue(File.Exists($"{path}\\errors.log"));
        }

        [Test]
        public void DeleteNameIsInTheLog()
        {
            Setup(new List<string>{"someanbotherfile.txt","file.png"},
                new Dictionary<string, int>(){{ "file.txt", 100 },{ "file.png", 250 }},true);

            FileService fileService = new FileService();

            fileService.RemoveTemporaryFiles(path);

            var log = File.ReadAllText($"{path}\\errors.log");

            Assert.IsTrue(log.Contains("someanbotherfile.txt does not exist"));
        }
    }
}