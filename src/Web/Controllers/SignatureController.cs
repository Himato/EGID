﻿using System.Threading.Tasks;
using EGID.Application.Cards.Commands;
using EGID.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGID.Web.Controllers
{
    public class SignatureController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> SignHash([FromBody] SignHashCommand command)
        {
            var signature = await Mediator.Send(command);

            return Ok(signature);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<(bool valid, FullName name, string Photo)>> VerifySignature(
            [FromBody] VerifySignatureCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }
    }
}