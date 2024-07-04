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

        public async Task InitailizeAsync(string excelFilePath)
        {
            var sheetData = _excelService.ReadFromExcel<(string Subject, string Module)>(excelFilePath, (worksheet, row) =>
            {
                return (
                Subject: worksheet.Cells[row, 1].Text,
                Module: worksheet.Cells[row, 2].Text);
            });

            Logger.Instance.Log($"Number of sheets: {sheetData.Count()}");

            var degreeCourseName = Path.GetFileNameWithoutExtension(excelFilePath);
            Logger.Instance.Log($"Created DegreeCourse: {degreeCourseName}");

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

        private async Task<Module> EnsureModuleAsync(DegreePath degreePath, string moduleName, List<Module> modules)
        {
            if(moduleName == "Kierunkowy" || string.IsNullOrEmpty(moduleName))
            {
                return null;
            }

            var module = await _unitOfWork.Modules.FindAsync(m => m.Name == moduleName && m.DegreePathId == degreePath.Id);
            if(module is null)
            {
                var Module = new Module
                {
                    Name = moduleName,
                    DegreePathId = degreePath.Id
                };
                modules.Add(Module);
            }
            return module;
        }

        private async Task<Subject> EnsureSubjectAsync(string subjectName, List<Subject> subjects)
        {
            var subject = await _unitOfWork.Subjects.FindAsync(s => s.Name == subjectName);
            if(subject is null)
            {
                subject = new Subject
                {
                    Name = subjectName
                };
                subjects.Add(subject);
            }
            return subject;
        }

        private async Task AddToDegreeCourseSubjectAsync(DegreeCourse degreeCourse, Subject subject, List<DegreeCourseSubject> degreeCourseSubjects)
        {
            if (!(await _unitOfWork.DegreeCourses.ExistsAsync(dcs => dcs.Id == degreeCourse.Id && dcs.DegreeCourseSubjects.Any(dcs => dcs.SubjectId == subject.Id))) &&
                !degreeCourseSubjects.Any(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id))
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

            if(!(await _unitOfWork.Modules.ExistsAsync(ms => ms.Id == module.Id && ms.ModuleSubjects.Any(ms => ms.SubjectId == subject.Id))) &&
                !moduleSubjects.Any(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id))
            {
                moduleSubjects.Add(new ModuleSubject
                {
                    ModuleId = module.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private async Task ProcessSheetDataAsync(DegreeCourse degreeCourse, DegreePath degreePath, List<(string Course, string Module)> data)
        {
            var subjects = new List<Subject>();
            var modules = new List<Module>();
            var moduleSubjects = new List<ModuleSubject>();
            var degreeCourseSubjects = new List<DegreeCourseSubject>();

            foreach(var (subjectName, moduleName) in data)
            {
                if (string.IsNullOrEmpty(subjectName)||string.IsNullOrEmpty(moduleName))
                {
                    continue;
                }

                var subject = await EnsureSubjectAsync(subjectName, subjects);

                if(moduleName == "Kierunkowy")
                {
                    await AddToDegreeCourseSubjectAsync(degreeCourse, subject, degreeCourseSubjects);
                }
                else
                {
                    var module = await EnsureModuleAsync(degreePath, moduleName, modules);
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
