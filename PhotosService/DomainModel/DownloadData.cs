namespace PhotosService.DomainModel
{
    public class DownloadData
    {
        public string PhotoName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Stream DownloadInfo { get; set; }
    }
}
