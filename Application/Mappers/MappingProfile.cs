using AutoMapper;
using Application.DTOs.AccountDtos;
using Application.DTOs.BaseDtos;
using Domain.Entities.AccountEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;
using Application.DTOs.EmployeeDtos;
using Application.DTOs.StudentDtos;

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

            // UserAccount to AccountOnlyDto
            CreateMap<UserAccount, AccountOnlyDto>().ReverseMap();

            // UserAccount to AccountFullDto
            CreateMap<UserAccount, AccountFullDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserAccountRoles.Select(x => x.Role)))
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee));

            // Role to RoleDto and reverse
            CreateMap<Role, RoleDto>().ReverseMap();

            // AddressBase to AddressOnlyDto and reverse
            CreateMap<AddressBase, AddressOnlyDto>().IncludeAllDerived();
            CreateMap<AddressOnlyDto, StudentAddress>();
            CreateMap<AddressOnlyDto, EmployeeAddress>();

            // ConsentBase to ConsentDto and reverse
            CreateMap<ConsentBase, ConsentDto>().IncludeAllDerived();
            CreateMap<ConsentDto, StudentConsent>();
            CreateMap<ConsentDto, EmployeeConsent>();

            // EducationBase to EducationDto
            CreateMap<EducationBase, EducationDto>().IncludeAllDerived().ReverseMap();
        }
    }
}
