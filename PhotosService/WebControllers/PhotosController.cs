using Microsoft.AspNetCore.Mvc;
using PhotosService.Services;

namespace PhotosService.WebControllers
{
    [ApiController]
    [Route("api/photos")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly List<string> _imageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("{name}")]
        public async Task<IActionResult> UploadPhoto(IFormFile photo, string name)
        {
            // verify the file is not empty and is in expected type (list of image files above)
            if (photo == null || photo.Length == 0 || !(_imageExtensions.Contains(Path.GetExtension(photo.FileName).ToUpperInvariant())))
            {
                return BadRequest("Invalid file");
            }

            try
            {
                var photoUrl = await _photoService.UploadPhotoAsync(photo, name);
                return Ok(photoUrl);
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading photo: {ex.Message}");
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> DownloadPhoto(string name)
        {
            try
            {
                var blobData = await _photoService.GetPhotoAsync(name);

                // Return the file response, convert the stream to file
                return File(blobData.DownloadInfo, blobData.FileType, blobData.FileName);
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error downloading photo: {ex.Message}");
            }
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeletePhoto(string name)
        {
            try
            {
                await _photoService.DeletePhotoAsync(name);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting photo: {ex.Message}");
            }
        }
    }
}
