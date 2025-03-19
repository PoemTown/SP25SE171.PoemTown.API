namespace PoemTown.Service.PlagiarismDetector.Interfaces;

public interface IEmbeddingService
{
    Task<double[]> GenerateEmbeddingFromOpenAI(string text);
}