namespace Therapy_Service.Therapy.Core.Interfaces
{
    public interface IExercise
    {

        /// <summary>
        /// Название упражнения
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Подробное описание упражнения
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Категория развития
        /// </summary>
        MentalCategory Category { get; set; }

        /// <summary>
        /// Уровень сложности упражнения
        /// </summary>
        Level Difficulty { get; set; }

        /// <summary>
        /// Инструкции по выполнению упражнения
        /// </summary>
        string[] Instructions { get; set; }

        /// <summary>
        /// Необходимые материалы или инструменты
        /// </summary>
        string[] RequiredMaterials { get; set; }

        /// <summary>
        /// Минимальный рекомендуемый возраст
        /// </summary>
        int MinimumRecommendedAge { get; set; }

        /// <summary>
        /// Научное обоснование упражнения
        /// </summary>
        string ScientificBasis { get; set; }

        /// <summary>
        /// Активно ли упражнение
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Категории развития
    /// </summary>
    public enum MentalCategory
    {
        Memory,
        Attention,
        ProblemSolving,
        Creativity,
        Logic,
        Language,
        EmotionalIntelligence,
        SpeedThinking
    }

    /// <summary>
    /// Уровень сложности
    /// </summary>
    public enum Level
    {
        Easy,
        Middle,
        Advanced,
    }
}
