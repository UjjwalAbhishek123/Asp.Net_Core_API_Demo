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

        [NonAction]
        private string GetFilePath(string productCode)
        {
            return _environment.WebRootPath + "\\Upload\\product\\" + productCode;
        }
    }
}
