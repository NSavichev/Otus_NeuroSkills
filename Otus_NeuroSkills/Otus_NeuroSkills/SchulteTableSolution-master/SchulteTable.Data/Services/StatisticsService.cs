using SchulteTable.Core.Models;
using SchulteTable.Core.Services;
using SchulteTable.Data.Repositories;
using SchulteTable.Data.Entities;

namespace SchulteTable.Data.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IGameResultRepository _gameResultRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public StatisticsService(
        IGameResultRepository gameResultRepository,
        IUserProfileRepository userProfileRepository)
    {
        _gameResultRepository = gameResultRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task SaveGameResultAsync(GameResult gameResult)
    {
        var userProfile = await _userProfileRepository.GetOrCreateAsync(gameResult.UserName);
        await _gameResultRepository.AddAsync(gameResult, userProfile.Id);
    }

    public async Task<List<GameResult>> GetUserStatisticsAsync(string userName, int? gridSize = null)
    {
        var entities = await _gameResultRepository.GetByUserAsync(userName, gridSize);
        return entities.Select(MapToGameResult).ToList();
    }

    public async Task<List<GameResult>> GetBestResultsAsync(int gridSize, int count = 10)
    {
        var entities = await _gameResultRepository.GetBestResultsAsync(gridSize, count);
        return entities.Select(MapToGameResult).ToList();
    }

    public async Task<TimeSpan> GetAverageTimeAsync(string userName, int gridSize)
    {
        return await _gameResultRepository.GetAverageTimeAsync(userName, gridSize);
    }

    public async Task<int> GetTotalGamesPlayedAsync(string userName)
    {
        return await _gameResultRepository.GetTotalGamesPlayedAsync(userName);
    }

    private static GameResult MapToGameResult(GameResultEntity entity)
    {
        return new GameResult
        {
            Id = entity.Id,
            GridSize = entity.GridSize,
            TimeElapsedTicks = entity.TimeElapsedTicks,
            DateCompleted = entity.DateCompleted,
            ErrorCount = entity.ErrorCount,
            UserName = entity.UserProfile.UserName
        };
    }
}
