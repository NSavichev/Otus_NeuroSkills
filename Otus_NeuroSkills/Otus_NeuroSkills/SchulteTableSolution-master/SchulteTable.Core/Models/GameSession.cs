using System.ComponentModel;

namespace SchulteTable.Core.Models;

public class GameSession : INotifyPropertyChanged // Текущая игра
{
    private int[,] _grid = new int[0, 0];
    private int _currentNumber = 1;
    private int _totalNumbers;
    private bool _isGameActive;
    private DateTime _startTime;
    private int _errorCount;
    private TimeSpan _elapsedTime;

    public int[,] Grid
    {
        get => _grid;
        set
        {
            _grid = value;
            OnPropertyChanged();
        }
    }

    public int CurrentNumber
    {
        get => _currentNumber;
        set
        {
            _currentNumber = value;
            OnPropertyChanged();
        }
    }

    public int TotalNumbers
    {
        get => _totalNumbers;
        set
        {
            _totalNumbers = value;
            OnPropertyChanged();
        }
    }

    public bool IsGameActive
    {
        get => _isGameActive;
        set
        {
            _isGameActive = value;
            OnPropertyChanged();
        }
    }

    public DateTime StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            OnPropertyChanged();
        }
    }

    public int ErrorCount
    {
        get => _errorCount;
        set
        {
            _errorCount = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            _elapsedTime = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
