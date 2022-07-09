using JoggingApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoggingApp.Controllers
{
    [Route("jog")]
    [ApiController]
    [Authorize]
    public class JogController : ControllerBase
    {
        private readonly IJogStorage _jogStorage;
        public JogController(IJogStorage jogStorage)
        {
            _jogStorage = jogStorage ?? throw new ArgumentNullException(nameof(jogStorage));
        }

        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<Jog>> SearchAsync(DateTime? from, DateTime? to)
        {
            return await _jogStorage.SearchAsync(User.GetId(), from, to);

        }

        [HttpGet]
        [Route("get/{jogId:guid}")]
        public async Task<Jog> GetAsync(Guid jogId)
        {
            return await _jogStorage.GetByUserIdJogIdAsync(User.GetId(), jogId);
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> InsertAsync(JogInsertRequest request)
        {
            await _jogStorage.InsertAsync(Jog.Create(User.GetId(), request.Date, request.Distance, request.Latitude, request.Longitude));
            return Ok();
        }

        [HttpPut]
        [Route("{jogId:guid}/update")]
        public async Task<IActionResult> UpdateAsync(Guid jogId, JogUpdateRequest request)
        {
            var jog = await _jogStorage.GetByUserIdJogIdAsync(User.GetId(), jogId);
            if (jog is null)
            {
                return BadRequest($"Can't update jog record {jogId}. You can update only existing jog records that you own.");
            }
            await _jogStorage.UpdateAsync(jog);
            return Ok();
        }

        [HttpDelete]
        [Route("{jogId:guid}/delete")]
        public async Task<IActionResult> DeleteAsync(Guid jogId)
        {
            var jog = await _jogStorage.GetByUserIdJogIdAsync(User.GetId(), jogId);
            if (jog is null)
            {
                return BadRequest($"Can't delete jog record {jogId}. You can delete only existing jog records that you own.");
            }
            await _jogStorage.DeleteAsync(jog);
            return Ok();
        }
    }
}
