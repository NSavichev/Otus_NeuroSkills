using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Therapy_Service.Therapy.Core.Repository;
using Therapy_Service.Therapy.Host.Dto;
using Therapy_Service.Therapy.Host.Models;

namespace TherapyService.Therapy.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExerciseController : Controller
    {
        private BaseRepository<Exercise> _exercise { get; set; }

        public ExerciseController(BaseRepository<Exercise> exercise)
        {
            _exercise = exercise;
        }

        /// <summary>
        /// Создание нового упражнения
        /// </summary>
        /// <returns></returns>
        // POST: api/Exercise/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync(JsonElement collection)
        {
            Exercise excis = new Exercise();

            var deserializedDto = JsonSerializer.Deserialize<ExerciseDto>(collection);
            if (deserializedDto == null)
                return BadRequest("Failed Deserialize");

            excis.Name = deserializedDto.Name;
            excis.Description = deserializedDto.Description;
            excis.Category = deserializedDto.Category;
            excis.Difficulty = deserializedDto.Difficulty;
            excis.Instructions = deserializedDto.Instructions;
            excis.RequiredMaterials = deserializedDto.RequiredMaterials;
            excis.MinimumRecommendedAge = deserializedDto.MinimumRecommendedAge;
            excis.ScientificBasis = deserializedDto.ScientificBasis;
            excis.IsActive = deserializedDto.IsActive;
            excis.CreatedAt = DateTime.UtcNow;
            excis.UpdatedAt = DateTime.UtcNow;

            return await _exercise.CreateAsync(excis) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Изменение данных в упражнение
        /// </summary>
        /// <returns></returns>
        // PUT: api/Exercise/Edit/5
        [HttpPut]
        public async Task<ActionResult> EditAsync(JsonElement collection, int id)
        {
            Exercise excis = new Exercise();

            var deserializedDto = JsonSerializer.Deserialize<ExerciseDto>(collection);
            if (deserializedDto == null)
                return NoContent();

            if (id < 1)
                return ValidationProblem("Переданный ИД не валидный");

            excis.ID = id;
            excis.Name = deserializedDto.Name;
            excis.Description = deserializedDto.Description;
            excis.Category = deserializedDto.Category;
            excis.Difficulty = deserializedDto.Difficulty;
            excis.Instructions = deserializedDto.Instructions;
            excis.RequiredMaterials = deserializedDto.RequiredMaterials;
            excis.MinimumRecommendedAge = deserializedDto.MinimumRecommendedAge;
            excis.ScientificBasis = deserializedDto.ScientificBasis;
            excis.IsActive = deserializedDto.IsActive;
            excis.CreatedAt = DateTime.UtcNow;
            excis.UpdatedAt = DateTime.UtcNow;

            return await _exercise.UpdateAsync(excis, id) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Получение значений из таблицы - Exercise
        /// </summary>
        /// <returns></returns>
        // GET api/Exercise/GetExercise
        [HttpGet]
        public async Task<List<Exercise>> GetExerciseAsync()
        {
            IList<Exercise> resultList = new List<Exercise>();
            resultList = await _exercise.GetValuesAsync();

            return resultList.ToList();
        }

        /// <summary>
        /// Получение упражнения по ID
        /// </summary>
        /// <returns></returns>
        // GET api/Exercise/GetFindRequest/5
        [HttpGet]
        public async Task<ExerciseDto> GetExerciseByIDAsync(int id)
        {
            ExerciseDto result = new ExerciseDto();
            result = await _exercise.GetExerciseAsync(id);

            return result;
        }

        /// <summary>
        /// Удаление упражнения по id
        /// </summary>
        /// <returns></returns>
        // DELETE: api/Exercise/Delete/5
        [HttpDelete]
        public async Task<ActionResult> DeleteExercise(int id)
        {
            return await _exercise.DeleteExerciseAsync(id) ? Ok() : BadRequest();
        }
    }
}
