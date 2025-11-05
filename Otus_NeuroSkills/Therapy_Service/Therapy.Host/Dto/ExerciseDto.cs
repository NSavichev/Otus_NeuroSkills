using Therapy_Service.Therapy.Core.Interfaces;

namespace Therapy_Service.Therapy.Host.Dto
{
    public class ExerciseDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MentalCategory Category { get; set; }

        public Level Difficulty { get; set; }

        public string[] Instructions { get; set; }

        public string[] RequiredMaterials { get; set; }

        public int MinimumRecommendedAge { get; set; }

        public string ScientificBasis { get; set; }

        public bool IsActive { get; set; }
    }
}
