using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchulteTable.Core.Models;
using SchulteTable.Core.Services;

namespace SchulteTable.Core.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IGameSessionService _gameSessionService;
    private readonly IStatisticsService _statisticsService;
    private readonly IThemeService _themeService;
    private readonly IVibrationService _vibrationService;
    private System.Timers.Timer? _gameTimer;

    [ObservableProperty]
    private GameSession _currentSession = new();

    [ObservableProperty]
    private bool _isGameActive;

    [ObservableProperty]
    private string _timerText = "00:00";

    [ObservableProperty]
    private int _currentNumber = 1;

    [ObservableProperty]
    private int _errorCount;

    [ObservableProperty]
    private string _statusMessage = "Нажмите 'Начать игру' для старта";

    public MainViewModel(
        IGameSessionService gameSessionService,
        IStatisticsService statisticsService,
        IThemeService themeService,
        IVibrationService vibrationService)
    {
        _gameSessionService = gameSessionService;
        _statisticsService = statisticsService;
        _themeService = themeService;
        _vibrationService = vibrationService;

        _gameSessionService.GameCompleted += OnGameCompleted;
    }

    [RelayCommand]
    private async Task StartGameAsync()
    {
        try
        {
            var gridSize = CurrentSession.TotalNumbers > 0 ? 
                (int)Math.Sqrt(CurrentSession.TotalNumbers) : 5;
            
            await _gameSessionService.StartNewGameAsync(gridSize);
            
            IsGameActive = true;
            CurrentNumber = 1;
            ErrorCount = 0;
            StatusMessage = $"Игра началась! Найдите число {CurrentNumber}";
            
            StartTimer();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ResetGameAsync()
    {
        await _gameSessionService.ResetGameAsync();
        
        IsGameActive = false;
        CurrentNumber = 1;
        ErrorCount = 0;
        TimerText = "00:00";
        StatusMessage = "Нажмите 'Начать игру' для старта";
        
        StopTimer();
    }

    [RelayCommand]
    private async Task SelectNumberAsync(int number)
    {
        if (!IsGameActive) return;

        var isCorrect = await _gameSessionService.TrySelectNumberAsync(number);
        
        if (isCorrect)
        {
            CurrentNumber = _gameSessionService.CurrentSession.CurrentNumber;
            
            if (CurrentNumber <= _gameSessionService.CurrentSession.TotalNumbers)
            {
                StatusMessage = $"Отлично! Теперь найдите число {CurrentNumber}";
            }
            
            await _vibrationService.VibrateAsync(VibrationType.Success);
            
            if (CurrentNumber > _gameSessionService.CurrentSession.TotalNumbers)
            {
                await CompleteGameAsync();
            }
        }
        else
        {
            ErrorCount = _gameSessionService.ErrorCount;
            StatusMessage = $"Попробуйте еще раз! Ищите число {CurrentNumber}";
            
            await _vibrationService.VibrateAsync(VibrationType.Error);
        }
    }

    private async Task CompleteGameAsync()
    {
        IsGameActive = false;
        StopTimer();
        
        var gameResult = new GameResult
        {
            GridSize = (int)Math.Sqrt(_gameSessionService.CurrentSession.TotalNumbers),
            TimeElapsedTicks = _gameSessionService.ElapsedTime.Ticks,
            DateCompleted = DateTime.Now,
            ErrorCount = ErrorCount,
            UserName = "Игрок"
        };

        await _statisticsService.SaveGameResultAsync(gameResult);
        
        var timeFormatted = FormatTime(_gameSessionService.ElapsedTime);
        StatusMessage = $"Поздравляем! Время: {timeFormatted}, ошибок: {ErrorCount}";
        
        await _vibrationService.VibrateAsync(VibrationType.Success);
    }

    private void OnGameCompleted(object? sender, GameResult gameResult)
    {
        // игра завершена
    }

    private void StartTimer()
    {
        StopTimer();
        
        _gameTimer = new System.Timers.Timer(100);
        _gameTimer.Elapsed += (s, e) =>
        {
            var elapsed = _gameSessionService.ElapsedTime;
            TimerText = FormatTime(elapsed);
        };
        _gameTimer.Start();
    }

    private void StopTimer()
    {
        _gameTimer?.Stop();
        _gameTimer?.Dispose();
        _gameTimer = null;
    }

    private static string FormatTime(TimeSpan time)
    {
        return $"{time.Minutes:D2}:{time.Seconds:D2}.{time.Milliseconds / 10:D2}";
    }
}
