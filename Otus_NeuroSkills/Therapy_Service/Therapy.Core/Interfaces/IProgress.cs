namespace Therapy_Service.Therapy.Core.Interfaces
{
    public interface IProgress
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        Guid UserId { get; set; }

        /// <summary>
        /// Идентификатор упражнения
        /// </summary>
        Guid ExerciseId { get; set; }

        /// <summary>
        /// Средняя продолжительность выполнения (в минутах)
        /// </summary>
        int AverageDurationMinutes { get; set; }

        /// <summary>
        /// Оценка сложности выполнения (от 1 до 10)
        /// </summary>
        int PerceivedDifficulty { get; set; }

        /// <summary>
        /// Текуший прогресс от выполнения
        /// </summary>
        string[] CurrentBenefits { get; set; }

        /// <summary>
        /// Рейтинг эффективности (от 1 до 5)
        /// </summary>
        double EffectivenessRating { get; set; }
    }
}
