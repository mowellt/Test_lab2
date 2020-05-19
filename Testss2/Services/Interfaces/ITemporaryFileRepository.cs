using System.Collections.Generic;

namespace Testss2.Services
{
    public interface ITemporaryFileRepository
    {
        ICollection<string> Paths { get; }

        void Add(string path);
    }
}
