namespace EsportsProfileWebApi.Web.Controllers.DTOs.Profile;

public class GetPaginatedUsersRequestDTO 
{
    public int Offset { get; set; } = 0;

    public int Limit { get; set; } = 20;

    public string Search { get; set; } = string.Empty;
}