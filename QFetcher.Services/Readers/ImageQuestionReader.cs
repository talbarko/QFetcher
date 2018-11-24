using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QFetcher.Models;

namespace QFetcher.Services.Readers
{
    public class ImageQuestionReader : IQuestionsReader
    {
        private readonly ILogger<IQuestionsReader> _logger;
        private const string ApiKey = "AIzaSyBBeDLFc9v1K2CIWKSMqhwcDdDHYO9Xuac";

        public ImageQuestionReader(ILogger<IQuestionsReader> logger)
        {
            _logger = logger;
        }

        public List<Question> ReadFromSource(string url)
        {
            try
            {
                var http = new HttpClient();
                var request = JsonConvert.SerializeObject(GetRequestContent(url));
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var response = http.PostAsync(
                    $"https://vision.googleapis.com/v1/images:annotate?key={ApiKey}",
                    content);

                var result = response.Result.Content.ReadAsStringAsync().Result;

                var text = ParseResult(result);
                return new List<Question> {new Question {Value = text,Source = "image"}};
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while trying to read image: {url}, Message {e.Message}");
                throw;
            }
        }

        private static string ParseResult(string result)
        {
            return JObject.Parse(result)["responses"][0]["textAnnotations"][0]["description"].ToString();
        }

        private static object GetRequestContent(string url)
        {
            return new
            {
                requests = new[]
                {
                    new
                    {
                        image = new
                        {
                            source = new
                            {
                                imageUri = url
                            }
                        },
                        features = new[]
                        {
                            new
                            {
                                type = "TEXT_DETECTION"
                            }
                        }
                    }
                }
            };
        }
    }
}