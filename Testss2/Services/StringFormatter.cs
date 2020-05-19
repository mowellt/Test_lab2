using System;
using System.Text.RegularExpressions;

namespace Testss2.Services
{
    public class StringFormatter : IStringFormatter
    {
        public string ShortFileString(string path)
        {
            if (path == null)
                throw new NullReferenceException("Input path is null.");

            if (path == String.Empty)
                return path;

            path = path.ToUpper();

            int indexSlesh = path.LastIndexOf("/");

            if (indexSlesh != -1) path = path.Substring(indexSlesh);

            int indexDot = path.LastIndexOf(".");

            if (Regex.IsMatch(path, @"[A-Z]*\.(\w|\d){2,3}"))
            {
                path = path.Substring(indexDot) + ".TMP";
            }
            else
            {
                path = path + ".TMP";
            }

            return path;
        }
    }
}
