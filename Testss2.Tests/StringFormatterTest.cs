using NUnit.Framework;
using System;
using System.Linq;
using Testss2.Services;

namespace Testss2.Tests
{
    [TestFixture]
    public class StringFormatterTest
    {
        private IStringFormatter _stringFormatter;

        [SetUp]
        public void TestInit()
        {
            _stringFormatter = new StringFormatter();
        }

        [Test]
        public void NullStringTest()
        {
            string path = null;

            Assert.Throws<NullReferenceException>(() => _stringFormatter.ShortFileString(path));
        }

        [Test]
        public void EmptyStringTest()
        {
            string path = String.Empty;
            string expected = String.Empty;

            var actual = _stringFormatter.ShortFileString(path);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("E:/folder/file.txt")]
        [TestCase("folder/file.txt")]
        [TestCase("file.txt")]
        [TestCase(".txt")]
        public void OnlyShortPathTest(string path)
        {
            var actual = _stringFormatter.ShortFileString(path);

            StringAssert.DoesNotContain("/", actual);
        }

        [Test]
        [TestCase("folder/file.txt")]
        [TestCase("folder/file")]
        [TestCase("file.txt")]
        [TestCase("file")]
        [TestCase(".txt")]
        public void ChangeExtensionFileTest(string path)
        {
            var actual = _stringFormatter.ShortFileString(path);

            StringAssert.EndsWith(".TMP", actual);
        }

        [Test]
        [TestCase("E:/folder/file.txt")]
        [TestCase("FOLDER/file.txt")]
        [TestCase("FILE.txt")]
        [TestCase(".TXT")]
        public void OnlyUpperCharsTest(string path)
        {
            var actual = _stringFormatter.ShortFileString(path);

            Assert.IsFalse(actual.Any(char.IsLower));
        }
    }
}
