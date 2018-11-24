using System.Collections.Generic;

namespace QFetcher.Services
{
    public interface IFileManager
    {
        List<string> ReadText(string path);
    }
}