using FluentValidation;
using JoggingApp.Core.Clock;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Jogs
{
    [Route("jog")]
    [ApiController]
    [Authorize]
    public class JogController : ControllerBase
    {
        private readonly IValidator<JogUpdateRequest> _jogUpdateValidator;
        private readonly IValidator<JogInsertRequest> _jogInsertValidator;
        private readonly IJogStorage _jogStorage;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<JogController> _logger;
        private readonly IClock _clock;
        public JogController(IJogStorage jogStorage,
            IWeatherService weatherService,
            ILogger<JogController> logger,
            IValidator<JogUpdateRequest> jogUpdateValidator, 
            IValidator<JogInsertRequest> jogInsertValidator,
            IClock clock)
        {
            _jogStorage = jogStorage ?? throw new ArgumentNullException(nameof(jogStorage));
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jogUpdateValidator = jogUpdateValidator ?? throw new ArgumentNullException(nameof(jogUpdateValidator));
            _jogInsertValidator = jogInsertValidator ?? throw new ArgumentNullException(nameof(jogInsertValidator));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<JogDto>> SearchAsync(DateTime? from, DateTime? to, CancellationToken cancellation)
        {
            var jogs = await _jogStorage.SearchAsync(User.GetId(), from, to, cancellation);
            return jogs.Select(j => new JogDto(j));
        }

        [HttpGet]
        [Route("{jogId:guid}")]
        public async Task<JogDto> GetAsync(Guid jogId, CancellationToken cancellation)
        {
            var jog = await _jogStorage.GetByUserIdJogIdAsync(User.GetId(), jogId, cancellation);
            return jog is not null ? new JogDto(jog) : null;
        }
        
        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> InsertAsync(JogInsertRequest request, CancellationToken cancellation)
        {
            var validationResult = await _jogInsertValidator.ValidateAsync(request, cancellation);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var jog = Jog.Create(User.GetId(), request.Distance, request.Time.ToTimeSpan(), _clock);
            jog.SetLocationDetail(request.Coordinates);

            await _jogStorage.InsertAsync(jog, cancellation);
            return Ok();
        }

        [HttpPut]
        [Route("{jogId:guid}/update")]
        public async Task<IActionResult> UpdateAsync(Guid jogId, JogUpdateRequest request, CancellationToken cancellation)
        {
            var validationResult = await _jogUpdateValidator.ValidateAsync(request, cancellation);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            var jog = await _jogStorage.GetByJogIdAsync(jogId, cancellation);
            if (jog is null)
            {
                return NotFound();
            }
            if (jog.UserId != User.GetId())
            {
                return BadRequest($"Can't update jog record id:{jogId}. You can update only jog records that you own.");
            }
            jog.Update(request.Distance, request.Time.ToTimeSpan());
            await _jogStorage.UpdateAsync(jog, cancellation);
            return Ok();
        }

        [HttpDelete]
        [Route("{jogId:guid}/delete")]
        public async Task<IActionResult> DeleteAsync(Guid jogId, CancellationToken cancellation)
        {
            var jog = await _jogStorage.GetByJogIdAsync(jogId, cancellation);
            if (jog is null)
            {
                return NotFound();
            }
            if (jog.UserId != User.GetId())
            {
                return BadRequest($"Can't delete jog record id:{jogId}. You can delete only jog records that you own.");
            }
            await _jogStorage.DeleteAsync(jog, cancellation);
            return Ok();
        }
    }
}
