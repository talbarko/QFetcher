using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFetcher.Models;
using QFetcher.Services;

namespace QFetcher.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/fetch")]
    public class FetchController : Controller
    {
        private readonly QuestionAggregator _questionAggregator;
        private readonly ILogger<FetchController> _logger;

        public FetchController(QuestionAggregator questionAggregator,ILogger<FetchController> logger)
        {
            _questionAggregator = questionAggregator;
            _logger = logger;
        }

        [HttpGet]
        public List<Question> GetAll()
        {
            try
            {
                return _questionAggregator.GetAll("manifest.dat");
            }
            catch (Exception e)
            {
                _logger.LogError("Error while Fetching questions: " + e.Message);
                throw;
            }
        }
    }
}