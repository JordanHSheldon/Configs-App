namespace EsportsProfileWebApi.Web.Repository.Entities;

public class ProfileEntity
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? SteamId { get; set;}

    public string? Avatar { get; set; }

    public int MouseId { get; set; }

    public int MousepadId { get; set; }

    public int KeyboardId { get; set; }
}