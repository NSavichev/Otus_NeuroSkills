namespace TherapyService.Therapy.Host.Dto
{
    public class ProgressDto
    {
        public int ID { get; set; }

        public Guid UserId { get; set; }

        public Guid ExerciseId { get; set; }

        public int AverageDurationMinutes { get; set; }

        public int PerceivedDifficulty { get; set; }

        public string[] CurrentBenefits { get; set; }
        public double EffectivenessRating { get; set; }
    }
}
