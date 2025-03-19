using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Utils;
using IEmbeddingService = PoemTown.Service.PlagiarismDetector.Interfaces.IEmbeddingService;

namespace PoemTown.Service.PlagiarismDetector.Services;

public class EmbeddingService : IEmbeddingService
{
    //private readonly InferenceSession _session;
    private readonly IOpenAIService _openAiService;
    //private static readonly string ModelPath = ReadConfigurationHelper.GetModelOnnxPath();

    public EmbeddingService(IOpenAIService openAiService)
    {
    //    _session = new InferenceSession(ModelPath);
        _openAiService = openAiService;
    }

    /*public float[] GenerateEmbedding(string text)
    {
        var (inputIds, attentionMask, tokenTypeIds) = Tokenize(text);

        // Run ONNX inference
        using var results = _session.Run(new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIds),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMask),
            NamedOnnxValue.CreateFromTensor("token_type_ids", tokenTypeIds)
        });

        return results.First().AsTensor<float>().ToArray();
    }

    private (Tensor<long>, Tensor<long>, Tensor<long>) Tokenize(string text)
    {
        var tokens = text.Split(' ').Select(t => (long)t[0]).ToArray();  
        var attentionMask = Enumerable.Repeat(1L, tokens.Length).ToArray();  // All 1s (no padding)
        var tokenTypeIds = new long[tokens.Length];  // All 0s (single sentence)

        return (
            new DenseTensor<long>(tokens, new[] { 1, tokens.Length }),
            new DenseTensor<long>(attentionMask, new[] { 1, tokens.Length }),
            new DenseTensor<long>(tokenTypeIds, new[] { 1, tokens.Length })
        );
    }*/

    public async Task<double[]> GenerateEmbeddingFromOpenAI(string text)
    {
        var request = new EmbeddingCreateRequest
        {
            Input = text,
            Model = Models.TextEmbeddingV3Large,
        };

        var response = await _openAiService.Embeddings.CreateEmbedding(request);
        
        // If response is not successful then throw exception
        if (response.Successful == false)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, response.Error?.Message);

        }

        return response.Data[0].Embedding.ToArray();
    }
}