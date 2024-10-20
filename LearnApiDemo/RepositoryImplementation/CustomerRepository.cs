using AutoMapper;
using LearnApiDemo.Data;
using LearnApiDemo.DTOs;
using LearnApiDemo.Helper;
using LearnApiDemo.Models;
using LearnApiDemo.Repositories;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LearnApiDemo.RepositoryImplementation
{
    public class CustomerRepository : ICustomerRepository
    {
        //object to access Database
        private readonly LearnApiDbContext _dbContext;

        //using mapper
        private readonly IMapper _mapper;

        public CustomerRepository(LearnApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateCustomer(CustomerDto data)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                TblCustomer _customer = _mapper.Map<CustomerDto, TblCustomer>(data);

                await _dbContext.TblCustomers.AddAsync(_customer);
                await _dbContext.SaveChangesAsync();

                //if everything is right => positive scenario response code will be 201 Created
                response.ResponseCode = 201;
                response.Result = data.Code;

            }
            catch (Exception ex)
            {
                //if any error occur => negative scenario response code will be 400
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<List<CustomerDto>> GetAll()
        {
            List<CustomerDto> _customerResponse = new List<CustomerDto>();

            var data = await _dbContext.TblCustomers.ToListAsync();

            if(data != null)
            {
                //if fetched data is not null, then Map the TblCustomer model to CustomerDto
                _customerResponse = _mapper.Map<List<TblCustomer>, List<CustomerDto>>(data);
            }
            return _customerResponse;
        }

        public async Task<CustomerDto> GetCustomerByCode(string code)
        {
            CustomerDto _response = new CustomerDto();

            var data = await _dbContext.TblCustomers.FindAsync(code);

            if (data != null)
            {
                //if fetched data is not null, then Map the individual TblCustomer model object to CustomerDto
                _response = _mapper.Map<TblCustomer, CustomerDto>(data);
            }

            return _response;
        }

        public async Task<ApiResponse> RemoveCustomer(string code)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var _customer = await _dbContext.TblCustomers.FindAsync(code);

                if (_customer != null)
                {
                    _dbContext.TblCustomers.Remove(_customer);
                    await _dbContext.SaveChangesAsync();

                    //if everything is right => positive scenario response code will be 201 Created
                    response.ResponseCode = 200;
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                //if any error occur => negative scenario response code will be 400
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse> UpdateCustomer(CustomerDto data, string code)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var _customer = await _dbContext.TblCustomers.FindAsync(code);

                if (_customer != null)
                {
                    //update the data
                    _customer.Name = data.Name;
                    _customer.Email = data.Email;
                    _customer.Phone = data.Phone;
                    _customer.IsActive = data.IsActive;
                    _customer.CreditLimit = data.CreditLimit;

                    await _dbContext.SaveChangesAsync();

                    //if everything is right => positive scenario response code will be 201 Created
                    response.ResponseCode = 200;
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                //if any error occur => negative scenario response code will be 400
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
