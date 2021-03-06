﻿using System.Threading.Tasks;
using EGID.Application.Health.Commands;
using EGID.Application.Health.Queries;
using EGID.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGID.Web.Controllers
{
    [Authorize]
    public class HealthInfoController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<HealthInfoVm>> GetOne(string healthInfoId)
        {
            var result = await Mediator.Send(new GetHealthInfoQuery {HealthInfoId = healthInfoId});

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EmergencyInfo>> EmergencyInfo(string healthInfoId)
        {
            var result = await Mediator.Send(new GetEmergencyInfoQuery {HealthInfoId = healthInfoId});

            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] AddHealthRecordCommand command)
        {
            command.Attachments = Request.Files();

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EmergencyPhones([FromBody] UpdateEmergencyPhonesCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }
    }
}