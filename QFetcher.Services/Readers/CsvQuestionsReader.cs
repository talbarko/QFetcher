using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using QFetcher.Models;

namespace QFetcher.Services.Readers
{
    public class CsvQuestionsReader : IQuestionsReader
    {
        private readonly ILogger<IQuestionsReader> _logger;

        public CsvQuestionsReader(ILogger<IQuestionsReader> logger)
        {
            _logger = logger;
        }

        public List<Question> ReadFromSource(string url)
        {
            try
            {
                var http = new HttpClient();
                var data = http.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                var result = ParseResult(data);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while trying to read csv: {url}, Message {e.Message}");
                throw;
            }
        }

        private List<Question> ParseResult(string data)
        {
            var rows = data.Split(Environment.NewLine).ToList();
            rows.RemoveAt(0);

            var result = new List<Question>();
            foreach (var row in rows)
            {
                var fields = row.Split(',');
                var question = new Question {Value = fields[1], Source = "csv"};
                result.Add(question);
            }

            return result;
        }
    }
}