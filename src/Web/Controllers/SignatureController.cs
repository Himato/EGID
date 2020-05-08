﻿using System.Threading.Tasks;
using EGID.Application.Cards.Commands;
using EGID.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGID.Web.Controllers
{
    [Authorize]
    public class SignatureController : ApiControllerBase
    {
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Sign([FromBody] SignHashCommand command)
        {
            var signature = await Mediator.Send(command);

            return Ok(signature);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VerifySignatureResult>> Verify(
            [FromBody] VerifySignatureCommand command)
        {
            var (valid, fullName, photo) = await Mediator.Send(command);

            return Ok(new VerifySignatureResult{FullName = fullName, Valid = valid, Photo = photo});
        }
    }

    public class VerifySignatureResult
    {
        public bool Valid { get; set; }

        public FullName FullName { get; set; }

        public string Photo { get; set; }
    }
}