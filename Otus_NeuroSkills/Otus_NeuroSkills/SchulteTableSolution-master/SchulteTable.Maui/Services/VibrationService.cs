using Microsoft.Maui.Devices;
using SchulteTable.Core.Services;

namespace SchulteTable.Maui.Services;

// сервис для вибрации
public class VibrationService : IVibrationService
{
    public async Task VibrateAsync(VibrationType type)
    {
        try
        {
            var duration = type switch
            {
                VibrationType.Success => TimeSpan.FromMilliseconds(100),
                VibrationType.Error => TimeSpan.FromMilliseconds(300),
                VibrationType.Click => TimeSpan.FromMilliseconds(50),
                _ => TimeSpan.FromMilliseconds(100)
            };

            if (HapticFeedback.Default.IsSupported)
            {
                var hapticType = type switch
                {
                    VibrationType.Success => HapticFeedbackType.LongPress,
                    VibrationType.Error => HapticFeedbackType.LongPress,
                    VibrationType.Click => HapticFeedbackType.Click,
                    _ => HapticFeedbackType.Click
                };

                HapticFeedback.Default.Perform(hapticType);
            }
            else
            {
                // для старых устройств
                Vibration.Default.Vibrate(duration);
            }
        }
        catch
        {
            // игнорим ошибки вибрации
        }

        await Task.CompletedTask;
    }
}
