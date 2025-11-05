namespace Therapy_Service.Therapy.Core.Interfaces
{
    public interface IProgressService
    {
        /// <summary>
        /// Проверить, является ли выполнение успешным
        /// </summary>
        /// <returns>Результат проверки</returns>
        bool IsSuccessfulAttempt();

        /// <summary>
        /// Обновить рейтинг эффективности
        /// </summary>
        /// <param name="newRating">Новая оценка</param>
        void UpdateEffectivenessRating(double newRating);

        /// <summary>
        /// Рассчитать времени освоения (в днях)
        /// </summary>
        /// <returns>Количество дней</returns>
        int CalculateMasteryDays();
    }
}
