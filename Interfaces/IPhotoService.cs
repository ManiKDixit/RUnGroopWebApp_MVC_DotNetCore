using CloudinaryDotNet.Actions;

namespace RUnGroopWebApp.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult>  DeletePhotoAsync(string publicId);
    }
}
