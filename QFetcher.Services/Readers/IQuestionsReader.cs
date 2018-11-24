using System.Collections.Generic;
using QFetcher.Models;

namespace QFetcher.Services.Readers
{
    public interface IQuestionsReader
    {
        List<Question> ReadFromSource(string url);
    }
}