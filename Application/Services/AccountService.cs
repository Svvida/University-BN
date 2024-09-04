using Application.DTOs.Account.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.AccountEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.StudentEntities;
using Domain.Enums.SearchableFields;
using Domain.Interfaces.InterfacesBase;
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
        private readonly IPersonRepository<Student> _studentRepository;
        private readonly IPersonRepository<Employee> _employeeRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(
            IMapper mapper,
            IAccountRepository accountRepository,
            IPasswordHasher<UserAccount> passwordHasher,
            ILogger<AccountService> logger,
            IPersonRepository<Student> studentRepository,
            IPersonRepository<Employee> employeeRepository,
            IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _studentRepository = studentRepository;
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
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

        public async Task<VisibilityFieldsDto> GetVisibilityFieldsAsync(Guid accountId)
        {
            var role = await _roleRepository.GetRoleByAccountId(accountId);
            if (role is null)
            {
                throw new KeyNotFoundException("Role not found for the account");
            }

            _logger.LogInformation($"Retrieved role: {role.Name}");

            string name = null;
            string surename = null;

            switch (role.Name)
            {
                case "Student":
                    var student = await _studentRepository.GetByAccountIdAsync(accountId);
                    name = student.Name;
                    surename = student.Surname;
                    break;

                case "Teacher":
                    var teacher = await _employeeRepository.GetByAccountIdAsync(accountId);
                    name = teacher.Name;
                    surename = teacher.Surname;
                    break;

                default:
                    throw new Exception("Unknown role type");
            }

            var visibilityFieldsDto = new VisibilityFieldsDto
            {
                Name = name,
                Surename = surename,
                Organizer = string.Empty
            };

            return visibilityFieldsDto;
        }

        public async Task UpdatePasswordAsync(Guid accountId, string newPassword)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account is null)
            {
                throw new KeyNotFoundException("Account not found");
            }

            account.Password = _passwordHasher.HashPassword(account, newPassword);

            await _accountRepository.UpdateAsync(account);
        }
    }
}
