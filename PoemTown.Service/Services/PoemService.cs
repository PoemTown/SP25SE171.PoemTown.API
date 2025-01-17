using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class PoemService : IPoemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PoemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateNewPoem(Guid userId, CreateNewPoemRequest request) 
    {
        // Mapping request to entity
        Poem poem = _mapper.Map<CreateNewPoemRequest, Poem>(request);

        // Check if source copy right exist
        if (request.SourceCopyRight != null)
        {
            User? sourceCopyRight = await _unitOfWork.GetRepository<User>()
                .FindAsync(p => p.Id == request.SourceCopyRight);
            if(sourceCopyRight == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Source copy right not found");
            }
        }

        Collection? collection = null;
        
        // If collectionId is not null then check if the collection exist
        if (request.CollectionId != null)
        {
            collection = await _unitOfWork.GetRepository<Collection>()
                .FindAsync(p => p.Id == request.CollectionId);
            if(collection == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Collection not found");
            }
        }
        
        // Check if user has any collection, if not then create default collection, finally assign that poem to default collection
        bool existCollections = await _unitOfWork.GetRepository<Collection>()
            .AsQueryable()
            .AnyAsync(p => p.UserId == userId);
        if (!existCollections)
        {
            collection = new Collection
            {
                UserId = userId,
                CollectionName = "Bộ sưu tập mặc định",
                CollectionDescription = "Bộ sưu tập được khởi tạo mặc định bởi hệ thống",
                IsDefault = true
            };
            await _unitOfWork.GetRepository<Collection>().InsertAsync(collection);
        }
        
        poem.Collection = collection;
        poem.UserId = userId;
        // Add poem to database
        await _unitOfWork.GetRepository<Poem>().InsertAsync(poem);
        
        // Save changes
        await _unitOfWork.SaveChangesAsync();
    }
}