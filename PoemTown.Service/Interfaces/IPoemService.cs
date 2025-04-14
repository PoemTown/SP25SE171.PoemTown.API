using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.PlagiarismDetector.PDModels;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.ThirdParties.Models.TheHiveAi;

namespace PoemTown.Service.Interfaces;

public interface IPoemService
{
    Task CreateNewPoem(Guid userId, CreateNewPoemRequest request);

    Task<PaginationResponse<GetPoemResponse>> GetMyPoems
        (Guid userId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request);
    Task<PaginationResponse<GetPoemInCollectionResponse>> GetPoemsInCollection
       (Guid? userId, Guid collectionId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request);
    Task UpdatePoem(Guid userId, UpdatePoemRequest request);

    Task DeletePoem(Guid poemId);
    Task DeletePoemPermanent(Guid poemId);
    Task<GetPoemDetailResponse> 
        GetPoemDetail(Guid? userId, Guid poemId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
    Task<PaginationResponse<GetPostedPoemResponse>> 
        GetPostedPoems(Guid? userId, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request);
    Task<PaginationResponse<GetPostedPoemResponse>>
        GetTrendingPoems(Guid? userId, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request);

    Task<string> UploadPoemImage(Guid userId, IFormFile file);

    Task SellingSaleVersionPoem(Guid userId, SellingSaleVersionPoemRequest request);
    Task<string> PoemAiChatCompletion(PoemAiChatCompletionRequest request);
    Task<string> ConvertPoemTextToImage(Guid userId, ConvertPoemTextToImageRequest request);

    Task<TheHiveAiResponse> ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhanced(
        Guid userId, ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhancedRequest request);

    Task<TheHiveAiResponse> ConvertPoemTextToImageWithTheHiveAiSdxlEnhanced(
        Guid userId, ConvertPoemTextToImageWithTheHiveAiSdxlEnhancedRequest request);

    Task PurchasePoemCopyRight(Guid userId, Guid saleVersionId);
    Task CreatePoemInCommunity(Guid userId, CreateNewPoemRequest request);
    Task DeletePoemInCommunity(Guid userId, Guid poemId);
    Task ConvertPoemIntoEmbeddingAndSaveToQdrant(Guid poemId);
    Task<PoemPlagiarismResponse> CheckPoemPlagiarism(Guid userId, CheckPoemPlagiarismRequest request);
    bool IsPoemPlagiarism(double score);
    Task FreeSaleVersionPoem(Guid userId, Guid poemId);

    Task<PaginationResponse<GetUserPoemResponse>>
        GetUserPoems(Guid? userId, string userName, RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request);

    IList<SearchPointsResult> GetListQDrantSearchPoint(QDrantResponse<SearchPointsResult> response, int top);
    Task AdminUpdatePoemStatus(Guid poemId, PoemStatus status);
    Task<string> DownloadAiImageAndUploadToS3Storage(UploadAiPoemImageRequest request, Guid userId);
    Task RemoveRecordFileFromPoem(Guid userId, Guid recordFileId);
    Task CreatePoetSamplePoem(Guid poetSampleId, CreatePoetSamplePoemRequest request);

    Task<PaginationResponse<GetPoetSamplePoemResponse>>
        GetPoetSamplePoems(Guid? userId, Guid? poetSampleId,
            RequestOptionsBase<GetPoetSamplePoemFilterOption, GetPoetSamplePoemSortOption> request);

    Task UpdatePoetSamplePoem(Guid poetSampleId, UpdatePoetSamplePoemRequest request);
    Task DeletePoetSamplePoem(Guid poetSampleId, Guid poemId);
    Task UpdatePoetSampleSaleVersionCommissionPercentage(Guid poemId, int commissionPercentage);
}