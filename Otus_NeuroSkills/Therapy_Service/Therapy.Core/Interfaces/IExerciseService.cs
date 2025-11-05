namespace Therapy_Service.Therapy.Core.Interfaces
{
    public interface IExerciseService
    {
        /// <summary>
        /// Получить общее описание упражнения
        /// </summary>
        /// <returns>Форматированное описание</returns>
        string GetFullDescription();

        /// <summary>
        /// Проверить валидность данных упражнения
        /// </summary>
        /// <returns>Результат валидации</returns>
        bool Validate();

        /// <summary>
        /// Получить рекомендации по выполнению
        /// </summary>
        /// <returns>Текст рекомендаций</returns>
        string GetRecommendations();
    }
}
