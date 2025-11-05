using SchulteTable.Core.Models;

namespace SchulteTable.Core.Services;

// Основной сервис игры
public class GameSessionService : IGameSessionService
{
    private readonly ITableGenerationService _tableGenerationService;
    private GameSession _currentSession;
    private System.Timers.Timer? _timer;
    private DateTime _startTime;

    public GameSessionService(ITableGenerationService tableGenerationService)
    {
        _tableGenerationService = tableGenerationService;
        _currentSession = new GameSession();
    }

    public GameSession CurrentSession => _currentSession;
    public bool IsGameActive => _currentSession.IsGameActive;
    public TimeSpan ElapsedTime => _currentSession.ElapsedTime;
    public int ErrorCount => _currentSession.ErrorCount;

    public event EventHandler<GameResult>? GameCompleted;

    public async Task StartNewGameAsync(int gridSize)
    {
        if (gridSize < 4 || gridSize > 8)
            throw new ArgumentException("Размер сетки должен быть от 4 до 8");

        StopTimer();

        _currentSession = new GameSession
        {
            Grid = _tableGenerationService.GenerateTable(gridSize),
            TotalNumbers = gridSize * gridSize,
            CurrentNumber = 1,
            IsGameActive = true,
            StartTime = DateTime.Now,
            ErrorCount = 0,
            ElapsedTime = TimeSpan.Zero
        };

        _startTime = DateTime.Now;
        StartTimer();

        await Task.CompletedTask;
    }

    public async Task<bool> TrySelectNumberAsync(int number)
    {
        if (!_currentSession.IsGameActive)
            return false;

        if (number == _currentSession.CurrentNumber)
        {
            _currentSession.CurrentNumber++;
            
            if (_currentSession.CurrentNumber > _currentSession.TotalNumbers)
            {
                await EndGameAsync();
            }
            
            return true;
        }
        else
        {
            _currentSession.ErrorCount++;
            return false;
        }
    }

    public async Task EndGameAsync()
    {
        if (!_currentSession.IsGameActive)
            return;

        _currentSession.IsGameActive = false;
        StopTimer();

        var gameResult = new GameResult
        {
            GridSize = (int)Math.Sqrt(_currentSession.TotalNumbers),
            TimeElapsedTicks = _currentSession.ElapsedTime.Ticks,
            DateCompleted = DateTime.Now,
            ErrorCount = _currentSession.ErrorCount,
            UserName = "Игрок"
        };

        GameCompleted?.Invoke(this, gameResult);
        await Task.CompletedTask;
    }

    public async Task ResetGameAsync()
    {
        StopTimer();
        _currentSession = new GameSession();
        await Task.CompletedTask;
    }

    private void StartTimer()
    {
        _timer = new System.Timers.Timer(100);
        _timer.Elapsed += (s, e) =>
        {
            _currentSession.ElapsedTime = DateTime.Now - _startTime;
        };
        _timer.Start();
    }

    private void StopTimer()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }
}
