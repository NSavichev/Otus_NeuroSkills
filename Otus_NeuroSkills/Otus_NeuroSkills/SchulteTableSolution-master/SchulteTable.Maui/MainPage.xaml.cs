using SchulteTable.Core.ViewModels;
using SchulteTable.Core.Services;
using SchulteTable.Core.Models;
using SchulteTable.Maui.Services;

namespace SchulteTable.Maui;

public partial class MainPage : ContentPage
{
    private MainViewModel? _viewModel;
    private IGameSessionService? _gameSessionService;
    private IVibrationService? _vibrationService;
    private SettingsService? _settingsService;
    private System.Timers.Timer? _gameTimer;
    private bool _isGameActive = false;
    private int _currentNumber = 1;
    private int _errorCount = 0;
    private DateTime _startTime;
    private int _gridSize = 5; // Размер сетки по умолчанию

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        LoadServices();
    }

    private void LoadServices()
    {
        _viewModel = Handler?.MauiContext?.Services?.GetService<MainViewModel>();
        _gameSessionService = Handler?.MauiContext?.Services?.GetService<IGameSessionService>();
        _vibrationService = Handler?.MauiContext?.Services?.GetService<IVibrationService>();
        _settingsService = new SettingsService();
        
        // Загружаем настройки
        _ = LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        if (_settingsService == null) return;

        try
        {
            var settings = await _settingsService.GetSettingsAsync();
            _gridSize = settings.SelectedGridSize;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
        }
    }

    private async void OnStartGameClicked(object? sender, EventArgs e)
    {
        // Попробуем загрузить сервисы еще раз
        LoadServices();
        
        if (_gameSessionService == null)
        {
            StatusLabel.Text = "Ошибка: GameSessionService не найден";
            return;
        }
        
        if (_vibrationService == null)
        {
            StatusLabel.Text = "Ошибка: VibrationService не найден";
            return;
        }

        try
        {
            StatusLabel.Text = "Запуск игры...";
            
            // Загружаем настройки перед запуском
            await LoadSettingsAsync();
            
            // Создаем сетку с размером из настроек
            _isGameActive = true;
            _currentNumber = 1;
            _errorCount = 0;
            _startTime = DateTime.Now;
            
            StatusLabel.Text = $"Игра началась! Нажмите на числа по порядку (сетка {_gridSize}x{_gridSize})";
            CurrentNumberLabel.Text = $"Текущее число: {_currentNumber}";
            ErrorCountLabel.Text = $"Ошибки: {_errorCount}";
            
            CreateGameGrid(_gridSize);
            StartTimer();
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Ошибка при запуске игры: {ex.Message}";
        }
    }

    private async void OnResetGameClicked(object? sender, EventArgs e)
    {
        if (_gameSessionService == null) return;

        await _gameSessionService.ResetGameAsync();
        _isGameActive = false;
        _currentNumber = 1;
        _errorCount = 0;
        
        StatusLabel.Text = "Готов к игре";
        TimerLabel.Text = "00:00";
        CurrentNumberLabel.Text = $"Текущее число: {_currentNumber}";
        ErrorCountLabel.Text = $"Ошибки: {_errorCount}";
        
        GameGrid.Clear();
        StopTimer();
    }

    private void CreateGameGrid(int size)
    {
        GameGrid.Clear();
        GameGrid.RowDefinitions.Clear();
        GameGrid.ColumnDefinitions.Clear();

        // Вычисляем размер кнопки (максимум 60x60, минимум 40x40)
        var maxButtonSize = 60.0;
        var minButtonSize = 40.0;
        var buttonSize = Math.Max(minButtonSize, Math.Min(maxButtonSize, 600.0 / size));

        // Создаем строки и столбцы с фиксированным размером
        for (int i = 0; i < size; i++)
        {
            GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(buttonSize, GridUnitType.Absolute) });
            GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(buttonSize, GridUnitType.Absolute) });
        }

        // Создаем кнопки для каждой ячейки
        var numbers = Enumerable.Range(1, size * size).OrderBy(x => Random.Shared.Next()).ToArray();
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var number = numbers[i * size + j];
                var button = new Button
                {
                    Text = number.ToString(),
                    FontSize = Math.Max(12, buttonSize / 4), // Адаптивный размер шрифта
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black,
                    CornerRadius = 8,
                    BorderWidth = 1,
                    BorderColor = Colors.Gray,
                    WidthRequest = buttonSize,
                    HeightRequest = buttonSize
                };
                
                // Добавляем анимацию при наведении
                button.Pressed += (s, e) => AnimateButton(button, true);
                button.Released += (s, e) => AnimateButton(button, false);
                
                button.Clicked += (s, e) => OnCellClicked(number);
                
                GameGrid.Add(button, j, i);
            }
        }
    }

    private async void OnCellClicked(int number)
    {
        if (!_isGameActive) return;

        try
        {
            // Простая логика проверки без сервиса
            var isCorrect = (number == _currentNumber);
            
            // Находим кнопку с этим числом для анимации
            var button = FindButtonByNumber(number);
            
            if (isCorrect)
            {
                _currentNumber++;
                CurrentNumberLabel.Text = $"Текущее число: {_currentNumber}";
                StatusLabel.Text = $"Правильно! Следующее число: {_currentNumber}";
                
                // Попробуем вибрацию, если сервис доступен
                if (_vibrationService != null)
                {
                    await _vibrationService.VibrateAsync(VibrationType.Success);
                }
                
                if (button != null)
                {
                    AnimateCorrectButton(button);
                }
                
                if (_currentNumber > _gridSize * _gridSize)
                {
                    await CompleteGame();
                }
            }
            else
            {
                _errorCount++;
                ErrorCountLabel.Text = $"Ошибки: {_errorCount}";
                StatusLabel.Text = $"Неверно! Попробуйте снова. Текущее число: {_currentNumber}";
                
                // Попробуем вибрацию, если сервис доступен
                if (_vibrationService != null)
                {
                    await _vibrationService.VibrateAsync(VibrationType.Error);
                }
                
                if (button != null)
                {
                    AnimateIncorrectButton(button);
                }
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Ошибка: {ex.Message}";
        }
    }

    private Button? FindButtonByNumber(int number)
    {
        foreach (var child in GameGrid.Children)
        {
            if (child is Button button && button.Text == number.ToString())
            {
                return button;
            }
        }
        return null;
    }

    private async Task CompleteGame()
    {
        _isGameActive = false;
        StopTimer();
        
        var elapsed = DateTime.Now - _startTime;
        var timeText = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 10:D2}";
        
        StatusLabel.Text = $"Игра завершена! Время: {timeText}, Ошибок: {_errorCount}";
        TimerLabel.Text = timeText;
        
        // Сохраняем результат игры
        await SaveGameResult(elapsed);
        
        if (_vibrationService != null)
        {
            await _vibrationService.VibrateAsync(VibrationType.Success);
        }
    }

    private async Task SaveGameResult(TimeSpan elapsed)
    {
        try
        {
            var statisticsService = Handler?.MauiContext?.Services?.GetService<IStatisticsService>();
            if (statisticsService != null)
            {
                var gameResult = new GameResult
                {
                    GridSize = _gridSize,
                    TimeElapsedTicks = elapsed.Ticks,
                    DateCompleted = DateTime.Now,
                    ErrorCount = _errorCount,
                    UserName = "Player" // TODO: Получить из настроек
                };

                await statisticsService.SaveGameResultAsync(gameResult);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сохранения результата: {ex.Message}");
        }
    }

    private void StartTimer()
    {
        _gameTimer = new System.Timers.Timer(100);
        _gameTimer.Elapsed += (s, e) =>
        {
            var elapsed = DateTime.Now - _startTime;
            var timeText = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 10:D2}";
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TimerLabel.Text = timeText;
            });
        };
        _gameTimer.Start();
    }

    private void StopTimer()
    {
        _gameTimer?.Stop();
        _gameTimer?.Dispose();
        _gameTimer = null;
    }

    private async void OnStatisticsClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.StatisticsPage());
    }

    private async void OnSettingsClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.SettingsPage());
    }

    private void AnimateButton(Button button, bool isPressed)
    {
        if (isPressed)
        {
            button.ScaleTo(0.95, 100);
            button.BackgroundColor = Colors.LightGray;
        }
        else
        {
            button.ScaleTo(1.0, 100);
            button.BackgroundColor = Colors.White;
        }
    }

    private void AnimateCorrectButton(Button button)
    {
        // Анимация для правильного ответа
        button.ScaleTo(0.9, 100);
        button.BackgroundColor = Colors.LightGreen;
        button.ScaleTo(1.0, 100);
    }

    private void AnimateIncorrectButton(Button button)
    {
        // Анимация для неправильного ответа
        button.ScaleTo(0.9, 100);
        button.BackgroundColor = Colors.LightPink;
        
        // Анимация тряски
        var originalX = button.TranslationX;
        button.TranslateTo(-5, 0, 50);
        button.TranslateTo(5, 0, 50);
        button.TranslateTo(-5, 0, 50);
        button.TranslateTo(5, 0, 50);
        button.TranslateTo(originalX, 0, 50);
        
        button.BackgroundColor = Colors.White;
    }
}
