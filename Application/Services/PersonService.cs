using Application.DTOs.BaseDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.EntitiesBase;
using Domain.Enums.SearchableFields;
using Domain.Interfaces.Base;

namespace Application.Services
{
    public class PersonService<T> : IPersonService<T> where T : PersonExtendedBase
    {
        private readonly IMapper _mapper;
        private readonly ICRUDRepository<T> _repository;

        public PersonService(IMapper mapper, ICRUDRepository<T> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<PersonOnlyDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PersonOnlyDto>>(entities);
        }

        public async Task<PersonExtendedDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null)
            {
                return null;
            }

            return _mapper.Map<PersonExtendedDto>(entity);
        }

        public async Task CreateAsync(PersonCreateDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            entity.Id = Guid.NewGuid();
            await _repository.CreateAsync(entity);
        }

        public async Task UpdateAsync(PersonExtendedDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity is not null)
            {
                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity);
            }
            else
            {
                throw new KeyNotFoundException("Entity not found");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is not null)
            {
                await _repository.DeleteAsync(id);
            }
            else
            {
                throw new KeyNotFoundException("Entity not found");
            }
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(PersonSearchableFields field, string value)
        {
            throw new NotImplementedException();
        }
    }
}
