using System;
using AuthAPI.Command;
using AuthAPI.Domain;

namespace AuthAPI.Service
{
    public interface IAccountService
    {
        OperationResult Register(AccountCreateCommand command);
    }
}
