using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Therapy_Service.Therapy.Core.Interfaces;
using Therapy_Service.Therapy.Core.Repository;
using Therapy_Service.Therapy.Host.Dto;
using Therapy_Service.Therapy.Host.Models;
using TherapyService.Therapy.Host.Dto;

namespace TherapyService.Therapy.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProgressController : Controller
    {
        private BaseRepository<Progress> _progress { get; set; }

        public ProgressController(BaseRepository<Progress> progress)
        {
            _progress = progress;
        }

        /// <summary>
        /// Создание нового упражнения
        /// </summary>
        /// <returns></returns>
        // POST: api/Progress/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync(JsonElement collection)
        {
            Progress progs = new Progress();

            var deserializedDto = JsonSerializer.Deserialize<ProgressDto>(collection);
            if (deserializedDto == null)
                return BadRequest("Failed Deserialize");

            progs.UserId = deserializedDto.UserId;
            progs.ExerciseId = deserializedDto.ExerciseId;
            progs.AverageDurationMinutes = deserializedDto.AverageDurationMinutes;
            progs.PerceivedDifficulty = deserializedDto.PerceivedDifficulty;
            progs.CurrentBenefits = deserializedDto.CurrentBenefits;
            progs.EffectivenessRating = deserializedDto.EffectivenessRating;

            return await _progress.CreateAsync(progs) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Изменение данных в упражнение
        /// </summary>
        /// <returns></returns>
        // PUT: api/Progress/Edit/5
        [HttpPut]
        public async Task<ActionResult> EditAsync(JsonElement collection, int id)
        {
            Progress progs = new Progress();

            var deserializedDto = JsonSerializer.Deserialize<ProgressDto>(collection);
            if (deserializedDto == null)
                return NoContent();

            if (id < 1)
                return ValidationProblem("Переданный ИД не валидный");

            progs.ID = id;
            progs.UserId = deserializedDto.UserId;
            progs.ExerciseId = deserializedDto.ExerciseId;
            progs.AverageDurationMinutes = deserializedDto.AverageDurationMinutes;
            progs.PerceivedDifficulty = deserializedDto.PerceivedDifficulty;
            progs.CurrentBenefits = deserializedDto.CurrentBenefits;
            progs.EffectivenessRating = deserializedDto.EffectivenessRating;

            return await _progress.UpdateAsync(progs, id) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Получение значений из таблицы - Exercise
        /// </summary>
        /// <returns></returns>
        // GET api/Progress/GetExercise
        [HttpGet]
        public async Task<List<Progress>> GetProgressAsync()
        {
            IList<Progress> resultList = new List<Progress>();
            resultList = await _progress.GetValuesAsync();

            return resultList.ToList();
        }

        /// <summary>
        /// Получение упражнения по ID
        /// </summary>
        /// <returns></returns>
        // GET api/Progress/GetFindRequest/5
        [HttpGet]
        public async Task<ProgressDto> GetProgressByIDAsync(int id)
        {
            ProgressDto result = new ProgressDto();
            result = await _progress.GetProgressAsync(id);

            return result;
        }
    }
}
