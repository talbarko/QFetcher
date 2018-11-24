using System;
using Microsoft.Extensions.Logging;

namespace QFetcher.Services.Readers
{
    
    public class ReadersFactory : IReadersFactory
    {
        private readonly ILogger<IQuestionsReader> _logger;

        public ReadersFactory(ILogger<IQuestionsReader> logger)
        {
            _logger = logger;
        }

        public IQuestionsReader GetReader(string type)
        {
            switch (type)
            {
                case "png":
                    return new ImageQuestionReader(_logger);
                case "json":
                    return new JsonQuestionsReader(_logger);
                case "csv":
                    return new CsvQuestionsReader(_logger);
                default:
                    throw new Exception($"file type {type} is not supported");
            }
        }
    }
}