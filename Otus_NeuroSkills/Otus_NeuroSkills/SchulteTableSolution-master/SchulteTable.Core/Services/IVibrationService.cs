namespace SchulteTable.Core.Services;

public enum VibrationType // типы вибраций для различных событий
{
    Success, // короткая вибраци
    Error, // длтнная
    Click //короткий клик
}

public interface IVibrationService
{
    Task VibrateAsync(VibrationType type);
}
