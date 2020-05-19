using System.Collections.Generic;

namespace Testss2.Services
{
    public class TemporaryFileRepository : ITemporaryFileRepository
    {
        private readonly IStringFormatter _stringFormatter;

        public ICollection<string> Paths { get; private set; }

        public TemporaryFileRepository(IStringFormatter stringFormatter)
        {
            _stringFormatter = stringFormatter;

            Paths = new List<string>();
        }

        public void Add(string path)
        {
            Paths.Add(_stringFormatter.ShortFileString(path));

            // It's all?
        }
    }
}
