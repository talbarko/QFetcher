using System.Collections.Generic;
using Moq;
using QFetcher.Models;
using QFetcher.Services.Readers;
using Xunit;

namespace QFetcher.Services.Tests
{
    public class QuestionsAggregatorTests
    {
        [Fact]
        public void GetAll_WhenNoSources_RetutnEmptyList()
        {
            //Arrange

            const string filePath = "file.dat";
            var readersFactory = Mock.Of<IReadersFactory>();
            var fileManager = Mock.Of<IFileManager>();
            Mock.Get(fileManager).Setup(x => x.ReadText(filePath)).Returns(new List<string>());

            //Act
            var result = new QuestionAggregator(fileManager, readersFactory).GetAll(filePath);

            //Assert
            Assert.Empty(result);
        }
        
        [Fact]
        public void GetAll_WhenOnlyOneSourceFile_RetutnListWithQuestions()
        {
            //Arrange
            const string filePath = "file.dat";
            const string type = "json";
            const string sourcePath = "url.json";
            
            var reader = Mock.Of<IQuestionsReader>();
            Mock.Get(reader).Setup(x => x.ReadFromSource(sourcePath)).Returns(new List<Question>
            {
                new Question {Source = "read1", Value = "how are you?"},
                new Question {Source = "read1", Value = "what is the time?"}
            });
            var readersFactory = Mock.Of<IReadersFactory>();
            Mock.Get(readersFactory).Setup(x => x.GetReader(type)).Returns(reader);

            var fileManager = Mock.Of<IFileManager>();
            Mock.Get(fileManager).Setup(x => x.ReadText(filePath)).Returns(new List<string>{sourcePath});

            //Act
            var actual = new QuestionAggregator(fileManager, readersFactory).GetAll(filePath);

            //Assert
            var expectedResult = new List<Question>
            {
                new Question {Source = "read1", Value = "how are you?"},
                new Question {Source = "read1", Value = "what is the time?"}
            };

            TestUtils.AreListObjectsJsonEqual(expectedResult, actual);
        }
        
        [Fact]
        public void GetAll_WhenOnlyMoreThanOneSourceFile_RetutnListWithQuestionsFromBothSources()
        {
            //Arrange
            const string filePath = "file.dat";
            const string firstType = "json";
            const string secondType = "csv";
            const string firstSourcePath = "url.json";
            const string secondSourcePath = "url.csv";
            
            var firstReader = Mock.Of<IQuestionsReader>();
            Mock.Get(firstReader).Setup(x => x.ReadFromSource(firstSourcePath)).Returns(new List<Question>
            {
                new Question {Source = "read1", Value = "how are you?"},
                new Question {Source = "read1", Value = "what is the time?"}
            });
            
            var secondReader = Mock.Of<IQuestionsReader>();
            Mock.Get(secondReader).Setup(x => x.ReadFromSource(secondSourcePath)).Returns(new List<Question>
            {
                new Question {Source = "read2", Value = "can i help you?"},
                new Question {Source = "read2", Value = "where do you live?"}
            });
            
            var readersFactory = Mock.Of<IReadersFactory>();
            Mock.Get(readersFactory).Setup(x => x.GetReader(firstType)).Returns(firstReader);
            Mock.Get(readersFactory).Setup(x => x.GetReader(secondType)).Returns(secondReader);

            var fileManager = Mock.Of<IFileManager>();
            Mock.Get(fileManager).Setup(x => x.ReadText(filePath)).Returns(new List<string>{firstSourcePath,secondSourcePath});

            //Act
            var actual = new QuestionAggregator(fileManager, readersFactory).GetAll(filePath);

            //Assert
            var expectedResult = new List<Question>
            {
                new Question {Source = "read1", Value = "how are you?"},
                new Question {Source = "read1", Value = "what is the time?"},
                new Question {Source = "read2", Value = "can i help you?"},
                new Question {Source = "read2", Value = "where do you live?"}
            };

            TestUtils.AreListObjectsJsonEqual(expectedResult, actual);
        }
    }
}