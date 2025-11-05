using System.ComponentModel.DataAnnotations;
using Therapy_Service.Therapy.Core.Interfaces;
using Therapy_Service.Therapy.Host.Models.Base;

namespace Therapy_Service.Therapy.Host.Models
{
    public class Exercise : BaseModel, IExercise
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public MentalCategory Category { get; set; }
        public Level Difficulty { get; set; }
        public string[] Instructions { get; set; }
        public string[] RequiredMaterials { get; set; }
        public int MinimumRecommendedAge { get; set; }
        [StringLength(250)]
        public string ScientificBasis { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
