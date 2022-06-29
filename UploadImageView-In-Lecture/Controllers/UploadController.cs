using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UploadImageView_In_Lecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss_" + file.FileName);
            var path = Path.Combine("Upload", fileName);
            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetImage()
        {
            var fileName = Path.Combine("Upload", "20220629135336_캡처.PNG");
            var image = System.IO.File.OpenRead(fileName);
            return File(image, "image/png");
        }

        static readonly HttpClient _httpClient = new HttpClient();

        [HttpGet("image-proxy")]
        public async Task<IActionResult> GetImageFromWeb()
        {
            const string url = "";
            byte[] fileBytes = await _httpClient.GetByteArrayAsync(url);

            string fileToWriteTo = Path.GetTempFileName();
            await System.IO.File.WriteAllBytesAsync(fileToWriteTo, fileBytes);
            return File(System.IO.File.ReadAllBytes(fileToWriteTo), "image/jpg");
        }
    }
}
