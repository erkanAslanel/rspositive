using System;
using AuthAPI.Command;
using AuthAPI.Domain;
using AuthAPI.Repository;

namespace AuthAPI.Service
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public OperationResult Register(AccountCreateCommand command)
        {
            return new OperationResult {  };
        }
    }
}
