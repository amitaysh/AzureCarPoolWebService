using PhotosService.DomainModel;

namespace PhotosService.Services
{
    public interface IPhotoService
    {
        Task<string> UploadPhotoAsync(IFormFile photo, string name);
        Task<DownloadData> GetPhotoAsync(string name);
        Task DeletePhotoAsync(string name);
    }
}
