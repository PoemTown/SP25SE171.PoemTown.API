using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.StatisticResponse;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StatisticResponse> GetStatisticsAsync(Guid userId)
    {
            var collections = _unitOfWork.GetRepository<Collection>()
                .AsQueryable()
                .Where(c => c.UserId == userId && c.DeletedTime == null);

            // Nếu không có bộ sưu tập, trả về thống kê mặc định
            if (!await collections.AnyAsync())
            {
                return new StatisticResponse();
            }

            // Lấy danh sách bài thơ chưa bị xóa từ các bộ sưu tập
            var poemsQuery = collections
                .SelectMany(c => c.Poems)
                .Where(p => p.DeletedTime == null);

            // Truy vấn tuần tự để tránh lỗi DbContext bị sử dụng đồng thời
            var totalCollections = await collections.CountAsync();
            var totalPoems = await poemsQuery.CountAsync();
            var totalRecords = await poemsQuery.SelectMany(p => p.RecordFiles).CountAsync(r => r.DeletedTime == null);
            var totalCollectionBookmarks = await collections.SelectMany(p => p.TargetMarks).CountAsync(l => l.DeletedTime == null);
            var totalLikes = await poemsQuery.SelectMany(p => p.Likes).CountAsync(l => l.DeletedTime == null);
            var totalPoemBookmarks = await poemsQuery.SelectMany(p => p.TargetMarks).CountAsync(l => l.DeletedTime == null);

            // Trả về kết quả
            return new StatisticResponse
            {
                TotalCollections = totalCollections,
                TotalPoems = totalPoems,
                TotalLikes = totalLikes,
                TotalPersonalAudios = totalRecords,
                PoemBookmarks = totalPoemBookmarks,
                CollectionBookmarks = totalCollectionBookmarks
            };
        }
       
}

