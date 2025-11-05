using SchulteTable.Core.Services;
using SchulteTable.Core.Enums;
using SchulteTable.Core.Models;
using SchulteTable.Maui.Services;

namespace SchulteTable.Maui.Pages;

public partial class SettingsPage : ContentPage
{
    private IThemeService? _themeService;
    private SettingsService? _settingsService;

    public SettingsPage()
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
        _themeService = Handler?.MauiContext?.Services?.GetService<IThemeService>();
        _settingsService = new SettingsService();
        
        // Загружаем настройки после инициализации сервисов
        _ = LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        if (_settingsService == null) return;

        try
        {
            var settings = await _settingsService.GetSettingsAsync();
            
            // Обновляем UI в главном потоке
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UserNameEntry.Text = settings.UserName;
                GridSizePicker.SelectedIndex = settings.SelectedGridSize - 4; // 4,5,6,7,8 -> 0,1,2,3,4
                ThemePicker.SelectedIndex = (int)settings.PreferredTheme;
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
        }
    }

    private async void OnSaveSettingsClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_settingsService == null)
            {
                ShowStatus("Ошибка: Сервис настроек не найден", false);
                return;
            }

            // Получаем значения из UI
            var userName = UserNameEntry.Text;
            var gridSize = GridSizePicker.SelectedIndex + 4; // 4, 5, 6, 7, 8
            var theme = (ThemeMode)ThemePicker.SelectedIndex;
            
            // Создаем объект настроек
            var settings = new UserSettings
            {
                UserName = userName,
                SelectedGridSize = gridSize,
                PreferredTheme = theme
            };

            // Сохраняем настройки
            await _settingsService.SaveSettingsAsync(settings);
            
            // Применяем тему
            if (_themeService != null)
            {
                await _themeService.SetThemeAsync(theme);
            }
            
            ShowStatus("Настройки сохранены!", true);
        }
        catch (Exception ex)
        {
            ShowStatus($"Ошибка сохранения: {ex.Message}", false);
        }
    }

    private async void OnResetSettingsClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_settingsService != null)
            {
                await _settingsService.ResetSettingsAsync();
            }
            
            UserNameEntry.Text = "Player";
            GridSizePicker.SelectedIndex = 1; // 5x5
            ThemePicker.SelectedIndex = 0; // Auto
            
            if (_themeService != null)
            {
                await _themeService.SetThemeAsync(ThemeMode.Auto);
            }
            
            ShowStatus("Настройки сброшены!", true);
        }
        catch (Exception ex)
        {
            ShowStatus($"Ошибка сброса: {ex.Message}", false);
        }
    }

    private async void OnGoHomeClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void ShowStatus(string message, bool isSuccess)
    {
        StatusLabel.Text = message;
        StatusLabel.TextColor = isSuccess ? Colors.Green : Colors.Red;
        StatusLabel.IsVisible = true;
        
        // Скрыть сообщение через 3 секунды
        Dispatcher.StartTimer(TimeSpan.FromSeconds(3), () =>
        {
            StatusLabel.IsVisible = false;
            return false;
        });
    }
}
