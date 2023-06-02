using Azure.Storage.Blobs;
using PhotosService.DomainModel;

namespace PhotosService.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageRepository(string connectionString, string containerName)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = containerName;
        }

        public async Task<string> UploadPhotoAsync(UploadRequest uploadRequest)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(uploadRequest.PhotoName);
            var blobMetadata = new Dictionary<string, string>
            {
                { "FileName", uploadRequest.FileName },
                { "FileType", uploadRequest.FileType }
            };

            await blobClient.UploadAsync(uploadRequest.FileSteam);
            await blobClient.SetMetadataAsync(blobMetadata);

            return blobClient.Uri.ToString();
        }

        public async Task<DownloadData> GetPhotoAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);

            var response = await blobClient.DownloadAsync();

            var blobMetadata = (await blobClient.GetPropertiesAsync()).Value.Metadata;


            var photoData = new DownloadData
            {
                PhotoName = name,
                FileName = blobMetadata["FileName"],
                FileType = blobMetadata["FileType"],
                DownloadInfo = response.Value.Content
            };

            return photoData;
        }

        public async Task DeletePhotoAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);

            await blobClient.DeleteAsync();
        }
    }
}
