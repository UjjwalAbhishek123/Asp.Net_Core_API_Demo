using LearnApiDemo.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace LearnApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //To use wwwroot folder, create IWebHostEnvironment field
        private readonly IWebHostEnvironment _environment;

        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile, string productCode)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                //access the path
                string filePath = GetFilePath(productCode);

                //check if we already have the folder, else create folder
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //product files to store at particular path
                string imagePath = filePath + "\\" + productCode + ".png";

                //check for existing file
                if (System.IO.File.Exists(imagePath))
                {
                    //deleting old files
                    System.IO.File.Delete(imagePath);
                }

                //creating new Files
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
            }
            catch(Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return Ok(response);
        }

        //to upload multiple images
        [HttpPut("MultiUploadImage")]
        public async Task<IActionResult> MultiUploadImage(IFormFileCollection formFileCollection, string productCode)
        {
            ApiResponse response = new ApiResponse();
            int passCount = 0, errorCount = 0;

            try
            {
                //access the path
                string filePath = GetFilePath(productCode);

                //check if we already have the folder, else create folder
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                foreach(var file in formFileCollection)
                {
                    //product files to store at particular path
                    string imagePath = filePath + "\\" + file.FileName;

                    //check for existing file
                    if (System.IO.File.Exists(imagePath))
                    {
                        //deleting old files
                        System.IO.File.Delete(imagePath);
                    }

                    //creating new Files
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);

                        passCount++;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                response.ErrorMessage = ex.Message;
            }

            response.ResponseCode = 200;
            response.Result = passCount + " Files uploaded & " + errorCount + " files failed";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string productCode)
        {
            string imageUrl = string.Empty;

            //getting Host url, for eg. LocalHost or any other hosting env, so we have to get that dynamically
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            try
            {
                string filePath = GetFilePath(productCode);

                string imagePath = filePath + "\\" + productCode + ".png";

                if (System.IO.File.Exists(imagePath))
                {
                    //forming url
                    imageUrl = hostUrl + "/Upload/product/" + productCode + "/" + productCode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {

            }

            return Ok(imageUrl);
        }

        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string productCode)
        {
            List<string> imageUrl = new List<string>();

            //getting Host url, for eg. LocalHost or any other hosting env, so we have to get that dynamically
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            try
            {
                string filePath = GetFilePath(productCode);

                // Check if the directory exists
                if (!System.IO.Directory.Exists(filePath))
                {
                    return NotFound(); // Return NotFound if directory does not exist
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                FileInfo[] fileInfos = directoryInfo.GetFiles();

                if (fileInfos.Length == 0)
                {
                    return NotFound(); // Return NotFound if no files exist
                }

                foreach (FileInfo fileInfo in fileInfos)
                {
                    string fileName = fileInfo.Name;
                    string imagePath = Path.Combine(filePath, fileName); // Use Path.Combine for path formation

                    if (System.IO.File.Exists(imagePath))
                    {
                        string _imageUrl = hostUrl + "/Upload/product/" + productCode + "/" + fileName;
                        imageUrl.Add(_imageUrl);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Ok(imageUrl);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download(string productCode)
        {
            //string imageUrl = string.Empty;

            //getting Host url, for eg. LocalHost or any other hosting env, so we have to get that dynamically
            //string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            try
            {
                string filePath = GetFilePath(productCode);

                string imagePath = filePath + "\\" + productCode + ".png";

                if (System.IO.File.Exists(imagePath))
                {
                    //creating Memory Stream
                    MemoryStream stream = new MemoryStream();

                    using (FileStream fileStream = new FileStream(imagePath, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(stream);
                    }

                    stream.Position = 0;

                    return File(stream, "image/png", productCode + ".png");
                    //Imageurl = hosturl + "/Upload/product/" + productcode + "/" + productcode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("remove")]
        public async Task<IActionResult> Remove(string productCode)
        {
            //string imageUrl = string.Empty;

            //getting Host url, for eg. LocalHost or any other hosting env, so we have to get that dynamically
            //string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            try
            {
                string filePath = GetFilePath(productCode);

                string imagePath = filePath + "\\" + productCode + ".png";

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    return Ok("image deleted");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("multiRemove")]
        public async Task<IActionResult> MultiRemove(string productCode)
        {
            //string imageUrl = string.Empty;

            //getting Host url, for eg. LocalHost or any other hosting env, so we have to get that dynamically
            //string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            try
            {
                string filePath = GetFilePath(productCode);

                if (System.IO.Directory.Exists(filePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [NonAction]
        private string GetFilePath(string productCode)
        {
            return _environment.WebRootPath + "\\Upload\\product\\" + productCode;
        }
    }
}
