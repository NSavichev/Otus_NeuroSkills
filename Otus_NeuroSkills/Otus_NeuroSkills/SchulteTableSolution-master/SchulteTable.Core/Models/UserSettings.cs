using System.ComponentModel;
using SchulteTable.Core.Enums;

namespace SchulteTable.Core.Models;

public class UserSettings : INotifyPropertyChanged //настройки юзера
{
    private string _userName = "Игрок";
    private ThemeMode _preferredTheme = ThemeMode.Auto;
    private int _selectedGridSize = 5;

    public string UserName
    {
        get => _userName;
        set
        {
            // имя не пустое и не слишком длинное
            if (!string.IsNullOrWhiteSpace(value) && value.Length <= 50)
            {
                _userName = value.Trim();
                OnPropertyChanged();
            }
        }
    }

    public ThemeMode PreferredTheme //цветовая тема
    {
        get => _preferredTheme;
        set
        {
            _preferredTheme = value;
            OnPropertyChanged();
        }
    }

    public int SelectedGridSize
    {
        get => _selectedGridSize;
        set
        {
            // допустимый диапазон размеров сетки
            if (value >= 4 && value <= 8)
            {
                _selectedGridSize = value;
                OnPropertyChanged();
            }
        }
    }

    // событие изменения свойства для привязки данных
    public event PropertyChangedEventHandler? PropertyChanged;

    //автоматическое обновление интерфейса при изменении настроек
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
