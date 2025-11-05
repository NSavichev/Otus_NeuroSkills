using Microsoft.Maui.Storage;
using System.Text.Json;
using SchulteTable.Core.Models;

namespace SchulteTable.Maui.Services;

// настройки
public class SettingsService
{
    private const string SettingsKey = "UserSettings";
    private UserSettings? _cachedSettings;

    public async Task<UserSettings> GetSettingsAsync()
    {
        if (_cachedSettings != null)
            return _cachedSettings;

        try
        {
            var json = await SecureStorage.GetAsync(SettingsKey);
            if (!string.IsNullOrEmpty(json))
            {
                _cachedSettings = JsonSerializer.Deserialize<UserSettings>(json);
            }
        }
        catch
        {
            // игнорим ошибки десериализации
        }

        _cachedSettings ??= new UserSettings();
        return _cachedSettings;
    }

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings);
            await SecureStorage.SetAsync(SettingsKey, json);
            _cachedSettings = settings;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сохранения настроек: {ex.Message}");
        }
    }

    public async Task ResetSettingsAsync()
    {
        try
        {
            SecureStorage.Remove(SettingsKey);
            _cachedSettings = new UserSettings();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сброса настроек: {ex.Message}");
        }
    }
}
