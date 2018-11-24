using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace QFetcher.Services
{
    public class FileManager : IFileManager
    {
        private readonly ILogger<FileManager> _logger;

        public FileManager(ILogger<FileManager> logger)
        {
            _logger = logger;
        }

        public List<string> ReadText(string path)
        {
            try
            {
                return File.ReadAllLines(path).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while trying to read file: {path}, error: {e.Message}");
                throw;
            }
        }
    }
}