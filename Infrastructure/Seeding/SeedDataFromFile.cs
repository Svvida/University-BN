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
using Domain.Interfaces;

namespace Infrastructure.Seeding
{
    public class SeedDataFromFile
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExcelService _excelService;

        public SeedDataFromFile(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            _excelService = new ExcelService();
        }

        public async Task InitializeAsync(string excelFilePath)
        {
            var sheetData = _excelService.ReadFromExcel<(string Subject, string Module)>(excelFilePath, (worksheet, row) =>
            {
                return (
                Subject: worksheet.Cells[row, 1].Text,
                Module: worksheet.Cells[row, 2].Text);
            });

            Logger.Instance.Log($"Number of sheets: {sheetData.Count()}");

            var degreeCourseName = Path.GetFileNameWithoutExtension(excelFilePath);
            Logger.Instance.Log($"Processing {degreeCourseName}");

            var degreeCourse = await EnsureDegreeCourseAsync(degreeCourseName);

            foreach(var sheet in sheetData)
            {
                var degreePathName = sheet.Key;
                var data = sheet.Value;

                Logger.Instance.Log($"Degree path: {degreePathName}");

                var degreePath = await EnsureDegreePathAsync(degreeCourse, degreePathName);
                await ProcessSheetDataAsync(degreeCourse, degreePath, data);
            }
        }

        private async Task<DegreeCourse> EnsureDegreeCourseAsync(string degreeCourseName)
        {
            var degreeCourse = await _unitOfWork.DegreeCourses.FindAsync(dc => dc.Name == degreeCourseName);
            if (degreeCourse is null)
            {
                degreeCourse = new DegreeCourse { Name = degreeCourseName };
                await _unitOfWork.DegreeCourses.CreateAsync(degreeCourse);
                await _unitOfWork.CompleteAsync();
                Logger.Instance.Log($"Created new DegreeCourse: {degreeCourse.Name}");
            }
            return degreeCourse;
        }

        private async Task<DegreePath> EnsureDegreePathAsync(DegreeCourse degreeCourse, string degreePathName)
        {
            var degreePath = await _unitOfWork.DegreePaths.FindAsync(dp => dp.Name == degreePathName && dp.DegreeCourseId == degreeCourse.Id);
            if(degreePath is null)
            {
                degreePath = new DegreePath
                {
                    Name = degreePathName,
                    DegreeCourseId = degreeCourse.Id
                };
                await _unitOfWork.DegreePaths.CreateAsync(degreePath);
                await _unitOfWork.CompleteAsync();
                Logger.Instance.Log($"Created new DegreePath: {degreePath.Name}");
            }
            return degreePath;
        }

        private async Task EnsureModuleAsync(DegreePath degreePath, string moduleName, List<Module> modules, HashSet<string> existingModules)
        {
            if(moduleName == "Kierunkowy" || string.IsNullOrEmpty(moduleName))
            {
                return;
            }

            if (!existingModules.Contains(moduleName))
            {
                var exists  = await _unitOfWork.Modules.ExistsAsync(m => m.Name == moduleName && m.DegreePathId == degreePath.Id);
                if (!exists)
                {
                    var module = new Module
                    {
                        Name = moduleName,
                        DegreePathId = degreePath.Id
                    };
                    modules.Add(module);
                }
                existingModules.Add(moduleName);
            }
        }

        private async Task EnsureSubjectAsync(string subjectName, List<Subject> subjects, HashSet<string> existingStudents)
        {
            if (!existingStudents.Contains(subjectName))
            {
                var exists = await _unitOfWork.Subjects.ExistsAsync(s => s.Name == subjectName);
                if (!exists)
                {
                    var subject = new Subject
                    {
                        Name = subjectName
                    };
                    subjects.Add(subject);
                }
                existingStudents.Add(subjectName);
            }
        }

        private async Task AddToDegreeCourseSubjectAsync(DegreeCourse degreeCourse, Subject subject, List<DegreeCourseSubject> degreeCourseSubjects)
        {
            var exists = await _unitOfWork.DegreeCourseSubjects.ExistsAsync(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id);
            if(!exists)
            {
                degreeCourseSubjects.Add(new DegreeCourseSubject
                {
                    DegreeCourseId = degreeCourse.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private async Task AddToModuleSubjectAsync(Module module, Subject subject, List<ModuleSubject> moduleSubjects)
        {
            if(module is null)
            {
                return;
            }

            var exists = await _unitOfWork.ModuleSubjects.ExistsAsync(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id);
            if(!exists)
            {
                moduleSubjects.Add(new ModuleSubject
                {
                    ModuleId = module.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private async Task ProcessSheetDataAsync(DegreeCourse degreeCourse, DegreePath degreePath, List<(string Subject, string Module)> data)
        {
            var subjects = new List<Subject>();
            var modules = new List<Module>();
            var moduleSubjects = new List<ModuleSubject>();
            var degreeCourseSubjects = new List<DegreeCourseSubject>();

            var existingSubjects = new HashSet<string>();
            var existingModules = new HashSet<string>();

            foreach(var (subjectName, moduleName) in data)
            {
                if (string.IsNullOrEmpty(subjectName)||string.IsNullOrEmpty(moduleName))
                {
                    continue;
                }

                await EnsureSubjectAsync(subjectName, subjects, existingSubjects);
                var subject = subjects.FirstOrDefault(s => s.Name == subjectName);
                if(subject is null)
                {
                    throw new InvalidOperationException("Subject should have been ensured to exists");
                }

                if(moduleName == "Kierunkowy")
                {
                    await AddToDegreeCourseSubjectAsync(degreeCourse, subject, degreeCourseSubjects);
                }
                else
                {
                    await EnsureModuleAsync(degreePath, moduleName, modules, existingModules);
                    var module = modules.FirstOrDefault(m => m.Name == moduleName);
                    if(module is null)
                    {
                        throw new InvalidOperationException("Module should have been ensured to exists");
                    }
                    await AddToModuleSubjectAsync(module, subject, moduleSubjects);
                }
            }

            await _unitOfWork.Subjects.AddRangeAsync(subjects.DistinctBy(s => s.Name));
            await _unitOfWork.Modules.AddRangeAsync(modules.DistinctBy(m => m.Name));
            await _unitOfWork.CompleteAsync();

            await _unitOfWork.ModuleSubjects.AddRangeAsync(moduleSubjects.DistinctBy(ms => new { ms.ModuleId, ms.SubjectId }));
            await _unitOfWork.DegreeCourseSubjects.AddRangeAsync(degreeCourseSubjects.DistinctBy(dcs => new {dcs.DegreeCourseId, dcs.SubjectId }));
            await _unitOfWork.CompleteAsync();
        }
    }
}
