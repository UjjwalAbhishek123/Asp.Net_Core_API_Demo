using LearnApiDemo.DTOs;
using LearnApiDemo.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnApiDemo.Controllers
{
    //[DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //injecting Service
        private readonly ICustomerService _customerService;
        
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

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
    }
}
