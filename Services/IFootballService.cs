using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFootballService
{
    Task<IEnumerable<MatchDto>> GetTeamMatchesAsync(int teamId, DateTime from, DateTime to);
}
