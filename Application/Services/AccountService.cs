using Application.DTOs.Account.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.AccountEntities;
using Domain.Enums.SearchableFields;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<UserAccount> _passwordHasher;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, IPasswordHasher<UserAccount> passwordHasher, ILogger<AccountService> logger)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AccountFullDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetByIdAsync called with empty id");
                throw new ArgumentNullException("Id cannot be null");
            }

            var account = await _accountRepository.GetByIdAsync(id);
            if (account is null)
            {
                _logger.LogWarning("Account with id {id} not found", id);
                throw new KeyNotFoundException("Account not found");
            }

            return _mapper.Map<AccountFullDto>(account);
        }

        public async Task<IEnumerable<AccountOnlyDto>> GetAllAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountOnlyDto>>(accounts);
        }

        public async Task<IEnumerable<AccountOnlyDto>> GetByFieldAsync(AccountSearchableFields field, string value)
        {
            var accounts = await _accountRepository.GetByFieldAsync(field, value);
            return _mapper.Map<IEnumerable<AccountOnlyDto>>(accounts);
        }

        public async Task CreateAsync(AccountCreateDto account)
        {
            var accountEntity = _mapper.Map<UserAccount>(account);
            accountEntity.Id = Guid.NewGuid();
            accountEntity.Password = _passwordHasher.HashPassword(accountEntity, account.Password);

            await _accountRepository.CreateAsync(accountEntity);
        }

        public async Task UpdateAsync(AccountUpdateDto account)
        {
            var existingAccount = await _accountRepository.GetByIdAsync(account.Id);
            if (existingAccount is not null)
            {
                var accountEntity = _mapper.Map(account, existingAccount);
                accountEntity.Password = _passwordHasher.HashPassword(existingAccount, account.Password);
                await _accountRepository.UpdateAsync(accountEntity);
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id cannot be null");
            }
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is not null)
            {
                await _accountRepository.DeleteAsync(account.Id);
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }
    }
}
