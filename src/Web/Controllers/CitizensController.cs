﻿using System.Linq;
using System.Threading.Tasks;
using EGID.Application.CitizenDetails.Commands;
using EGID.Application.CitizenDetails.Queries;
using EGID.Application.Common;
using EGID.Application.Common.Interfaces;
using EGID.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGID.Web.Controllers
{
    [Authorize(Roles = Roles.CivilAffairs + "," + Roles.Admin)]
    public class CitizensController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ICardManagerService _cardManager;

        public CitizensController(ICurrentUserService currentUser, ICardManagerService cardManager)
        {
            _currentUser = currentUser;
            _cardManager = cardManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var citizens = await Mediator.Send(new GetCitizenDetailsListQuery());

            return Ok(citizens);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetOne(string id, [FromBody] GetCitizenDetailsQuery query)
        {
            if (id != query.Id) return NotFound();

            if (_currentUser.UserId != id ||
                await _cardManager.IsInRoleAsync(_currentUser.UserId, Roles.CivilAffairs) ||
                await _cardManager.IsInRoleAsync(_currentUser.UserId, Roles.Admin))
                return Forbid();

            await Mediator.Send(query);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Post([FromBody] CreateCitizenCommand command)
        {
            command.Photo = Request.Files().FirstOrDefault();

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(string id, [FromBody] UpdateCitizenCommand command)
        {
            if (id != command.Id) return NotFound();

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteCitizenCommand {Id = id});

            return NoContent();
        }
    }
}