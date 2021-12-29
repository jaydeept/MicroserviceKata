using AccountService.Models;
using AccountService.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Transactions;

namespace AccountService.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAccounts();
                return Ok(accounts);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id:int}", Name = "Get")]
        [SwaggerResponse((int) HttpStatusCode.OK)]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "", typeof(Account))]
        [SwaggerResponse((int) HttpStatusCode.InternalServerError)]
        public IActionResult Get(int id)
        {
            try
            {
                var account = _accountRepository.GetAccountById(id);
                if (account == null)
                {
                    return NotFound($"Account with ID '{id}' doesn't exists.");
                }

                return Ok(account);
            }
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "", typeof(Account))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "", typeof(Account))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "", typeof(Account))]
        public IActionResult Post([FromBody] Account account)
        {
            try
            {
                if (_accountRepository.GetAccountByName(account.Name) != null)
                {
                    return Conflict($"An account already exists with name '{account.Name}'");
                }
                using var scope = new TransactionScope();
                _accountRepository.AddAccount(account);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = account.Id }, account);
            }
            catch(Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
