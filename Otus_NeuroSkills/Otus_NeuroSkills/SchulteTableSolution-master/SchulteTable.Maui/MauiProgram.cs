using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SchulteTable.Core.Services;
using SchulteTable.Core.ViewModels;
using SchulteTable.Data.Entities;
using SchulteTable.Data.Repositories;
using SchulteTable.Data.Services;
using SchulteTable.Maui.Services;

namespace SchulteTable.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // сервисы
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={Path.Combine(FileSystem.AppDataDirectory, "schultetable.db")}"));

        // репы
        builder.Services.AddScoped<IGameResultRepository, GameResultRepository>();
        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

        // сервисы
        builder.Services.AddScoped<ITableGenerationService, TableGenerationService>();
        builder.Services.AddScoped<IGameSessionService, GameSessionService>();
        builder.Services.AddScoped<IStatisticsService, Data.Services.StatisticsService>();
        builder.Services.AddScoped<IThemeService, ThemeService>();
        builder.Services.AddScoped<IVibrationService, VibrationService>();

        builder.Services.AddTransient<MainViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
