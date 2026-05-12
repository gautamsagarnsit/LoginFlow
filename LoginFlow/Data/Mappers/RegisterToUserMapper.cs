using AutoMapper;
using LoginFlow.Common;
using LoginFlow.Controllers;
using LoginFlow.Data.Tables;
using Microsoft.AspNetCore.Identity;

namespace LoginFlow.Data.Mappers
{
    public class RegisterToUserMapper : Profile
    {
        public RegisterToUserMapper()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash,
                       opt => opt.MapFrom(src => HashPassword(src.Password)));

            CreateMap<User, UserResponseDTO>();
        }
        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

    }
}
