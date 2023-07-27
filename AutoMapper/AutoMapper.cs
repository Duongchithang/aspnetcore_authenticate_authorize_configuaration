using AutoMapper;

using Microservice.Models;
using Microservice.Models.DataViewModel;

namespace Microservice.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}
