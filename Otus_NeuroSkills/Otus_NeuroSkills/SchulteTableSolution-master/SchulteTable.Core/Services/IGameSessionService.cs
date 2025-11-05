using SchulteTable.Core.Models;

namespace SchulteTable.Core.Services;

// Сервис для управления игрой
public interface IGameSessionService
{
    GameSession CurrentSession { get; }
    bool IsGameActive { get; }
    TimeSpan ElapsedTime { get; }
    int ErrorCount { get; }
    
    Task StartNewGameAsync(int gridSize);
    Task<bool> TrySelectNumberAsync(int number);
    Task EndGameAsync();
    Task ResetGameAsync();
    
    event EventHandler<GameResult>? GameCompleted;
}
