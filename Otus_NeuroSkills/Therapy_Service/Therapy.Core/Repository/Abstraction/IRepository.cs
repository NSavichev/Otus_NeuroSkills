using Therapy_Service.Therapy.Host.Dto;
using Therapy_Service.Therapy.Host.Models;
using Therapy_Service.Therapy.Host.Models.Base;
using TherapyService.Therapy.Host.Dto;

namespace Therapy_Service.Therapy.Core.Repository.Abstraction
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<bool> CreateAsync(T model);

        Task<bool> UpdateAsync(T model, int id);

        Task<IList<T>> GetValuesAsync();
        Task<ExerciseDto> GetExerciseAsync(int id);
        Task<ProgressDto> GetProgressAsync(int id);

        Task<bool> DeleteExerciseAsync(int id);
    }
}
