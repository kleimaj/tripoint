﻿namespace TriPoint.Services; 

public interface IBufferedFileUploadService
{
    Task<bool> UploadFile(IFormFile file);
}