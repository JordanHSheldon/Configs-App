namespace Tests;

using AutoMapper;
using EsportsProfileWebApi.Web.Controllers;
using EsportsProfileWebApi.Web.Controllers.DTOs.Profile;
using EsportsProfileWebApi.Web.Orchestrators;
using EsportsProfileWebApi.Web.Orchestrators.Models.Profile;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

public class ProfileControllerTests
{
    private readonly IProfileOrchestrator ProfileOrchestrator = Substitute.For<IProfileOrchestrator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ILogger<ProfileController> logger = Substitute.For<ILogger<ProfileController>>();
    private readonly ProfileController _ProfileController;

    public ProfileControllerTests()
    {
        _ProfileController = new ProfileController(ProfileOrchestrator, logger, _mapper);
    }

    [Test]
    public async Task GetPaginatedUsersAsync_ValidRequest_ReturnsGetProfileResponse()
    {
        // Arrange
        var response = new List<GetPaginatedUsersResponseModel>()
        {
            new ()
            {
                Id = "1",
                Username = "username",
                Avatar = "avatar"
            }
        };

        var orchReq = new GetPaginatedUsersRequestModel();
        var request = new GetPaginatedUsersRequestDTO();
        ProfileOrchestrator.GetPaginatedUsersAsync(orchReq).Returns(response);

        // Act
        var result = await _ProfileController.GetPaginatedProfiles(request);

        // Assert
        Assert.IsNotNull(result);
        await ProfileOrchestrator.Received().GetPaginatedUsersAsync(orchReq);
    }

    [Test]
    public void GetProfileByUserName_ReturnsGetProfileResponse()
    {
        // // Arrange
        // var request = new GetProfileByNameRequestDTO();
        // ProfileOrchestrator.Do(test => test.GetProfile(It.IsAny<GetProfileRequestModel>())).ReturnsAsync(new GetProfileResponseModel());

        // // Act
        // var result = _ProfileController.GetProfileByUsername(request);

        // // Assert
        // Assert.IsNotNull(result);
        // ProfileOrchestrator.Verify(verify => verify.GetProfile(It.IsAny<GetProfileRequestModel>()), Times.Once);
    }
}