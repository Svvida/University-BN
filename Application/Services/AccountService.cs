using Application.DTOs.AccountDtos;
using Application.Interfaces;
using Application.Mappers;
using AutoMapper;
using Domain.Entities.AccountEntities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<object> _passwordHasher;

        public AccountService(IMapper mapper, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<AccountFullDto> GetByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException("Id cannot be null");
            }

            var account = await _accountRepository.GetByIdAsync(id);
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

        public async Task UpdateAsync(AccountFullDto account)
        {
            var existingAccount = await _accountRepository.GetByIdAsync(account.Id);
            if(existingAccount is not null)
            {
                var accountEntity = _mapper.Map(account, existingAccount);
                await _accountRepository.UpdateAsync(accountEntity);
            }
            else
            {
                throw new KeyNotFoundException("Account not found");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException("Id cannot be null");
            }
            var account  = await _accountRepository.GetByIdAsync(id);
            if(account is not null)
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
