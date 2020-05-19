using Moq;
using NUnit.Framework;
using System;
using Testss2.Services;
using Testss2.Tests.CustomMocks;

namespace Testss2.Tests
{
    [TestFixture]
    public class TemporaryFileRepositoryTest
    {
        [Test]
        public void NullStringTest()
        {
            string path = null;

            var stringFormatter = new NullStringFormatterMock();

            var fileRepo = new TemporaryFileRepository(stringFormatter);

            Assert.Throws<NullReferenceException>(() => fileRepo.Add(path));
        }

        [Test]
        public void EmptyStringTest()
        {
            string path = String.Empty;
            string expected = String.Empty;

            var stringFormatter = new EmptyStringFormatterMock();

            var fileRepo = new TemporaryFileRepository(stringFormatter);
            fileRepo.Add(path);

            CollectionAssert.Contains(fileRepo.Paths, expected);
        }

        [Test]
        [TestCase("E:/folder/file1.txt", "FILE1.TMP")]
        [TestCase("folder/file2.txt", "FILE2.TMP")]
        [TestCase("file3.txt", "FILE3.TMP")]
        [TestCase(".txt", ".TMP")]
        public void CorrectStringTest(string path, string expected)
        {
            var stringFormatter = new CorrectStringFormatterMock(expected);

            var fileRepo = new TemporaryFileRepository(stringFormatter);
            fileRepo.Add(path);

            CollectionAssert.Contains(fileRepo.Paths, expected);
        }
    }

    [TestFixture]
    public class TemporaryFileRepositoryTestMock
    {
        private Mock<IStringFormatter> _stringFormatterMock;

        [OneTimeSetUp]
        public void TestFixtureInit()
        {
            _stringFormatterMock = new Mock<IStringFormatter>();
        }

        [Test]
        public void NullStringTest()
        {
            string path = null;

            _stringFormatterMock.Setup(s => s.ShortFileString(path)).Throws<NullReferenceException>();

            var fileRepo = new TemporaryFileRepository(_stringFormatterMock.Object);

            Assert.Throws<NullReferenceException>(() => fileRepo.Add(path));
        }

        [Test]
        public void EmptyStringTest()
        {
            string path = String.Empty;
            string expected = String.Empty;

            _stringFormatterMock.Setup(s => s.ShortFileString(path)).Returns(String.Empty);

            var fileRepo = new TemporaryFileRepository(_stringFormatterMock.Object);
            fileRepo.Add(path);

            CollectionAssert.Contains(fileRepo.Paths, expected);
        }

        [Test]
        [TestCase("E:/folder/file1.txt", "FILE1.TMP")]
        [TestCase("folder/file2.txt", "FILE2.TMP")]
        [TestCase("file3.txt", "FILE3.TMP")]
        [TestCase(".txt", ".TMP")]
        public void CorrectStringTest(string path, string expected)
        {
            _stringFormatterMock.Setup(s => s.ShortFileString(path)).Returns(expected);

            var fileRepo = new TemporaryFileRepository(_stringFormatterMock.Object);
            fileRepo.Add(path);

            CollectionAssert.Contains(fileRepo.Paths, expected);
        }
    }
}
