using AutoMapper;
using LoginFlow.Controllers;
using LoginFlow.Data.Tables;

namespace LoginFlow.Data.Mappers
{
    public class LoginToUserMapper : Profile
    {
        public LoginToUserMapper()
        {
            CreateMap<UserLoginDTO, User>()
                .ForMember(dest => dest.PasswordHash,
                      opt => opt.MapFrom(src => HashPassword(src.Password)));
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
