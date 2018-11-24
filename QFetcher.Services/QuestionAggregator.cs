using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using QFetcher.Models;
using System.Linq;
using System.Threading.Tasks;
using QFetcher.Services.Readers;


namespace QFetcher.Services
{
    public class QuestionAggregator
    {
        private readonly IFileManager _fileManager;
        private readonly IReadersFactory _readersFactory;

        public QuestionAggregator(IFileManager fileManager,IReadersFactory readersFactory)
        {
            _fileManager = fileManager;
            _readersFactory = readersFactory;
        }

        public List<Question> GetAll(string path)
        {
            var aggregatedQuestions = new ConcurrentBag<Question>();
            var readText = _fileManager.ReadText(path);
            Parallel.ForEach(readText, sourceUrl =>
                {
                    var fileSuffix = sourceUrl.Substring(sourceUrl.LastIndexOf(".", StringComparison.Ordinal) + 1);

                    var reader = _readersFactory.GetReader(fileSuffix);
                    var readerResult = reader.ReadFromSource(sourceUrl);
                    aggregatedQuestions.AddRange(readerResult);
                }
            );

            return aggregatedQuestions.ToList();
        }
    }
}