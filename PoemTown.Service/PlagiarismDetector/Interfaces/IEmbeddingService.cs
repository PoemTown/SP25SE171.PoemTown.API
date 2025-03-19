namespace PoemTown.Service.PlagiarismDetector.Interfaces;

public interface IEmbeddingService
{
    float[] GenerateEmbedding(string text);
    Task<double[]> GenerateEmbeddingFromOpenAI(string text);
}