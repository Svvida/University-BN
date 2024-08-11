using Domain.EntitiesBase;
using Domain.Interfaces.InterfacesBase;

namespace Domain.Interfaces.Base
{
    public interface IExtendedPersonRepository<T> : IPersonRepository<T> where T : PersonExtendedBase
    {
        Task<T> GetByPESELAsync(string pesel);
    }
}
