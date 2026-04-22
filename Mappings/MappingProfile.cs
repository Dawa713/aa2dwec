using AutoMapper;
using api_clase.Models;
using api_clase.DTOs;

namespace api_clase.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper que configura los mapeos entre Modelos y DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo de Customer a CustomerDTO (modelos de dominio a DTOs para respuestas)
            CreateMap<Customer, CustomerDTO>();

            // Mapeo de CreateUpdateCustomerDTO a Customer (DTOs de entrada a modelos de dominio)
            CreateMap<CreateUpdateCustomerDTO, Customer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                // No mapear CreatedAt ni IsActive - se asignan en el constructor o en Add()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // Mapeo de Phone a PhoneDTO
            CreateMap<Phone, PhoneDTO>();

            // Mapeo de CreateUpdatePhoneDTO a Phone
            CreateMap<CreateUpdatePhoneDTO, Phone>()
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}
