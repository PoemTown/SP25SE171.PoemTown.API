using AutoMapper;
using Castle.Core.Logging;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Orders;
using PoemTown.Repository.Enums.RecordFile;
using PoemTown.Repository.Enums.SaleVersions;
using PoemTown.Repository.Enums.TargetMarks;
using PoemTown.Repository.Enums.Transactions;
using PoemTown.Repository.Enums.UsageRights;
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
using PoemTown.Service.Events.TransactionEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Service.Events.AnnouncementEvents;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using static Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels.RealtimeEventTypes;

namespace PoemTown.Service.Services
{
    public class RecordFileService : IRecordFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAwsS3Service _awsS3Service;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IHttpClientFactory _httpClientFactory;
        public RecordFileService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAwsS3Service awsS3Service,
            IPublishEndpoint publishEndpoint,
            IHttpClientFactory httpClientFactory

        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _awsS3Service = awsS3Service;
            _publishEndpoint = publishEndpoint;
            _httpClientFactory = httpClientFactory;
        }

        public async Task CreateNewRecord(Guid userId, Guid poemID, CreateNewRecordFileRequest request)
        {
            RecordFile recordFile = _mapper.Map<RecordFile>(request);
            Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemID);
            if (poem == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
            }

            //check user own poem create without sale version
            if (poem.UserId == userId)
            {
                recordFile.UserId = userId;
                recordFile.PoemId = poemID;
                recordFile.IsPublic = true;
                recordFile.Price = 0;
                await _unitOfWork.GetRepository<RecordFile>().InsertAsync(recordFile);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                SaleVersion? saleVersion = await _unitOfWork.GetRepository<SaleVersion>()
                    .FindAsync(s => s.IsInUse == true && s.PoemId == poemID);
                // check sale version to create record (free) 
                if (saleVersion.Status == SaleVersionStatus.Free)
                {
                    recordFile.UserId = userId;
                    recordFile.PoemId = poemID;
                    recordFile.IsPublic = true;
                    recordFile.SaleVersionId = saleVersion.Id;
                    recordFile.Price = 0;
                    await _unitOfWork.GetRepository<RecordFile>().InsertAsync(recordFile);
                    await _unitOfWork.SaveChangesAsync();
                }
                
                // check sale version to create record (private) 
                else
                {
                    var purchase = _unitOfWork.GetRepository<UsageRight>().AsQueryable();
                    var matchedUsageRight = purchase.FirstOrDefault(ur => ur.UserId == userId
                                                                          && ur.SaleVersionId == saleVersion.Id
                                                                          && ur.DeletedTime == null
                                                                          && ur.Status == UsageRightStatus.StillValid);
                    if (matchedUsageRight == null)
                    {
                        throw new CoreException(StatusCodes.Status400BadRequest,
                            "Poem has not purchased or not still valid");
                    }

                    recordFile.UserId = userId;
                    recordFile.PoemId = poemID;
                    recordFile.SaleVersionId = matchedUsageRight.SaleVersionId;
                    recordFile.IsPublic = true;
                    recordFile.Price = 0;
                    await _unitOfWork.GetRepository<RecordFile>().InsertAsync(recordFile);
                    await _unitOfWork.SaveChangesAsync();
                }
                
               
            }
            User user = await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == userId);
            
            if(user == null)
            {
                return;
            }
            
            // List of userId who targetMark this poem
            var userIds = _unitOfWork.GetRepository<TargetMark>()
                .AsQueryable()
                .Where(p => p.PoemId == poemID && p.MarkByUserId != null)
                .Select(b => b.MarkByUserId!.Value)
                .ToList();

            // Include poem owner
            userIds.Add(poem.UserId!.Value);

            /*// Announce to Poem Usage Right Holder new record file
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                UserId = poem.UserId,
                Title = $"Bản ghi âm mới từ bài thơ '{poem.Title}'",
                Content = $"Người dùng {recordFile.User!.UserName} đã tạo bản ghi âm mới từ bài thơ '{poem.Title}'",
                Type = AnnouncementType.RecordFile,
                PoemId = poem.Id,
                RecordFileId = recordFile.Id,
                IsRead = false
            });*/
            // Announce to Poem Usage Right Holder new record file
            await _publishEndpoint.Publish(new SendBulkUserAnnouncementEvent()
            {
                UserIds = userIds,
                Title = $"Bản ghi âm mới từ bài thơ '{poem.Title}'",
                Content = $"Người dùng {user.UserName} đã tạo bản ghi âm mới từ bài thơ '{poem.Title}'",
                Type = AnnouncementType.RecordFile,
                PoemId = poem.Id,
                RecordFileId = recordFile.Id,
                IsRead = false
            });
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

            var userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>()
                .FindAsync(r => r.RecordFileId == request.Id && r.Type == UserPoemType.RecordBuyer);

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

            var userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>()
                .FindAsync(r => r.RecordFileId == recordId && r.Type == UserPoemType.RecordBuyer);

            //Check if record file has been bought, cannot update
            if (userPoemRecordFile != null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file has been bought, cannot delete");
            }

            if (userId != recordFile.UserId)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "You not own this record file, cannot delete");
            }

            _unitOfWork.GetRepository<RecordFile>().Delete(recordFile);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task AssigntToPrivate(Guid userId, AssignPrivateRequest request)
        {
            RecordFile? recordFile =
                await _unitOfWork.GetRepository<RecordFile>().FindAsync(r => r.Id == request.RecordId);
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


            //Check if record file is not yours
            if (recordFile.UserId != userId)
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

            // If recordFile is yours 
            if (recordFile.UserId == userId)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "This record file is yours, cannot buy");
            }

            if (recordFile.PoemId == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "This Record file is not belong to any Poem");
            }
            
            // Find user by id
            UsageRight? userPoemRecordFile = await _unitOfWork.GetRepository<UsageRight>()
                .FindAsync(p => p.UserId == userId && p.RecordFileId == recordId);

            // If user already purchased this poem then throw exception
            if (userPoemRecordFile != null && userPoemRecordFile.Type == UserPoemType.RecordBuyer)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User already purchased this record file");
            }

           /* var utc7Now = DateTime.UtcNow.AddHours(7);
            var utc7Today = utc7Now.Date;
            if (userPoemRecordFile.CopyRightValidTo < utc7Today)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Bài thơ hết quyền sử dụng nên không mua được bản ghi âm này nữa");
            }*/

            UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>()
                .FindAsync(p => p.UserId == userId);

            UserEWallet? userEWalletPoemOwner = await _unitOfWork.GetRepository<UserEWallet>()
                .FindAsync(p => p.UserId == recordFile.Poem.UserId);

            UserEWallet? userEWalletRecordOwner = await _unitOfWork.GetRepository<UserEWallet>()
                .FindAsync(p => p.UserId == recordFile.UserId);
            // If user e-wallet not found then throw exception
            if (userEWallet == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet not found");
            }

            // If user e-wallet balance is not enough then throw exception
            if (userEWallet.WalletBalance < recordFile.Price)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet balance is not enough");
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

            if (recordFile.SaleVersion != null)
            {
                userPoemRecordFile.SaleVersionId = recordFile.SaleVersionId;
            }

            await _unitOfWork.GetRepository<UsageRight>().InsertAsync(userPoemRecordFile);
            await _unitOfWork.SaveChangesAsync();


            decimal amountOfRecordOwnerEarn;
                
            // Deduct user e-wallet balance
            if (recordFile.SaleVersion == null)
            {
                userEWallet.WalletBalance -= recordFile.Price;

                amountOfRecordOwnerEarn = recordFile.Price;
                userEWalletRecordOwner.WalletBalance += amountOfRecordOwnerEarn;
                _unitOfWork.GetRepository<UserEWallet>().Update(userEWalletPoemOwner);
                _unitOfWork.GetRepository<UserEWallet>().Update(userEWallet);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                // Calculate usage right commission price
                decimal usageRightCommissionPrice =
                    recordFile.Price * recordFile.SaleVersion.CommissionPercentage / 100;
                
                // Deduct user e-wallet balance
                userEWallet.WalletBalance -= recordFile.Price;
                
                // Add commission to Poem Usage Right Holder e-wallet balance
                userEWalletPoemOwner.WalletBalance += usageRightCommissionPrice;
                
                amountOfRecordOwnerEarn = recordFile.Price * (100 - recordFile.SaleVersion.CommissionPercentage) / 100;
                // Add money to Record file Owner e-wallet balance
                userEWalletRecordOwner.WalletBalance += amountOfRecordOwnerEarn; 
                
                CreateCommissionTransactionEvent createTransactionEvent = new CreateCommissionTransactionEvent()
                {
                    Amount = recordFile.Price * recordFile.SaleVersion.CommissionPercentage / 100,
                    Description = $"Tiền hoa hồng từ bản quyền từ bài thơ {recordFile.Poem.Title}",
                    Type = TransactionType.CommissionFee,
                    UserEWalletId = userEWalletPoemOwner.Id,
                    PoemId = recordFile.PoemId.Value
                };
                await _publishEndpoint.Publish(createTransactionEvent);

                _unitOfWork.GetRepository<UserEWallet>().Update(userEWalletPoemOwner);
                _unitOfWork.GetRepository<UserEWallet>().Update(userEWallet);
                await _unitOfWork.SaveChangesAsync();
            }


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

            /*// Send announcement to record file owner
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                UserId = recordFile.UserId,
                Title = $"Phí hoa hồng quyền sử dụng bài thơ: '{recordFile.Poem!.Title}'",
                Content = $"Đã nhận được  '{recordFile.FileName}' của bạn",
                Type = AnnouncementType.RecordFile,
                RecordFileId = recordFile.Id,
                IsRead = false
            });*/
            
            // Create transaction and announce to usage right holder
            await _publishEndpoint.Publish(new CreateTransactionEvent()
            {
                IsAddToWallet = true,
                Amount = amountOfRecordOwnerEarn,
                DiscountAmount = 0,
                AnnouncementTitle = "Tiền từ bán quyền sử dụng bài ngâm thơ",
                AnnouncementContent =
                    $"Bạn nhận được '{amountOfRecordOwnerEarn}VND' từ việc bán quyền sử dụng bài ngâm thơ '{recordFile.FileName}'",
                Type = TransactionType.RecordFiles,
                TransactionCode = OrderCodeGenerator.Generate(),
                Description = $"Tiền từ việc bán quyền sử dụng bài ngâm thơ '{recordFile.FileName}'",
                UserEWalletId = userEWalletRecordOwner.Id
            });
        }

        public async Task<PaginationResponse<GetSoldRecordResponse>>
            GetSoldRecord(Guid? userId,
                RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all records that user have
            var records = _unitOfWork.GetRepository<RecordFile>()
                .AsQueryable()
                .Where(p => p.UserId == userId && p.DeletedTime == null);

            //Get all usage record roi chuyen thanh list khong trung
            var usageRecordIds = _unitOfWork.GetRepository<UsageRight>()
                .AsQueryable()
                .Select(u => u.RecordFileId)
                .ToHashSet();

            //lay ra record cua minh co trong usage right
            var soldRecords = records
                .Where(r => usageRecordIds.Contains(r.Id));

            var queryPaging = await _unitOfWork.GetRepository<RecordFile>()
                .GetPagination(soldRecords, request.PageNumber, request.PageSize);


            //Get all records have been sold

            IList<GetSoldRecordResponse> listSoldRecords = new List<GetSoldRecordResponse>();

            foreach (var soldRecord in queryPaging.Data)
            {
                var recordEntity =
                    await _unitOfWork.GetRepository<RecordFile>().FindAsync(ur => ur.Id == soldRecord.Id);
                if (recordEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
                }

                listSoldRecords.Add(_mapper.Map<GetSoldRecordResponse>(recordEntity));
                listSoldRecords.Last().Buyers = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                    .Where(u => u.RecordFileId == soldRecord.Id && u.DeletedTime == null)
                    .Select(b => _mapper.Map<GetBasicUserInformationResponse>(b.User))
                    .ToList();
                listSoldRecords.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(soldRecord.User);
                listSoldRecords.Last().Poem = _mapper.Map<GetPoemDetailResponse>(soldRecord.Poem);
            }

            return new PaginationResponse<GetSoldRecordResponse>(listSoldRecords, queryPaging.PageNumber,
                queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }




        public async Task<GetRecordFileResponse>GetRecordDetail(Guid recordId)
        {
            //Get record detail that user have
            RecordFile? record = await _unitOfWork.GetRepository<RecordFile>().FindAsync(r => r.Id == recordId);
            if(record == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
            }

            var recordDetail = _mapper.Map<GetRecordFileResponse>(record);
            recordDetail.Poem = _mapper.Map< GetPoemDetailResponse>(record.Poem);
            recordDetail.Owner = _mapper.Map<GetBasicUserInformationResponse>(record.User);
            recordDetail.Buyers = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                    .Where(u => u.RecordFileId == recordId && u.DeletedTime == null)
                    .Select(b => _mapper.Map<GetBasicUserInformationResponse>(b.User))
                    .ToList();
            return recordDetail;
        }


        public async Task<PaginationResponse<GetBoughtRecordResponse>>
            GetBoughtRecord(Guid? userId,
                RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all usage right that user have
            var usageRight = _unitOfWork.GetRepository<UsageRight>()
                .AsQueryable()
                .Where(p => p.UserId == userId
                            && p.RecordFileId != null
                            && p.Type == UserPoemType.RecordBuyer
                            && p.DeletedTime == null);

            var recordFiles = usageRight
                .Select(ur => ur.RecordFile);

            var poem = _unitOfWork.GetRepository<Poem>()
                .AsQueryable()
                .FirstOrDefault(p => usageRight.Select(s => s.RecordFile.PoemId).Contains(p.Id));

            var queryPaging = await _unitOfWork.GetRepository<RecordFile>()
                .GetPagination(recordFiles, request.PageNumber, request.PageSize);

            IList<GetBoughtRecordResponse> boughtRecords = new List<GetBoughtRecordResponse>();

            foreach (var boughtRecord in queryPaging.Data)
            {
                var recordEntity =
                    await _unitOfWork.GetRepository<RecordFile>().FindAsync(ur => ur.Id == boughtRecord.Id);
                if (recordEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Record file not found");
                }

                boughtRecords.Add(_mapper.Map<GetBoughtRecordResponse>(recordEntity));
                boughtRecords.Last().Buyer = _mapper.Map<GetBasicUserInformationResponse>(
                    _unitOfWork.GetRepository<User>()
                        .AsQueryable()
                        .Where(u => u.Id == userId && u.DeletedTime == null)
                        .FirstOrDefault());
                boughtRecords.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(boughtRecord.User);
                boughtRecords.Last().Poem = _mapper.Map<GetPoemDetailResponse>(boughtRecord.Poem);
            }

            return new PaginationResponse<GetBoughtRecordResponse>(boughtRecords, queryPaging.PageNumber,
                queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<PaginationResponse<GetRecordFileResponse>>
            GetAllRecord(Guid? userId,
                RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            //Get all records
            var recordsQuery = _unitOfWork.GetRepository<RecordFile>()
                .AsQueryable()
                .Where(r => r.DeletedTime == null && r.IsPublic == true);

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
                var recordEntity = await _unitOfWork.GetRepository<RecordFile>().FindAsync(p => p.Id == record.Id);
                if (recordEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
                }

                records.Add(_mapper.Map<GetRecordFileResponse>(recordEntity));
                records.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(recordEntity.User);
                records.Last().Buyers = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                    .Where(u => u.RecordFileId == record.Id && u.DeletedTime == null)
                    .Select(b => _mapper.Map<GetBasicUserInformationResponse>(b.User))
                    .ToList();
            }

            return new PaginationResponse<GetRecordFileResponse>(records, queryPaging.PageNumber, queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<PaginationResponse<GetRecordFileResponse>>
            GetMyRecord(Guid? userId,
                RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var recordsQuery = _unitOfWork.GetRepository<RecordFile>()
                .AsQueryable()
                .Where(r => r.UserId == userId && r.DeletedTime == null);

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
                var recordEntity = await _unitOfWork.GetRepository<RecordFile>().FindAsync(p => p.Id == record.Id);
                if (recordEntity == null)
                {
                    throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
                }

                records.Add(_mapper.Map<GetRecordFileResponse>(recordEntity));
                records.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(recordEntity.User);
                records.Last().Buyers = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                    .Where(u => u.RecordFileId == record.Id && u.DeletedTime == null)
                    .Select(b => _mapper.Map<GetBasicUserInformationResponse>(b.User))
                    .ToList();
            }

            return new PaginationResponse<GetRecordFileResponse>(records, queryPaging.PageNumber, queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }

        public async Task<PaginationResponse<GetRecordFileResponse>>
            GetUserRecord(string username,
                RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var recordsQuery = _unitOfWork.GetRepository<RecordFile>()
                .AsQueryable()
                .Where(r => r.User.UserName == username && r.DeletedTime == null);
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
                records.Last().Owner = _mapper.Map<GetBasicUserInformationResponse>(record.User);
                records.Last().Buyers = _unitOfWork.GetRepository<UsageRight>().AsQueryable()
                    .Where(u => u.RecordFileId == record.Id && u.DeletedTime == null)
                    .Select(b => _mapper.Map<GetBasicUserInformationResponse>(b.User))
                    .ToList();
            }

            return new PaginationResponse<GetRecordFileResponse>(records, queryPaging.PageNumber, queryPaging.PageSize,
                queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
        }


        public async Task<string> UploadRecordFile(Guid userId, IFormFile file)
        {
            var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);

            if (user == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
            }

            // Validate audio file
            FileAudioHelper.ValidateAudio(file);

            // Upload audio file to AWS S3
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName =
                $"recordings/{StringHelper.CapitalizeString(userId.ToString())}-{DateTime.UtcNow.Ticks}";

            UploadFileToAwsS3Model s3Model = new UploadFileToAwsS3Model()
            {
                File = file,
                FolderName = fileName
            };

            return await _awsS3Service.UploadAudioToAwsS3Async(s3Model);
        }

        public async Task<IActionResult> GetAudioStreamResultAsync(Guid id)
        {
            var record = await _unitOfWork.GetRepository<RecordFile>().FindAsync(r => r.Id == id);
            if (record == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Record not found");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(record.FileUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Lỗi khi truy xuất file từ server lưu trữ.");

            }
            // Đọc stream từ response
            var stream = await response.Content.ReadAsStreamAsync();

            /*using (var fileStream = File.Create("test_audio.mp3"))
            {
                await stream.CopyToAsync(fileStream);
            }
*/
            // Lấy MIME type từ header, nếu không có dùng mặc định audio/mpeg
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "audio/mpeg";

            // enableRangeProcessing: true cho phép client yêu cầu một đoạn (seek) của file
            return new FileStreamResult(stream, contentType)
            {
                EnableRangeProcessing = true
            };
        }




    }
}