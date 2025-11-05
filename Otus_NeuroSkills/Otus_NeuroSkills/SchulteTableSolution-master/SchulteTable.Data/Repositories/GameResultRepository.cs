using Microsoft.EntityFrameworkCore;
using SchulteTable.Core.Models;
using SchulteTable.Data.Entities;

namespace SchulteTable.Data.Repositories;

public class GameResultRepository : IGameResultRepository
{
    private readonly AppDbContext _context;

    public GameResultRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GameResultEntity> AddAsync(GameResult gameResult, int userProfileId)
    {
        var entity = new GameResultEntity
        {
            GridSize = gameResult.GridSize,
            TimeElapsedTicks = gameResult.TimeElapsedTicks,
            DateCompleted = gameResult.DateCompleted,
            ErrorCount = gameResult.ErrorCount,
            UserProfileId = userProfileId
        };

        _context.GameResults.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<GameResultEntity>> GetByUserAsync(string userName, int? gridSize = null)
    {
        var query = _context.GameResults
            .Include(r => r.UserProfile)
            .Where(r => r.UserProfile.UserName == userName);

        if (gridSize.HasValue)
        {
            query = query.Where(r => r.GridSize == gridSize.Value);
        }

        return await query
            .OrderByDescending(r => r.DateCompleted)
            .ToListAsync();
    }

    public async Task<List<GameResultEntity>> GetBestResultsAsync(int gridSize, int count = 10)
    {
        return await _context.GameResults
            .Include(r => r.UserProfile)
            .Where(r => r.GridSize == gridSize)
            .OrderBy(r => r.TimeElapsedTicks)
            .Take(count)
            .ToListAsync();
    }

    public async Task<TimeSpan> GetAverageTimeAsync(string userName, int gridSize)
    {
        var averageTicks = await _context.GameResults
            .Include(r => r.UserProfile)
            .Where(r => r.UserProfile.UserName == userName && r.GridSize == gridSize)
            .AverageAsync(r => (double?)r.TimeElapsedTicks);

        return averageTicks.HasValue ? TimeSpan.FromTicks((long)averageTicks.Value) : TimeSpan.Zero;
    }

    public async Task<int> GetTotalGamesPlayedAsync(string userName)
    {
        return await _context.GameResults
            .Include(r => r.UserProfile)
            .CountAsync(r => r.UserProfile.UserName == userName);
    }
}
