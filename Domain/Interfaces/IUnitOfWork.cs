using Domain.Entities.EducationEntities;
using Domain.Interfaces.InterfacesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEducationRepository<DegreeCourse> DegreeCourses { get; }
        IEducationRepository<DegreePath> DegreePaths { get; }
        IEducationRepository<Module> Modules { get; }
        IEducationRepository<Subject> Subjects { get; }
        Task<int> CompleteAsync();
    }
}
