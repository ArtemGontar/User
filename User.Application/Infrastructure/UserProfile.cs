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

            CreateMap<UpdateUserCommand, ApplicationUser>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.NormalizedUserName, x => x.Ignore())
                .ForMember(x => x.NormalizedEmail, x => x.Ignore())
                .ForMember(x => x.EmailConfirmed, x => x.Ignore())
                .ForMember(x => x.PasswordHash, x => x.Ignore())
                .ForMember(x => x.SecurityStamp, x => x.Ignore())
                .ForMember(x => x.ConcurrencyStamp, x => x.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, x => x.Ignore())
                .ForMember(x => x.LockoutEnd, x => x.Ignore())
                .ForMember(x => x.LockoutEnabled, x => x.Ignore())
                .ForMember(x => x.AccessFailedCount, x => x.Ignore());
        }
    }
}
