using Microsoft.Maui.Controls;
using SchulteTable.Core.Enums;
using SchulteTable.Core.Services;

namespace SchulteTable.Maui.Services;

// реализация сервиса управления темами 

public partial class ThemeService : IThemeService
{
    #region Поля и свойства

    private ThemeMode _currentTheme = ThemeMode.Auto;
    public ThemeMode CurrentTheme => _currentTheme;
    public event EventHandler<ThemeMode>? ThemeChanged;

    #endregion

    #region Публичные методы

    public async Task InitializeAsync()
    {
        try
        {
            // проверяем, что Application.Current доступен
            if (Application.Current == null)
            {
                System.Diagnostics.Debug.WriteLine("Application.Current недоступен при инициализации ThemeService");
                return;
            }

            // подписываемся на изменения системной темы
            Application.Current.RequestedThemeChanged += OnSystemThemeChanged;
            
            // применяем текущую тему
            await ApplyCurrentThemeAsync();
            
            System.Diagnostics.Debug.WriteLine($"ThemeService инициализирован с темой: {_currentTheme}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка инициализации ThemeService: {ex.Message}");
        }
    }

    public async Task SetThemeAsync(ThemeMode themeMode)
    {
        try
        {
            var previousTheme = _currentTheme;
            _currentTheme = themeMode;

            await ApplyCurrentThemeAsync();

            // уведомляем подписчиков об изменении темы
            ThemeChanged?.Invoke(this, themeMode);
            
            System.Diagnostics.Debug.WriteLine($"Тема изменена с {previousTheme} на {themeMode}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка установки темы {themeMode}: {ex.Message}");
        }
    }

    #endregion

    #region Приватные методы

    private async Task ApplyCurrentThemeAsync()
    {
        if (Application.Current == null)
        {
            System.Diagnostics.Debug.WriteLine("Application.Current недоступен для применения темы");
            return;
        }

        // преобразуем наш режим темы в системный
        Application.Current.UserAppTheme = _currentTheme switch
        {
            ThemeMode.Dark => AppTheme.Dark,
            ThemeMode.Light => AppTheme.Light,
            ThemeMode.Auto => AppTheme.Unspecified, // позволяет системе выбирать
            _ => AppTheme.Unspecified
        };

        await Task.CompletedTask;
    }

    private void OnSystemThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        try
        {
            // реагируем на изменение системной темы в автоматическом режиме
            if (_currentTheme == ThemeMode.Auto)
            {
                System.Diagnostics.Debug.WriteLine($"Системная тема изменена на: {e.RequestedTheme}");
                
                // уведомляем подписчиков об изменении. тема остается авто, но визуально меняется
                ThemeChanged?.Invoke(this, ThemeMode.Auto);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка обработки изменения системной темы: {ex.Message}");
        }
    }

    public AppTheme GetEffectiveTheme()
    {
        if (Application.Current == null)
            return AppTheme.Light; // безопасное значение по умолчанию

        return _currentTheme switch
        {
            ThemeMode.Dark => AppTheme.Dark,
            ThemeMode.Light => AppTheme.Light,
            ThemeMode.Auto => Application.Current.PlatformAppTheme,
            _ => AppTheme.Light
        };
    }

    #endregion
}
