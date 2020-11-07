using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
              .ForMember(c => c.FullAddress,
                 opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            
            CreateMap<Employee, EmployeeDto>();

            CreateMap<CreateCompanyDto, Company>();

            CreateMap<CreateEmployeeDto, Employee>();

            CreateMap<UpdateEmployeeDto, Employee>().ReverseMap();

            CreateMap<UpdateCompanyDto, Company>();

            CreateMap<UserRegisterationDto, ApplicationUser>();
        }
    }
}
