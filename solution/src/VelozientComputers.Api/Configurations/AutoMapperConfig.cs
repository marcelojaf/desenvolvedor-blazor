using AutoMapper;
using VelozientComputers.Core.Entities;
using VelozientComputers.Core.Extensions;
using VelozientComputers.Shared.DTOs;

namespace VelozientComputers.Api.Configurations
{
    /// <summary>
    /// AutoMapper profile configuration for mapping between entities and DTOs
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperConfig"/> class
        /// </summary>
        public AutoMapperConfig()
        {
            #region Computer Mappings

            CreateMap<Computer, ComputerDTO>()
                .ForMember(
                    dest => dest.Manufacturer,
                    opt => opt.MapFrom(src => src.Manufacturer.Name))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.StatusAssignments
                        .OrderByDescending(sa => sa.AssignDate)
                        .FirstOrDefault().Status.LocalizedName.ToComputerStatusEnum()))
                .ForMember(
                    dest => dest.CurrentAssignment,
                    opt => opt.MapFrom(src => src.UserAssignments
                        .FirstOrDefault(ua => !ua.AssignEndDate.HasValue)))
                .ForMember(
                    dest => dest.WarrantyExpirationDate,
                    opt => opt.MapFrom(src => src.WarrantyExpirationDate));

            CreateMap<CreateComputerDTO, Computer>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.CreateDate,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.ComputerManufacturerId,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Manufacturer,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.StatusAssignments,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.UserAssignments,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.WarrantyExpirationDate,
                    opt => opt.MapFrom(src => src.WarrantyExpiryDate));

            CreateMap<UpdateComputerDTO, Computer>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.CreateDate,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.StatusAssignments,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.UserAssignments,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Manufacturer,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.WarrantyExpirationDate,
                    opt => opt.MapFrom(src => src.WarrantyExpiryDate))
                .ForMember(
                    dest => dest.SerialNumber,
                    opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(
                    dest => dest.PurchaseDate,
                    opt => opt.MapFrom(src => src.PurchaseDate))
                .ForMember(
                    dest => dest.Specifications,
                    opt => opt.MapFrom(src => src.Specifications))
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(
                    dest => dest.ComputerManufacturerId,
                    opt => opt.MapFrom((src, dest, _, context) =>
                    {
                        var manufacturer = context.Items["Manufacturer"] as ComputerManufacturer;
                        return manufacturer?.Id ?? dest.ComputerManufacturerId;
                    }));

            #endregion

            #region User Mappings

            CreateMap<User, UserDTO>()
                .ForMember(
                    dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(
                    dest => dest.ComputerAssignments,
                    opt => opt.MapFrom(src => src.ComputerAssignments
                        .OrderByDescending(ca => ca.AssignStartDate)));

            CreateMap<CreateUserDTO, User>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.CreateDate,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(
                    dest => dest.ComputerAssignments,
                    opt => opt.Ignore());

            CreateMap<UpdateUserDTO, User>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.CreateDate,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(
                    dest => dest.ComputerAssignments,
                    opt => opt.Ignore());

            #endregion

            #region Assignment Mappings

            CreateMap<ComputerUserAssignment, UserAssignmentDTO>()
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId))
                .ForMember(
                    dest => dest.UserFullName,
                    opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(
                    dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src.User.EmailAddress))
                .ForMember(
                    dest => dest.AssignStartDate,
                    opt => opt.MapFrom(src => src.AssignStartDate))
                .ForMember(
                    dest => dest.AssignEndDate,
                    opt => opt.MapFrom(src => src.AssignEndDate));

            CreateMap<ComputerUserAssignment, ComputerAssignmentDTO>()
                .ForMember(
                    dest => dest.ComputerId,
                    opt => opt.MapFrom(src => src.ComputerId))
                .ForMember(
                    dest => dest.Manufacturer,
                    opt => opt.MapFrom(src => src.Computer.Manufacturer.Name))
                .ForMember(
                    dest => dest.SerialNumber,
                    opt => opt.MapFrom(src => src.Computer.SerialNumber))
                .ForMember(
                    dest => dest.AssignStartDate,
                    opt => opt.MapFrom(src => src.AssignStartDate))
                .ForMember(
                    dest => dest.AssignEndDate,
                    opt => opt.MapFrom(src => src.AssignEndDate));

            #endregion
        }
    }
}