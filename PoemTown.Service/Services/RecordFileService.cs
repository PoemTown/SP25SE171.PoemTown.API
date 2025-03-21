using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Enums.RecordFile;
using PoemTown.Repository.Enums.TargetMarks;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Events.OrderEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Services
{
    public class RecordFileService : IRecordFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _awsS3Service;
        private readonly IPublishEndpoint _publishEndpoint;

        public RecordFileService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAwsS3Service awsS3Service,
            IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _awsS3Service = awsS3Service;
            _publishEndpoint = publishEndpoint;
        }

        public async Task CreateNewRecord(Guid userId, Guid poemID, CreateNewRecordFileRequest request)
        {
            // Mapping request to entity
            RecordFile record = _mapper.Map<CreateNewRecordFileRequest, RecordFile>(request);
            record.PoemId = poemID;

            // Check if user does not own the poem, user can not create record file
            UsageRight userPoemRecord = await _unitOfWork.GetRepository<UsageRight>().FindAsync(u => u.PoemId == poemID && u.UserId == userId);
            if (userPoemRecord == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not own this poem");
            }
            await _unitOfWork.GetRepository<RecordFile>().InsertAsync(record);
                userPoemRecord = new UsageRight
                {
                    UserId = userId,
                    RecordFileId = record.Id,
                    Type = UserPoemType.RecordHolder,
                    PoemId= poemID,
                };
                await _unitOfWork.GetRepository<UsageRight>().InsertAsync(userPoemRecord);
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task UpdateNewRecord(Guid userId, UpdateRecordRequest request)
        {
            RecordFile? recordFile = await _unitOfWork.GetRepository<RecordFile>().FindAsync(r => r.Id == request.Id);

            //Check if record file not found
            if (recordFile == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
            }

            //Check if record file is public, disable update price
            if (recordFile.IsPublic == true && request.Price > 0)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Can not set price with public record file");
            }

            var userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>().FindAsync(r => r.RecordFileId == request.Id && r.Type == UserPoemType.RecordBuyer);

            //Check if record file has been bought, cannot update
            if (userPoemRecordFile != null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file has been bought, cannot update");
            }
            _mapper.Map(request, recordFile);
            _unitOfWork.GetRepository<RecordFile>().Update(recordFile);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNewRecord(Guid userId, Guid recordId)
        {
            RecordFile? recordFile = await _unitOfWork.GetRepository<RecordFile>().FindAsync(r => r.Id == recordId);

            //Check if record file not found
            if (recordFile == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
            }
            var userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>().FindAsync(r => r.RecordFileId == recordId && r.Type == UserPoemType.RecordBuyer);

            //Check if record file has been bought, cannot update
            if (userPoemRecordFile != null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file has been bought, cannot update");
            }
            _unitOfWork.GetRepository<RecordFile>().Delete(recordFile);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task AssigntToPrivate(Guid userId, AssignPrivateRequest request)
        {
            RecordFile? recordFile = await _unitOfWork.GetRepository<RecordFile>().FindAsync(r => r.Id == request.RecordId);
            //Check if record file not found
            if (recordFile == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
            }
            //Check if record file already set to be private
            if (recordFile.IsPublic == false)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file already set to be private");
            }
            UsageRight? userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>().FindAsync(r => r.RecordFileId == request.RecordId && r.UserId == userId);

            //Check if record file is not yours
            if (userPoemRecordFile == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "This record file is not yours");
            }
            recordFile.IsPublic = false;
            recordFile.Price = request.Price;
            _unitOfWork.GetRepository<RecordFile>().Update(recordFile);
            _unitOfWork.SaveChanges();
        }

        public async Task PurchaseRecordFile(Guid userId, Guid recordId)
        {
            // Find record file by id
            RecordFile? recordFile = await _unitOfWork.GetRepository<RecordFile>()
                .FindAsync(p => p.Id == recordId);

            // If record file not found then throw exception
            if (recordFile == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
            }

            // If recordFile is public then throw exception
            if (recordFile.IsPublic == true)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file is public, cannot purchase");
            }

            // Find user by id
            UsageRight? userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>()
                .FindAsync(p => p.UserId == userId && p.RecordFileId == recordId);

            // If user already purchased this poem then throw exception
            if (userPoemRecordFile != null && userPoemRecordFile.Type == UserPoemType.RecordBuyer)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User already purchased this record file");
            }

            // Create new user poem record file for buyer with 2 years valid copy right
            userPoemRecordFile = new UsageRight
            {
                UserId = userId,
                RecordFileId = recordId,
                Type = UserPoemType.RecordBuyer,
                CopyRightValidFrom = DateTimeHelper.SystemTimeNow.DateTime,
                CopyRightValidTo = DateTimeHelper.SystemTimeNow.AddYears(2).DateTime
            };

            await _unitOfWork.GetRepository<UsageRight>().InsertAsync(userPoemRecordFile);
            await _unitOfWork.SaveChangesAsync();

            CreateOrderEvent message = new CreateOrderEvent()
            {
                OrderCode = OrderCodeGenerator.Generate(),
                Amount = recordFile.Price,
                Type = OrderType.RecordFiles,
                OrderDescription = $"Mua bản quyền bài ngâm thơ {recordFile.FileName}",
                Status = OrderStatus.Paid,
                RecordFileId = recordFile.Id,
                PaidDate = DateTimeHelper.SystemTimeNow,
                DiscountAmount = 0,
                UserId = userId
            };
            await _publishEndpoint.Publish(message);
        }

        public async Task<PaginationResponse<GetSoldRecordResponse>>
       GetSoldRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all records that user have
            var records = _unitOfWork.GetRepository<UsageRight>()
           .AsQueryable()
           .Where(p => p.UserId == userId && p.RecordFileId != null && p.Type == UserPoemType.RecordHolder && p.DeletedTime == null);

            var recordsList = records.ToList();
            Console.WriteLine($"Tổng số records: {recordsList.Count}");
            //Get all records have been sold
            var result = records.Select(s => new GetSoldRecordResponse
            {
                FileName = s.RecordFile.FileName,
                Price = s.RecordFile.Price,
                Owner = _mapper.Map<GetBasicUserInformationResponse>(s.User),
                Buyers = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                            .Where(u => u.RecordFileId == s.RecordFileId && u.Type == UserPoemType.RecordBuyer && u.DeletedTime == null)
                            .Select(b => _mapper.Map<GetBasicUserInformationResponse>(b.User))
                            .ToList()
            }).ToList();

            var queryPaging = await _unitOfWork.GetRepository<UsageRight>()
                .GetPagination(records, request.PageNumber, request.PageSize);

            return new PaginationResponse<GetSoldRecordResponse>(result, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }

        public async Task<PaginationResponse<GetBoughtRecordResponse>>
      GetBoughtRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all records that user have
            var records = _unitOfWork.GetRepository<UsageRight>()
           .AsQueryable()
           .Where(p => p.UserId == userId && p.RecordFileId != null && p.Type == UserPoemType.RecordBuyer && p.DeletedTime == null);

            //Get all records have been bought
            var result = records.Select(s => new GetBoughtRecordResponse
            {
                FileName = s.RecordFile.FileName,
                Price = s.RecordFile.Price,
                Buyer = _mapper.Map<GetBasicUserInformationResponse>(s.User),
                Owner = _mapper.Map<GetBasicUserInformationResponse>(
    _unitOfWork.GetRepository<UsageRight>()
        .AsQueryable()
        .Where(u => u.RecordFileId == s.RecordFileId && u.Type == UserPoemType.RecordHolder && u.DeletedTime == null)
        .Select(u => u.User) // Lấy trực tiếp User từ UserPoemRecordFile
        .FirstOrDefault() // Chỉ lấy 1 user
)
            }).ToList();
            var queryPaging = await _unitOfWork.GetRepository<UsageRight>()
                .GetPagination(records, request.PageNumber, request.PageSize);

            return new PaginationResponse<GetBoughtRecordResponse>(result, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<PaginationResponse<GetRecordFileResponse>>
      GetAllRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all records that user have
            var recordFileIds = _unitOfWork.GetRepository<UsageRight>()
                .AsQueryable()
                .Where(p => p.UserId == userId && p.RecordFileId != null && p.DeletedTime == null)
                .Select(p => p.RecordFileId)
                .Distinct()
                .ToList();
            var recordsQuery = _unitOfWork.GetRepository<RecordFile>()
                    .AsQueryable()
                    .Where(r => recordFileIds.Contains(r.Id) && r.IsPublic == true);

            if (request.IsDelete == true)
            {
                recordsQuery = recordsQuery.Where(p => p.DeletedTime != null);
            }
            else
            {
                recordsQuery = recordsQuery.Where(p => p.DeletedTime == null);
            }
            // Apply filter
            if (request.FilterOptions != null)
            {
                if (!String.IsNullOrWhiteSpace(request.FilterOptions.FileName))
                {
                    recordsQuery = recordsQuery.Where(p =>
                        p.FileName!.Contains(request.FilterOptions.FileName, StringComparison.OrdinalIgnoreCase));
                }

            }

            switch (request.SortOptions)
            {
                case GetPoemRecordFileDetailSortOption.CreatedTimeDescending:
                    recordsQuery = recordsQuery.OrderByDescending(p => p.CreatedTime);
                    break;
                case GetPoemRecordFileDetailSortOption.CreatedTimeAscending:
                    recordsQuery = recordsQuery.OrderBy(p => p.CreatedTime);
                    break;
                default:
                    recordsQuery = recordsQuery.OrderByDescending(p => p.CreatedTime);
                    break;
            }

            var queryPaging = await _unitOfWork.GetRepository<RecordFile>()
                .GetPagination(recordsQuery, request.PageNumber, request.PageSize);
            IList<GetRecordFileResponse> records = new List<GetRecordFileResponse>();
            foreach (var record in queryPaging.Data)
            {
                var poemEntity = await _unitOfWork.GetRepository<RecordFile>().FindAsync(p => p.Id == record.Id);
                if (poemEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
                }
                records.Add(_mapper.Map<GetRecordFileResponse>(poemEntity));
            }
            return new PaginationResponse<GetRecordFileResponse>(records, queryPaging.PageNumber, queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<PaginationResponse<GetRecordFileResponse>>
      GetMyRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all records that user have
            var recordFileIds = _unitOfWork.GetRepository<UsageRight>()
                .AsQueryable()
                .Where(p => p.UserId == userId && p.RecordFileId !=null && p.DeletedTime == null)
                .Select(p => p.RecordFileId)
                .Distinct()
                .ToList();
            var recordsQuery = _unitOfWork.GetRepository<RecordFile>()
                    .AsQueryable()
                    .Where(r => recordFileIds.Contains(r.Id));

            if (request.IsDelete == true)
            {
                recordsQuery = recordsQuery.Where(p => p.DeletedTime != null);
            }
            else
            {
                recordsQuery = recordsQuery.Where(p => p.DeletedTime == null);
            }
            // Apply filter
            if (request.FilterOptions != null)
            {
                if (!String.IsNullOrWhiteSpace(request.FilterOptions.FileName))
                {
                    recordsQuery = recordsQuery.Where(p =>
                        p.FileName!.Contains(request.FilterOptions.FileName, StringComparison.OrdinalIgnoreCase));
                }

            }

            switch (request.SortOptions)
            {
                case GetPoemRecordFileDetailSortOption.CreatedTimeDescending:
                    recordsQuery = recordsQuery.OrderByDescending(p => p.CreatedTime);
                    break;
                case GetPoemRecordFileDetailSortOption.CreatedTimeAscending:
                    recordsQuery = recordsQuery.OrderBy(p => p.CreatedTime);
                    break;
                default:
                    recordsQuery = recordsQuery.OrderByDescending(p => p.CreatedTime);
                    break;
            }

            var queryPaging = await _unitOfWork.GetRepository<RecordFile>()
                .GetPagination(recordsQuery, request.PageNumber, request.PageSize);
            IList<GetRecordFileResponse> records = new List<GetRecordFileResponse>();
            foreach (var record in queryPaging.Data)
            {
                var poemEntity = await _unitOfWork.GetRepository<RecordFile>().FindAsync(p => p.Id == record.Id);
                if (poemEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
                }
                records.Add(_mapper.Map<GetRecordFileResponse>(poemEntity));
            }
            return new PaginationResponse<GetRecordFileResponse>(records, queryPaging.PageNumber, queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }
    }
}
