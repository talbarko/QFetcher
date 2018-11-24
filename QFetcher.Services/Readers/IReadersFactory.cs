namespace QFetcher.Services.Readers
{
    public interface IReadersFactory
    {
        IQuestionsReader GetReader(string type);
    }
}