using LearnApiDemo.DTOs;
using LearnApiDemo.Helper;
using LearnApiDemo.Models;

namespace LearnApiDemo.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<CustomerDto>> GetAll();

        //to return single object
        Task<CustomerDto> GetCustomerByCode(string code);
        Task<ApiResponse> RemoveCustomer(string code);
        Task<ApiResponse> CreateCustomer(CustomerDto data);
        Task<ApiResponse> UpdateCustomer(CustomerDto data, string code);
    }
}
