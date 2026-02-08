namespace EsportsProfileWebApi.Web.Controllers.DTOs.Data;

public class GetDataResponseDTO
{
    public string? Id { get; set; }

    public string? Username { get; set; }

    public string? Avatar { get;set; }

    public int MouseId { get; set; }

    public int MousepadId { get; set; }

    public int KeyboardId { get; set; }
}