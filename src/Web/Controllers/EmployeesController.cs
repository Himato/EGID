﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EGID.Application.CivilAffairs.Commands;
using EGID.Application.CivilAffairs.Queries;
using EGID.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGID.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class EmployeesController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<EmployeesVm>>> GetAll() =>
            Ok(await Mediator.Send(new GetEmployeesListQuery()));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Post([FromBody] AddEmployeeCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteEmployeeCommand{ CardId = id });

            return NoContent();
        }
    }
}