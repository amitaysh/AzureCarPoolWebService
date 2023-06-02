using PhotosService.DomainModel;
using PhotosService.Repository;
using System.IO;

namespace PhotosService.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IBlobStorageRepository _blobStorageRepository;

        public PhotoService(IBlobStorageRepository blobStorageRepository)
        {
            _blobStorageRepository = blobStorageRepository;
        }

        public async Task<string> UploadPhotoAsync(IFormFile photo, string name)
        {
            var uploadRequet = new UploadRequest
            {
                FileType = $"image/{Path.GetExtension(photo.FileName).Replace(".", "")}",
                FileName = photo.FileName,
                PhotoName = name,
                FileSteam = new MemoryStream()
            };
            await photo.CopyToAsync(uploadRequet.FileSteam);
            uploadRequet.FileSteam.Position = 0;

            var photoUrl = await _blobStorageRepository.UploadPhotoAsync(uploadRequet);
            return photoUrl;
        }

        public async Task<DownloadData> GetPhotoAsync(string name)
        {
            return await _blobStorageRepository.GetPhotoAsync(name);
        }

        public async Task DeletePhotoAsync(string name)
        {
            await _blobStorageRepository.DeletePhotoAsync(name);
        }
    }
}
