using System;
using Testss2.Services;

namespace Testss2.Tests.CustomMocks
{
    public class NullStringFormatterMock : IStringFormatter
    {
        public string ShortFileString(string path)
        {
            throw new NullReferenceException();
        }
    }

    public class EmptyStringFormatterMock : IStringFormatter
    {
        public string ShortFileString(string path)
        {
            return String.Empty;
        }
    }

    public class CorrectStringFormatterMock : IStringFormatter
    {
        private readonly string _returnedString;

        public CorrectStringFormatterMock(string path)
        {
            _returnedString = path;
        }

        public string ShortFileString(string path)
        {
            return _returnedString;
        }
    }
}
