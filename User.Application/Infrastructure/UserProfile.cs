using AutoMapper;
using Shared.Identity;
using User.Application.Update;

namespace User.Application.Infrastructure
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // to map public and internal properties
            ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;

            CreateMap<UpdateUserCommand, ApplicationUser>();
        }
    }
}
