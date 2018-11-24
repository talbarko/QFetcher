using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QFetcher.Models;
using QFetcher.Models.JsonReaderInput;

namespace QFetcher.Services.Readers

{
    public class JsonQuestionsReader : IQuestionsReader
    {
        private readonly ILogger<IQuestionsReader> _logger;

        public JsonQuestionsReader(ILogger<IQuestionsReader> logger)
        {
            _logger = logger;
        }

        public List<Question> ReadFromSource(string url)
        {
            try
            {
                var http = new HttpClient();
                var data = http.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                var parsedQuestions = JsonConvert.DeserializeObject<InputQuestionsWrapper>(data);
                return parsedQuestions.Questions.Select(x=> new Question{Value = x.Text,Source = "json"}).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while trying to read json: {url}, Message {e.Message}");
                throw;
            }
        }
    }
}