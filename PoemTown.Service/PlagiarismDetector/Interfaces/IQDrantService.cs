using PoemTown.Service.PlagiarismDetector.PDModels;

namespace PoemTown.Service.PlagiarismDetector.Interfaces;

public interface IQDrantService
{
    Task StorePoemEmbeddingAsync(Guid poemId, Guid poetId, string poemText, bool? isFamousPoem = false);
    Task<QDrantResponse<SearchPointsResult>?> SearchSimilarPoemEmbeddingPoint(Guid userId, string poemText);
    Task DeletePoemEmbeddingPoint(IList<Guid> poemIds);
}