using Application.Interfaces;
using Application.Mappers;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class AccountService
    {
        private readonly MappingProfile _mapper;
        private readonly IAccountRepository _accountRepository;

        public AccountService(MappingProfile mapper, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
        }
    }
}
