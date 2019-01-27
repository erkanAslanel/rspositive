using System;
using AuthAPI.Command;
using AuthAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController: Controller
    {
        private IAccountService _service;

        public AccountController(IAccountService service)
        {

            _service = service;
        }

        [HttpPost("Register")]
        public IActionResult Post([FromBody]AccountCreateCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Register(command);
            return Ok(result);
        }
    }
}
