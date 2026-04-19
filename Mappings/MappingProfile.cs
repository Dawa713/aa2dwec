using AutoMapper;
using ConsolePhoneStore.Models;
using ConsolePhoneStore.DTOs;

namespace ConsolePhoneStore.Mappings
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
            CreateMap<CreateUpdateCustomerDTO, Customer>();

            // Mapeo de Phone a PhoneDTO
            CreateMap<Phone, PhoneDTO>();

            // Mapeo de CreateUpdatePhoneDTO a Phone
            CreateMap<CreateUpdatePhoneDTO, Phone>();
        }
    }
}
