namespace Tests;

using AutoMapper;
using EsportsProfileWebApi.Web.Repository;
using EsportsProfileWebApi.Web.Orchestrators;
using NUnit.Framework;
using EsportsProfileWebApi.Web.Clients;
using NSubstitute;

[TestFixture]
public class ProfileOrchestratorTests
{
    private readonly IProfileRepository mockProfileRepository = Substitute.For<IProfileRepository>();
    private readonly IMapper mapper = Substitute.For<IMapper>();
    private readonly IStatsClient _statsClient = Substitute.For<IStatsClient>();

    private readonly ProfileOrchestrator _ProfileOrchestrator;

    public ProfileOrchestratorTests()
    {
        _ProfileOrchestrator = new ProfileOrchestrator(mockProfileRepository, mapper, _statsClient);
    }

    [Test]
    public async Task GetProfileByUsername()
    {
        //ARRANGE

        //ACT

        //ASSERT
        Assert.AreEqual(1,1);
    }

    public async Task GetProfileData()
    {
    }

    public async Task UpdateData()
    {
    }

    public async Task GetPaginatedUsersAsync()
    {
    }

    public async Task GetPeripheralsAsync()
    {
    }
    
    public async Task UpdateUserPeripherals()
    {
    }
}