using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Infrastructure.Data;
using Infrastructure.Services;
using System.Collections.Generic;
using Domain.Entities.EducationEntities;
using Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Seeding
{
    public class SeedDataFromFile
    {
        private readonly UniversityContext _context;
        private readonly ExcelService _excelService;

        public SeedDataFromFile(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<UniversityContext>();
            _excelService = new ExcelService();
        }

        public void Initialize(string excelFilePath)
        {
                var sheetData = _excelService.ReadFromExcel<(string Course, string Module)> (excelFilePath, (worksheet, row) =>
                {
                    return (
                    Course: worksheet.Cells[row, 1].Text, // "Przedmiot" column
                    Module: worksheet.Cells[row, 2].Text // "Rodzaj" column
                    );
                });

                Logger.Instance.Log($"Number of sheets: {sheetData.Count()}");

                var degreeCourseName = Path.GetFileNameWithoutExtension(excelFilePath);
                Logger.Instance.Log($"Created DegreeCourse: {degreeCourseName}");

                var degreeCourse = EnsureDegreeCourse(degreeCourseName);

                foreach(var sheet in sheetData)
                {
                    var degreePathName = sheet.Key;
                    var data = sheet.Value;

                    Logger.Instance.Log($"Degree path: {degreePathName}");

                    var degreePath = EnsureDegreePath(degreeCourse, degreePathName);
                    ProcessSheetData(degreeCourse, degreePath, data);
                }
            
        }

        private DegreeCourse EnsureDegreeCourse(string degreeCourseName)
        {
            var degreeCourse = _context.DegreeCourses.AsNoTracking().FirstOrDefault(dc => dc.Name == degreeCourseName);
            if(degreeCourse is null)
            {
                degreeCourse = new DegreeCourse
                {
                    Id = Guid.NewGuid(),
                    Name = degreeCourseName
                };
                _context.DegreeCourses.Add( degreeCourse );
                _context.SaveChanges();
                Logger.Instance.Log($"Created new DegreeCourse: {degreeCourse}");
            }
            return degreeCourse;
        }

        private DegreePath EnsureDegreePath(DegreeCourse degreeCourse, string degreePathName)
        {
            var degreePath = _context.DegreePaths.AsNoTracking().FirstOrDefault(dp => dp.Name == degreePathName && dp.DegreeCourseId == degreeCourse.Id);
            if(degreePath is null)
            {
                degreePath = new DegreePath
                {
                    Id = Guid.NewGuid(),
                    Name = degreePathName,
                    DegreeCourseId = degreeCourse.Id
                };
                _context.DegreePaths.Add( degreePath );
                _context.SaveChanges();
                Logger.Instance.Log($"Created new DegreePath: {degreePath}");
            }
            return degreePath;
        }

        private Module EnsureModule(DegreePath degreePath, string moduleName, List<Module> modules)
        {
            var module = _context.Modules.AsNoTracking().FirstOrDefault(m => m.Name == moduleName && m.DegreePathId == degreePath.Id);
            if(module is null)
            {
                module = new Module
                {
                    Id = Guid.NewGuid(),
                    Name = moduleName,
                    DegreePathId = degreePath.Id
                };
                modules.Add( module );
            }
            return module;
        }

        private Subject EnsureSubject(string subjectName, List<Subject> subjects)
        {
            var subject = _context.Subjects.AsNoTracking().FirstOrDefault(s => s.Name == subjectName);
            if(subject is null)
            {
                subject = new Subject
                {
                    Id = Guid.NewGuid(),
                    Name = subjectName,
                };
                subjects.Add( subject );
            }
            return subject;
        }

        private void AddToDegreeCourseSubject(DegreeCourse degreeCourse, Subject subject, List<DegreeCourseSubject> degreeCourseSubjects)
        {
            if (!_context.DegreeCourseSubjects.Any(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id) &&
                !degreeCourseSubjects.Any(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id))
            {
                degreeCourseSubjects.Add(new DegreeCourseSubject
                {
                    DegreeCourseId = degreeCourse.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private void AddToModuleSubject(Module module, Subject subject, List<ModuleSubject> moduleSubjects)
        {
            if (!_context.ModulesSubjects.Any(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id) &&
                !moduleSubjects.Any(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id))
            {
                moduleSubjects.Add(new ModuleSubject
                {
                    ModuleId = module.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private void ProcessSheetData(DegreeCourse degreeCourse, DegreePath degreePath, List<(string Course, string Module)> data)
        {
            var subjects = new List<Subject>();
            var modules = new List<Module>();
            var moduleSubjects = new List<ModuleSubject>();
            var degreeCourseSubjects = new List<DegreeCourseSubject>();

            foreach(var (subjectName, moduleName) in data)
            {
                if(string.IsNullOrEmpty(subjectName))
                {
                    continue;
                }

                var subject = EnsureSubject(subjectName, subjects);

                if(moduleName == "Kierunkowy")
                {
                    AddToDegreeCourseSubject(degreeCourse, subject, degreeCourseSubjects);
                }
                else if(!string.IsNullOrEmpty(moduleName))
                {
                    var module = EnsureModule(degreePath, moduleName, modules);
                    AddToModuleSubject(module, subject, moduleSubjects);
                }
            }

            _context.Subjects.AddRange(subjects);
            _context.Modules.AddRange(modules);
            _context.SaveChanges();

            // Add moduleSubjects and degreeCourseSubjects after saving changes to avoid tracking issues
            _context.ModulesSubjects.AddRange(moduleSubjects.Where(ms =>
            !_context.ModulesSubjects.Any(existingMs => existingMs.ModuleId == ms.ModuleId && existingMs.SubjectId == ms.SubjectId)));

            _context.DegreeCourseSubjects.AddRange(degreeCourseSubjects.Where(dcs =>
            !_context.DegreeCourseSubjects.Any(existingDcs => existingDcs.DegreeCourseId == dcs.DegreeCourseId && existingDcs.SubjectId == dcs.SubjectId)));

            _context.SaveChanges();
        }
    }
}
