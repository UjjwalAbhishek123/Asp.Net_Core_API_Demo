using AutoMapper;
using LearnApiDemo.DTOs;
using LearnApiDemo.Models;

namespace LearnApiDemo.Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            //creating Map
            CreateMap<TblCustomer, CustomerDto>().ForMember(item => item.StatusName,
                option => option.MapFrom(item => (item.IsActive.HasValue && item.IsActive.Value) ? "Active" : "Inactive")).ReverseMap();

            //Note => using ReverseMap() to enable bi-Directional functionality
        }
    }
}
