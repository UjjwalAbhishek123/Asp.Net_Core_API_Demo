using ClosedXML.Excel;
using LearnApiDemo.DTOs;
using LearnApiDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Data;

namespace LearnApiDemo.Controllers
{
    [Authorize]
    //[DisableCors]
    //[EnableRateLimiting("fixedWindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //injecting Service
        private readonly ICustomerService _customerService;

        private readonly IWebHostEnvironment _environment;

        public CustomerController(ICustomerService customerService, IWebHostEnvironment environment)
        {
            _customerService = customerService;
            _environment = environment;
        }

        [AllowAnonymous]
        //[EnableCors("corspolicy1")]
        [HttpGet("GetAllCustomer")]
        public async Task<IActionResult> GetAllResult()
        {
            var data = await  _customerService.GetAll();

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [DisableRateLimiting]
        [HttpGet("GetByCode")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var data = await _customerService.GetCustomerByCode(code);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerDto _data)
        {
            var data = await _customerService.CreateCustomer(_data);

            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerDto _data, string code)
        {
            var data = await _customerService.UpdateCustomer(_data, code);

            return Ok(data);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Remove(string code)
        {
            var data = await _customerService.RemoveCustomer(code);

            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                string filePath = GetFilepath();

                string excelPath = filePath + "\\customerinfo.xlsx";

                //initiate DataTable
                DataTable dt = new DataTable();

                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("CreditLimit", typeof(int));

                var data = await _customerService.GetAll();

                if (data != null && data.Count > 0)
                {
                    data.ForEach(item =>
                    {
                        dt.Rows.Add(item.Code, item.Name, item.Email, item.Phone, item.CreditLimit);
                    });
                }

                //initiate Excel Workbook
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //add worksheet
                    wb.AddWorksheet(dt, "Customer Info");

                    //converting Excel workbook into memory stream
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        if (System.IO.File.Exists(excelPath))
                        {
                            System.IO.File.Delete(excelPath);
                        }
                        wb.SaveAs(excelPath);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
                    }
                }

            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        [NonAction]
        private string GetFilepath()
        {
            return _environment.WebRootPath + "\\Export";
        }
    }
}
