namespace PhotosService.DomainModel
{
    public class UploadRequest
    {
        public string PhotoName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public MemoryStream FileSteam { get; set; }
    }
}
