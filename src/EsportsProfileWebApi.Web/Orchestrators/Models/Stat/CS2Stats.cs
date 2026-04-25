namespace EsportsProfileWebApi.Web.Orchestrators.Models.Stat;

public class CS2Stats
{
    public PlayerStats playerstats { get; set; }
}

public class PlayerStats
{
    public string steamID { get; set; }
    public string gameName { get; set; }
    public Stat[] stats { get; set; }
    public Achievement[] achievements { get; set; }
}

public class Stat
{
    public string name { get; set; }
    public int value { get; set; }
}

public class Achievement
{
    public string name { get; set; }
    public int achieved { get; set; }
}