using SchulteTable.Core.Services;
using SchulteTable.Core.Models;

namespace SchulteTable.Maui.Pages;

public partial class StatisticsPage : ContentPage
{
    private IStatisticsService? _statisticsService;
    private List<GameResult> _statistics = new();

    public StatisticsPage()
    {
        InitializeComponent();
        LoadServices();
    }

    private void LoadServices()
    {
        _statisticsService = Handler?.MauiContext?.Services?.GetService<IStatisticsService>();
    }

    private async void OnLoadStatisticsClicked(object? sender, EventArgs e)
    {
        if (_statisticsService == null) return;

        try
        {
            StatusLabel.Text = "Загрузка статистики...";
            
            var userName = UserNameEntry.Text;
            var selectedSize = GridSizePicker.SelectedIndex;
            int? gridSize = selectedSize > 0 ? selectedSize + 3 : null; // 4, 5, 6, 7, 8
            
            _statistics = await _statisticsService.GetUserStatisticsAsync(userName, gridSize);
            StatisticsCollectionView.ItemsSource = _statistics;
            
            StatusLabel.Text = $"Загружено {_statistics.Count} записей";
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Ошибка загрузки: {ex.Message}";
        }
    }

    private async void OnGoHomeClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
