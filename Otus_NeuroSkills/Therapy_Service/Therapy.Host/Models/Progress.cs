using Therapy_Service.Therapy.Core.Interfaces;
using Therapy_Service.Therapy.Host.Models.Base;

namespace Therapy_Service.Therapy.Host.Models
{
    public class Progress : BaseModel, IProgress
    {
        public Guid UserId { get; set; }
        public Guid ExerciseId { get; set; }
        public int AverageDurationMinutes { get; set; }
        public int PerceivedDifficulty { get; set; }
        public string[] CurrentBenefits { get; set; }
        public double EffectivenessRating { get; set; }

    }
}
