using AutoMapper;
using Betalgo.Ranul.OpenAI.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UsageResponse;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.PlagiarismDetector.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.FilterOptions.UsageRightFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.QueryOptions.SortOptions.UsageRightSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Services
{
    public class UsageRightService : IUsageRightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        

        public UsageRightService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<GetSoldPoemResponse>> GetSoldPoem(Guid userId, RequestOptionsBase<GetUsageRightPoemFilter, GetUsageRightPoemSort> request)
        {
            var poems = _unitOfWork.GetRepository<Poem>()
                                         .AsQueryable()
                                         .Where(ur => ur.UserId == userId && ur.DeletedTime == null);
            if (poems == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }

            if (request.FilterOptions != null)
            {
                if (!string.IsNullOrWhiteSpace(request.FilterOptions.PoemName))
                {
                    string poemNameLower = request.FilterOptions.PoemName.ToLower();

                    poems = poems.Where(p =>
                        p.Title.ToLower().Contains(poemNameLower));
                }

            }


            var saleVersions = poems.SelectMany(p => p.SaleVersions).ToList();

            var usageRights = _unitOfWork.GetRepository<UsageRight>()
                                         .AsQueryable()
                                         .Where(ur => saleVersions.Contains(ur.SaleVersion) && ur.DeletedTime == null && ur.Type == UserPoemType.PoemBuyer);
            if(usageRights == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Usage right not found");
            }

            var queryPaging = await _unitOfWork.GetRepository<UsageRight>()
                .GetPagination(usageRights, request.PageNumber, request.PageSize);


            IList<GetSoldPoemResponse> soldPoems = new List<GetSoldPoemResponse>();

            foreach (var usageRightPoem in queryPaging.Data)
            {
                var usageEntity = await _unitOfWork.GetRepository<UsageRight>().FindAsync(ur => ur.Id == usageRightPoem.Id);
                if (usageEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Usage right not found");
                }

                soldPoems.Add(_mapper.Map<GetSoldPoemResponse>(usageEntity));
                soldPoems.Last().Buyer = _mapper.Map<GetBasicUserInformationResponse>(
                                            _unitOfWork.GetRepository<User>()
                                                .AsQueryable()
                                                .Where(u => u.Id == usageRightPoem.UserId && u.DeletedTime == null)
                                                .FirstOrDefault());
                soldPoems.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(
                                            _unitOfWork.GetRepository<User>()
                                                .AsQueryable()
                                                .Where(u => u.Id == userId && u.DeletedTime == null)
                                                .FirstOrDefault());
                var poemDto = _mapper.Map<GetPoemDetailResponse>(usageRightPoem.SaleVersion.Poem);
                poemDto.SaleVersion = null; // Xóa hoặc gán lại tùy mục đích
                soldPoems.Last().Poem = poemDto;

                soldPoems.Last().SaleVersion = _mapper.Map<GetSaleVersionResponse>(usageRightPoem.SaleVersion);

            }


            return new PaginationResponse<GetSoldPoemResponse>(soldPoems, queryPaging.PageNumber, queryPaging.PageSize,
               queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<PaginationResponse<GetBoughtPoemResponse>> GetBoughtPoem(Guid userId, RequestOptionsBase<GetUsageRightPoemFilter, GetUsageRightPoemSort> request)
        {
            var utc7Now = DateTime.UtcNow.AddHours(7);
            var utc7Today = utc7Now.Date;
            var usageRights = _unitOfWork.GetRepository<UsageRight>()
                                         .AsQueryable()
                                         .Where(ur => ur.UserId == userId
                                          && ur.DeletedTime == null
                                          && ur.Type == UserPoemType.PoemBuyer);

            if (request.FilterOptions != null)
            {
                if (!string.IsNullOrWhiteSpace(request.FilterOptions.PoemName))
                {
                    string poemNameLower = request.FilterOptions.PoemName.ToLower();

                    usageRights = usageRights.Where(p =>
                        p.SaleVersion.Poem.Title.ToLower().Contains(poemNameLower));
                }

            }


            var queryPaging = await _unitOfWork.GetRepository<UsageRight>()
                .GetPagination(usageRights, request.PageNumber, request.PageSize);

            IList<GetBoughtPoemResponse> boughtPoems = new List<GetBoughtPoemResponse>();

            foreach (var usageRightPoem in queryPaging.Data)
            {
                var usageEntity = await _unitOfWork.GetRepository<UsageRight>().FindAsync(ur => ur.Id == usageRightPoem.Id);
                if (usageEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Usage right not found");
                }
                var saleVersion = await _unitOfWork.GetRepository<SaleVersion>().FindAsync(ur => ur.Id == usageRightPoem.SaleVersionId);
                boughtPoems.Add(_mapper.Map<GetBoughtPoemResponse>(usageEntity));
                boughtPoems.Last().SaleVersion = _mapper.Map<GetSaleVersionResponse>(saleVersion);
                boughtPoems.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(saleVersion.Poem.User);
                boughtPoems.Last().Buyer = _mapper.Map<GetBasicUserInformationResponse>(
                                            _unitOfWork.GetRepository<User>()
                                                .AsQueryable()
                                                .Where(u => u.Id == userId && u.DeletedTime == null)
                                                .FirstOrDefault());
                var poemDto = _mapper.Map<GetPoemDetailResponse>(saleVersion.Poem);
                poemDto.SaleVersion = null; // Xóa hoặc gán lại tùy mục đích
                boughtPoems.Last().Poem = poemDto;

            }
            return new PaginationResponse<GetBoughtPoemResponse>(boughtPoems, queryPaging.PageNumber, queryPaging.PageSize,
               queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }

        public async Task<PaginationResponse<GetPoemVersionResponse>> VersionByPoemId(Guid userId,Guid poemId, RequestOptionsBase<object, object> request)
        {
            
            var versions = _unitOfWork.GetRepository<SaleVersion>()
                                         .AsQueryable()
                                         .Where(v => v.PoemId == poemId && v.DeletedTime == null)
                                         .OrderByDescending(v => v.CreatedTime);

            /*if (request.FilterOptions != null)
            {
                if (!string.IsNullOrWhiteSpace(request.FilterOptions.PoemName))
                {
                    string poemNameLower = request.FilterOptions.PoemName.ToLower();

                    usageRights = usageRights.Where(p =>
                        p.SaleVersion.Poem.Title.ToLower().Contains(poemNameLower));
                }

            }*/


            var queryPaging = await _unitOfWork.GetRepository<SaleVersion>()
                .GetPagination(versions, request.PageNumber, request.PageSize);

            IList<GetPoemVersionResponse> poemVersions = new List<GetPoemVersionResponse>();

            foreach (var version in queryPaging.Data)
            {
                var entity = await _unitOfWork.GetRepository<SaleVersion>().FindAsync(v => v.Id == version.Id);
                if (entity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Sale version not found");
                }
                poemVersions.Add(_mapper.Map<GetPoemVersionResponse>(entity));
                poemVersions.Last().UsageRights = _mapper.Map<List<GetUserBoughtUsage>>(
                    _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                    .Where(u => u.SaleVersionId == version.Id && u.Type == UserPoemType.PoemBuyer && u.DeletedTime == null)
                    .ToList());
            }
            return new PaginationResponse<GetPoemVersionResponse>(poemVersions, queryPaging.PageNumber, queryPaging.PageSize,
               queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }

        public async Task TimeOutUsageRight()
        {
            var currentDate = DateTime.Today;
            var usageRights = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                .Where(u => u.CopyRightValidTo.Value.Date <= currentDate && u.DeletedTime == null)
                .ToList();
            //var recordsToUpdate = new List<RecordFile>();
            foreach (var usageRight in usageRights)
            {
                var records =_unitOfWork.GetRepository<RecordFile>().AsQueryable()
                    .Where(r => r.UserId == usageRight.UserId && r.SaleVersionId == usageRight.SaleVersionId && r.DeletedTime == null);
                foreach(var record in records)
                {
                    record.IsAbleToRemoveFromPoem = true;
                    _unitOfWork.GetRepository<RecordFile>().Update(record);
                    _unitOfWork.SaveChanges();
                }
            }
/*            foreach (var record in recordsToUpdate)
            {
                _unitOfWork.GetRepository<RecordFile>().Update(record);
                _unitOfWork.SaveChanges();
            }*/
        }


        public async Task RenewLicense(Guid usageRightId)
        {
            var utcPlus7 = DateTimeHelper.ConvertToUtcPlus7(DateTime.Today);

            var usageRight = await _unitOfWork.GetRepository<UsageRight>()
                .FindAsync(u => u.Id == usageRightId);

            if (usageRight == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Usage right not found");
            }

            if (usageRight.CopyRightValidTo > utcPlus7)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "License is still valid");
            }

            if (usageRight.SaleVersion == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Sale version not exist");
            }

            if (!usageRight.SaleVersion.IsInUse)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Sale version is not used, cannot renew");
            }

            // Lấy records liên quan và cập nhật
            var records = _unitOfWork.GetRepository<RecordFile>().AsQueryable()
                .Where(r => r.UserId == usageRight.UserId
                         && r.SaleVersionId == usageRight.SaleVersionId
                         && r.DeletedTime == null)
                .ToList();

            foreach (var record in records)
            {
                record.IsAbleToRemoveFromPoem = false;
                _unitOfWork.GetRepository<RecordFile>().Update(record);
            }

            // Cập nhật UsageRight
            usageRight.CopyRightValidFrom = utcPlus7.DateTime;
            usageRight.CopyRightValidTo = utcPlus7.AddDays(usageRight.SaleVersion.DurationTime).DateTime;
            _unitOfWork.GetRepository<UsageRight>().Update(usageRight);

            await _unitOfWork.SaveChangesAsync();

        }
    }
}
