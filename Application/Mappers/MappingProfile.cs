using Application.DTOs.Account.Dtos;
using Application.DTOs.AccountDtos;
using Application.DTOs.BaseDtos;
using Application.DTOs.Employee.Dtos;
using Application.DTOs.Student.Dtos;
using AutoMapper;
using Domain.Entities.AccountEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;


namespace Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Base Person to PersonOnlyDto
            CreateMap<PersonBase, PersonOnlyDto>().IncludeAllDerived();

            // Specific mappings for derived types
            CreateMap<Student, StudentOnlyDto>();
            CreateMap<Employee, EmployeeOnlyDto>();

            // Full DTO mappings for Employee and Student
            CreateMap<Employee, EmployeeFullDto>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Consent, opt => opt.MapFrom(src => src.Consent));

            CreateMap<Student, StudentFullDto>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account));
            // Add other properties as needed

            // UserAccount to AccountOnlyDto
            CreateMap<UserAccount, AccountOnlyDto>().ReverseMap();

            // UserAccount to AccountFullDto
            CreateMap<UserAccount, AccountFullDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserAccountRoles.Select(x => x.Role)))
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
                .ReverseMap();

            // UserAccount to AccountCreateDto
            CreateMap<UserAccount, AccountCreateDto>().ReverseMap();

            // Role to RoleDto and reverse
            CreateMap<Role, RoleDto>().ReverseMap();

            // AddressBase to AddressOnlyDto and reverse
            CreateMap<AddressBase, AddressOnlyDto>().IncludeAllDerived();
            CreateMap<StudentAddress, AddressOnlyDto>().ReverseMap();
            CreateMap<EmployeeAddress, AddressOnlyDto>().ReverseMap();

            // ConsentBase to ConsentDto and reverse
            CreateMap<ConsentBase, ConsentDto>().IncludeAllDerived();
            CreateMap<StudentConsent, ConsentDto>().ReverseMap();
            CreateMap<EmployeeConsent, ConsentDto>().ReverseMap();

            // EducationBase to EducationDto
            CreateMap<EducationBase, EducationDto>().IncludeAllDerived().ReverseMap();
        }
    }
}
