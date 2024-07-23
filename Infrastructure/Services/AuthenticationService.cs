using Domain.Interfaces;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;

        public AuthenticationService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool ValidateUser(string username, string password)
        {
            var account = _accountRepository.GetByUsername(username);
            if (account is null)
            {
                return false;
            }

        }
    }
}
