namespace EsportsProfileWebApi.Web.Orchestrators.Models.Stat;

using System.Collections.Generic;
using System.Linq;

public class CS2Stats
{
    public int TotalKills { get; set; }
    public int TotalDeaths { get; set; }
    public int TotalTimePlayed { get; set; }
    public int TotalPlantedBombs { get; set; }
    public int TotalDefusedBombs { get; set; }
    public int TotalWins { get; set; }
    public int TotalDamageDone { get; set; }
    public int TotalMoneyEarned { get; set; }
    public int TotalRescuedHostages { get; set; }

    public int TotalKillsKnife { get; set; }
    public int TotalKillsHegrenade { get; set; }
    public int TotalKillsGlock { get; set; }
    public int TotalKillsDeagle { get; set; }
    public int TotalKillsElite { get; set; }
    public int TotalKillsFiveSeven { get; set; }
    public int TotalKillsXm1014 { get; set; }
    public int TotalKillsMac10 { get; set; }
    public int TotalKillsUmp45 { get; set; }
    public int TotalKillsP90 { get; set; }
    public int TotalKillsAwp { get; set; }
    public int TotalKillsAk47 { get; set; }
    public int TotalKillsAug { get; set; }
    public int TotalKillsFamas { get; set; }
    public int TotalKillsG3sg1 { get; set; }
    public int TotalKillsM249 { get; set; }

    public int TotalKillsHeadshot { get; set; }
    public int TotalDominations { get; set; }
    public int TotalRevenges { get; set; }

    public int TotalShotsHit { get; set; }
    public int TotalShotsFired { get; set; }
    public int TotalRoundsPlayed { get; set; }

    public int TotalMvps { get; set; }
    public int TotalMatchesWon { get; set; }
    public int TotalMatchesPlayed { get; set; }

    public int TotalContributionScore { get; set; }

    public int LastMatchKills { get; set; }
    public int LastMatchDeaths { get; set; }
    public int LastMatchMvps { get; set; }
    public int LastMatchDamage { get; set; }
    public int LastMatchRounds { get; set; }

    public double KD => TotalDeaths == 0 ? TotalKills : (double)TotalKills / TotalDeaths;

    public CS2Stats (dynamic response)
    {
        var statsDict = ((IEnumerable<dynamic>)response.playerstats.stats)
            .ToDictionary(x => (string)x.name, x => (int)x.value);

        int Get(string key) => statsDict.TryGetValue(key, out var val) ? val : 0;

        TotalKills = Get("total_kills");
        TotalDeaths = Get("total_deaths");
        TotalTimePlayed = Get("total_time_played");
        TotalPlantedBombs = Get("total_planted_bombs");
        TotalDefusedBombs = Get("total_defused_bombs");
        TotalWins = Get("total_wins");
        TotalDamageDone = Get("total_damage_done");
        TotalMoneyEarned = Get("total_money_earned");
        TotalRescuedHostages = Get("total_rescued_hostages");

        TotalKillsKnife = Get("total_kills_knife");
        TotalKillsHegrenade = Get("total_kills_hegrenade");
        TotalKillsGlock = Get("total_kills_glock");
        TotalKillsDeagle = Get("total_kills_deagle");
        TotalKillsElite = Get("total_kills_elite");
        TotalKillsFiveSeven = Get("total_kills_fiveseven");
        TotalKillsXm1014 = Get("total_kills_xm1014");
        TotalKillsMac10 = Get("total_kills_mac10");
        TotalKillsUmp45 = Get("total_kills_ump45");
        TotalKillsP90 = Get("total_kills_p90");
        TotalKillsAwp = Get("total_kills_awp");
        TotalKillsAk47 = Get("total_kills_ak47");
        TotalKillsAug = Get("total_kills_aug");
        TotalKillsFamas = Get("total_kills_famas");
        TotalKillsG3sg1 = Get("total_kills_g3sg1");
        TotalKillsM249 = Get("total_kills_m249");

        TotalKillsHeadshot = Get("total_kills_headshot");
        TotalDominations = Get("total_dominations");
        TotalRevenges = Get("total_revenges");

        TotalShotsHit = Get("total_shots_hit");
        TotalShotsFired = Get("total_shots_fired");
        TotalRoundsPlayed = Get("total_rounds_played");

        TotalMvps = Get("total_mvps");
        TotalMatchesWon = Get("total_matches_won");
        TotalMatchesPlayed = Get("total_matches_played");

        TotalContributionScore = Get("total_contribution_score");

        LastMatchKills = Get("last_match_kills");
        LastMatchDeaths = Get("last_match_deaths");
        LastMatchMvps = Get("last_match_mvps");
        LastMatchDamage = Get("last_match_damage");
        LastMatchRounds = Get("last_match_rounds");
    }
}