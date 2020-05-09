﻿using System.Threading.Tasks;
using EGID.Application.Cards.Commands;
using EGID.Application.Cards.Queries;
using EGID.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGID.Web.Controllers
{
    [Authorize]
    public class CardController : ApiControllerBase
    {
        [Authorize(Roles = Roles.CivilAffairs + "," + Roles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Post([FromBody] CreateCardCommand command)
        {
            var cardId = await Mediator.Send(command);

            return Ok(cardId);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Puk([FromBody] ChangePukCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Pin1([FromBody] ChangePin1Command command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Pin2([FromBody] ChangePin2Command command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> IsCorrectPin2([FromBody] IsCorrectPin2Query command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}