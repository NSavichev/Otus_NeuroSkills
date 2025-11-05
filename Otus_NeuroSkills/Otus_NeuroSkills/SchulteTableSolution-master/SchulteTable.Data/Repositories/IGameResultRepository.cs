using SchulteTable.Core.Models;
using SchulteTable.Data.Entities;

namespace SchulteTable.Data.Repositories;

public interface IGameResultRepository
{
    Task<GameResultEntity> AddAsync(GameResult gameResult, int userProfileId);
    Task<List<GameResultEntity>> GetByUserAsync(string userName, int? gridSize = null);
    Task<List<GameResultEntity>> GetBestResultsAsync(int gridSize, int count = 10);
    Task<TimeSpan> GetAverageTimeAsync(string userName, int gridSize);
    Task<int> GetTotalGamesPlayedAsync(string userName);
}
