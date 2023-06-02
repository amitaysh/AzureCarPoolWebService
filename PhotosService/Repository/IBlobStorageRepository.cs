using PhotosService.DomainModel;

namespace PhotosService.Repository
{
    public interface IBlobStorageRepository
    {
        Task<string> UploadPhotoAsync(UploadRequest uploadRequest);
        Task<DownloadData> GetPhotoAsync(string name);
        Task DeletePhotoAsync(string name);
    }
}