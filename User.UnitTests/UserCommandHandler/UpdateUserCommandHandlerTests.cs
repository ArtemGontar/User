using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Shared.Common;
using Shared.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using User.Application.Infrastructure;
using User.Application.Update;
using Xunit;

namespace User.UnitTests.UserCommandHandler
{
    public class UpdateUserCommandHandlerTests
    {
        private AutoMocker _autoMocker;
        private UpdateUserCommandHandler _userCommandHandler;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        public UpdateUserCommandHandlerTests()
        {
            _autoMocker = new AutoMocker();

            var loggerMock = new Mock<ILogger<UpdateUserCommandHandler>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _autoMocker.Use<IMapper>(new MapperConfiguration(x => x.AddMaps(typeof(UserProfile).Assembly)).CreateMapper());
            _autoMocker.Use<UserManager<ApplicationUser>>(new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null,
                null, null, null, null));

            _userCommandHandler = _autoMocker.CreateInstance<UpdateUserCommandHandler>();

            _userManagerMock = _autoMocker.GetMock<UserManager<ApplicationUser>>();
        }

        [Fact]
        public async Task Handler_ValidUserData_ShouldSuccess()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand() 
            {
                UserId = userId,
                FirstName = "anonymousFirstName",
                LastName = "anonymousLastName",
                EnglishLevel = EnglishLevel.Beginner,
                BirthDate = DateTime.UtcNow,
                Departament = "anonymousDepartament",
                JobTitle = "anonymousJobTitle",
                PhoneNumber = "anonymousPhoneNumber"
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser() 
                {
                    Id = Guid.NewGuid(),
                    FirstName = "anonymousFirstName",
                    LastName = "anonymousLastName",
                    EnglishLevel = EnglishLevel.Beginner,
                    BirthDate = DateTime.UtcNow,
                    Departament = "anonymousDepartament",
                    JobTitle = "anonymousJobTitle",
                    PhoneNumber = "anonymousPhoneNumber"
                });
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userCommandHandler.Handle(command, CancellationToken.None);
            
            //Assert
            Assert.Equal(userId, result);
        }
    }
}
