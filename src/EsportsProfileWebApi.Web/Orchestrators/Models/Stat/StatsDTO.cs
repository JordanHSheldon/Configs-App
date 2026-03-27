namespace EsportsProfileWebApi.Web.Controllers.DTOs.Stats;

public class StatsDTO
{
    public Stat[] stats { get; set; }
}

public class Stat
{
    public string name { get; set; }
    public int value { get; set; }
}
