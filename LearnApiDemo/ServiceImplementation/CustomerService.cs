using LearnApiDemo.DTOs;
using LearnApiDemo.Helper;
using LearnApiDemo.Models;
using LearnApiDemo.Repositories;
using LearnApiDemo.Services;

namespace LearnApiDemo.ServiceImplementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<ApiResponse> CreateCustomer(CustomerDto data)
        {
            return await _customerRepo.CreateCustomer(data);
        }

        public async Task<List<CustomerDto>> GetAll()
        {
            return await _customerRepo.GetAll();
        }

        public async Task<CustomerDto> GetCustomerByCode(string code)
        {
            return await _customerRepo.GetCustomerByCode(code);
        }

        public async Task<ApiResponse> RemoveCustomer(string code)
        {
            return await _customerRepo.RemoveCustomer(code);
        }

        public async Task<ApiResponse> UpdateCustomer(CustomerDto data, string code)
        {
            return await _customerRepo.UpdateCustomer(data, code);
        }
    }
}
