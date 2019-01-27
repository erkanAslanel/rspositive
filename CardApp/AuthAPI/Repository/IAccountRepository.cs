using System;
using AuthAPI.Domain.Account;

namespace AuthAPI.Repository
{
    public interface IAccountRepository
    {
        void Save(Account item);

    }
}
