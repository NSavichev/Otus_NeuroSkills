using SchulteTable.Core.Enums;

namespace SchulteTable.Core.Services;


public interface IThemeService // интерфейс  управления темами
{
    ThemeMode CurrentTheme { get; }    
    Task SetThemeAsync(ThemeMode themeMode);
    Task InitializeAsync();
    event EventHandler<ThemeMode>? ThemeChanged;
}
