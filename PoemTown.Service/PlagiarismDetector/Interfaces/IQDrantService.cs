using PoemTown.Service.PlagiarismDetector.PDModels;

namespace PoemTown.Service.PlagiarismDetector.Interfaces;

public interface IQDrantService
{
    Task StorePoemEmbeddingAsync(Guid poemId, Guid poetId, string poemText);
    Task<QDrantResponse<SearchPointsResult>?> SearchSimilarPoemEmbeddingPoint(Guid userId, string poemText);
    
}