﻿using PoemTown.Repository.Interfaces;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ImageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    
}