﻿using Domain.EntitiesBase;
using Domain.Enums;
using Domain.Interfaces.InterfacesBase;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RepositoriesBase
{
    public class EducationRepository<T> : IEducationRepository<T> where T : EducationBase
    {
        private readonly UniversityContext _context;
        private readonly DbSet<T> _educations;

        public EducationRepository(UniversityContext context)
        {
            _context = context;
            _educations = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var education = await _educations.FindAsync(id);
            if(education is not null)
            {
                return education;
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} not found");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _educations.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByFieldAsync(EducationSearchableFields field, string value)
        {
            string dbFieldName = GetDbFieldName(field);

            var education = await _educations.Where(e => EF.Property<string>(e, dbFieldName) == value).ToListAsync();

            if (!education.Any())
            {
                throw new KeyNotFoundException($"No {typeof(T).Name} with {field} == {value} was found");
            }

            return education;
        }

        private string GetDbFieldName(EducationSearchableFields field)
        {
            switch (field)
            {
                case EducationSearchableFields.Name:
                    return "Name";
                default:
                    throw new ArgumentException("Invalid field", nameof(field));
            }
        }

        public async Task CreateAsync(T education)
        {
            if (education is null)
            {
                throw new ArgumentNullException(nameof(education), $"{typeof(T).Name} cannot be null");
            }

            await _educations.AddAsync(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T education)
        {
            if (education is null)
            {
                throw new ArgumentNullException(nameof(education), $"{typeof(T).Name} cannot be null");
            }

            _educations.Update(education);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var education = await _educations.FindAsync(id);
            if(education is not null)
            {
                _educations.Remove(education);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with ID: {id} not found");
            }
        }
    }
}
