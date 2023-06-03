using Azure.Storage.Blobs;
using PhotosService.DomainModel;

namespace PhotosService.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        // blob repository ctor
        public BlobStorageRepository(string connectionString, string containerName)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = containerName;
        }

        // upload photo to blob
        public async Task<string> UploadPhotoAsync(UploadRequest uploadRequest)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(uploadRequest.PhotoName);

            // since we want to keep the name provided by user - save the file info as metadata
            var blobMetadata = new Dictionary<string, string>
            {
                { "FileName", uploadRequest.FileName },
                { "FileType", uploadRequest.FileType }
            };

            await blobClient.UploadAsync(uploadRequest.FileSteam);
            await blobClient.SetMetadataAsync(blobMetadata);

            // return direct url to the photo
            return blobClient.Uri.ToString();
        }

        // get photo from blob
        public async Task<DownloadData> GetPhotoAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);

            var response = await blobClient.DownloadAsync();

            var blobMetadata = (await blobClient.GetPropertiesAsync()).Value.Metadata;
    
            // populate data from blob metadata
            var photoData = new DownloadData
            {
                PhotoName = name,
                FileName = blobMetadata["FileName"],
                FileType = blobMetadata["FileType"],
                DownloadInfo = response.Value.Content
            };

            return photoData;
        }

        // delete photo from blob
        public async Task DeletePhotoAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(name);

            await blobClient.DeleteAsync();
        }
    }
}
