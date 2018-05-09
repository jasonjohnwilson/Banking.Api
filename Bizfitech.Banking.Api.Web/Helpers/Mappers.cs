using AutoMapper;
using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Web.Models;

namespace Bizfitech.Banking.Api.Web.Helpers
{
    public static class Mappers
    {
        public static void Initialise()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserPostDto, UserCreateDto>();

                config.CreateMap<UserCreateDto, UserGetDto>();
            });
        }
    }
}
