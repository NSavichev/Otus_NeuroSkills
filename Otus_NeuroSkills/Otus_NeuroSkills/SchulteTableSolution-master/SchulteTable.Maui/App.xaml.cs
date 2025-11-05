using Microsoft.EntityFrameworkCore;
using SchulteTable.Data.Entities;
using SchulteTable.Core.Services;

namespace SchulteTable.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override async void OnStart()
	{
		base.OnStart();
		
		// Инициализация базы данных
		await InitializeDatabaseAsync();
		
		// Инициализация сервиса тем
		var themeService = Handler?.MauiContext?.Services?.GetService<IThemeService>();
		if (themeService != null)
		{
			await themeService.InitializeAsync();
		}
	}

	private async Task InitializeDatabaseAsync()
	{
		try
		{
			var dbContext = Handler?.MauiContext?.Services?.GetService<AppDbContext>();
			if (dbContext != null)
			{
				await dbContext.Database.EnsureCreatedAsync();
			}
		}
		catch (Exception ex)
		{
			// Логирование ошибки инициализации БД
			System.Diagnostics.Debug.WriteLine($"Ошибка инициализации БД: {ex.Message}");
		}
	}
}