using SchulteTable.Core.Models;

namespace SchulteTable.Core.Services;

public interface IStatisticsService // Интерфейс статистики 
{
    Task SaveGameResultAsync(GameResult gameResult); 
    Task<List<GameResult>> GetUserStatisticsAsync(string userName, int? gridSize = null);
    Task<List<GameResult>> GetBestResultsAsync(int gridSize, int count = 10);
    Task<TimeSpan> GetAverageTimeAsync(string userName, int gridSize);
    Task<int> GetTotalGamesPlayedAsync(string userName);
}
