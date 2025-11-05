using Microsoft.EntityFrameworkCore;
using Therapy_Service.Therapy.Core.Repository.Abstraction;
using Therapy_Service.Therapy.DataAccess.Database;
using Therapy_Service.Therapy.Host.Dto;
using Therapy_Service.Therapy.Host.Models;
using Therapy_Service.Therapy.Host.Models.Base;
using TherapyService.Therapy.Host.Dto;

namespace Therapy_Service.Therapy.Core.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseModel
    {
        private ApplicationContext _context { get; set; }
        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Универсальный метод для записи новых значений в таблицы БД
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> CreateAsync(T model)
        {
            bool result = true;

            try
            {
                await _context.Set<T>().AddAsync(model);
                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Универсальный метод для обновления данных в таблице БД
        /// </summary>
        /// <param name="model"></param>
        /// /// <param name="id"></param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> UpdateAsync(T model, int id)
        {
            bool result = true;

            try
            {
                var existingEntity = await _context.Set<T>().FindAsync(id);
                if (existingEntity == null)
                    return false;

                _context.Entry(existingEntity).CurrentValues.SetValues(model);

                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Универсальный метод для получения всех значений из таблиц в БД
        /// </summary>
        /// <returns></returns>
        public async Task<IList<T>> GetValuesAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Метод для асинхронного получения всех необходимых значений по ID упражнения
        /// Возвращает список RequestDto
        /// </summary>
        /// <returns></returns>
        public async Task<ExerciseDto> GetExerciseAsync(int id)
        {
            string query = "select * from \"ExerciseTest\" where \"ID\" = " + id;

            var exerciseDtos = await _context.ExerciseDtos
                .FromSqlRaw(query)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return exerciseDtos;
        }

        /// <summary>
        /// Метод для асинхронного получения всех необходимых значений по ID прогресса
        /// Возвращает список RequestDto
        /// </summary>
        /// <returns></returns>
        public async Task<ProgressDto> GetProgressAsync(int id)
        {
            string query = "select * from \"ProgressTest\" where \"ID\" = " + id;

            var progressDtos = await _context.ProgressDtos
                .FromSqlRaw(query)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return progressDtos;
        }

        /// <summary>
        /// Удаление заявки по ID из БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> DeleteExerciseAsync(int id)
        {
            bool result = false;
            Exercise? request = await _context.Exercises.FindAsync(id);

            if (request == null)
            {
                return result;
            }

            try
            {
                _context.Exercises.Remove(request);
                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
