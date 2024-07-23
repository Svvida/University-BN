namespace Application.Interfaces.IGenericServices
{
    public interface ICRUDService<TReadOnly, TReadFull, TCreate, TUpdate>
    {
        public Task<TReadFull> GetByIdAsync(Guid id);
        public Task<IEnumerable<TReadOnly>> GetAllAsync();
        public Task CreateAsync(TCreate entity);
        public Task UpdateAsync(TUpdate entity);
        public Task DeleteAsync(Guid id);
    }
}
