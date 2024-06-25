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
        public static void Initialize(IServiceProvider serviceProvider, string excelFilePath)
        {
                var context = serviceProvider.GetRequiredService<UniversityContext>();
                var excelService = new ExcelService();
                var sheetData = excelService.ReadFromExcel<(string Course, string Module)> (excelFilePath, (worksheet, row) =>
                {
                    return (
                    Course: worksheet.Cells[row, 1].Text, // "Przedmiot" column
                    Module: worksheet.Cells[row, 2].Text // "Rodzaj" column
                    );
                });

                Logger.Instance.Log($"Number of sheets: {sheetData.Count()}");

                var degreeCourseName = Path.GetFileNameWithoutExtension(excelFilePath);
                Logger.Instance.Log($"Created DegreeCourse: {degreeCourseName}");

                var degreeCourse = EnsureDegreeCourse(context, degreeCourseName);

                foreach(var sheet in sheetData)
                {
                    var degreePathName = sheet.Key;
                    var data = sheet.Value;

                    Logger.Instance.Log($"Degree path: {degreePathName}");

                    var degreePath = EnsureDegreePath(context, degreeCourse, degreePathName);
                    ProcessSheetData(context, degreeCourse, degreePath, data);
                }
            
        }

        private static DegreeCourse EnsureDegreeCourse(UniversityContext context, string degreeCourseName)
        {
            var degreeCourse = context.DegreeCourses.FirstOrDefault(dc => dc.Name == degreeCourseName);
            if(degreeCourse is null)
            {
                degreeCourse = new DegreeCourse
                {
                    Id = Guid.NewGuid(),
                    Name = degreeCourseName
                };
                context.DegreeCourses.Add( degreeCourse );
                context.SaveChanges();
                Logger.Instance.Log($"Created new DegreeCourse: {degreeCourse}");
            }
            return degreeCourse;
        }

        private static DegreePath EnsureDegreePath(UniversityContext context, DegreeCourse degreeCourse, string degreePathName)
        {
            var degreePath = context.DegreePaths.FirstOrDefault(dp => dp.Name == degreePathName && dp.DegreeCourseId == degreeCourse.Id);
            if(degreePath is null)
            {
                degreePath = new DegreePath
                {
                    Id = Guid.NewGuid(),
                    Name = degreePathName,
                    DegreeCourseId = degreeCourse.Id
                };
                context.DegreePaths.Add( degreePath );
                context.SaveChanges();
                Logger.Instance.Log($"Created new DegreePath: {degreePath}");
            }
            return degreePath;
        }

        private static Module EnsureModule(UniversityContext context, DegreePath degreePath, string moduleName, List<Module> modules)
        {
            var module = context.Modules.FirstOrDefault(m => m.Name == moduleName && m.DegreePathId == degreePath.Id);
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
            else
            {
                context.Entry(module).State = EntityState.Detached;
            }
            return module;
        }

        private static Subject EnsureSubject(UniversityContext context, string subjectName, List<Subject> subjects)
        {
            var subject = context.Subjects.FirstOrDefault(s => s.Name == subjectName);
            if(subject is null)
            {
                subject = new Subject
                {
                    Id = Guid.NewGuid(),
                    Name = subjectName,
                };
                subjects.Add( subject );
            }
            else
            {
                context.Entry(subject).State = EntityState.Detached;
            }
            return subject;
        }

        private static void AddToDegreeCourseSubject(UniversityContext context, DegreeCourse degreeCourse, Subject subject, List<DegreeCourseSubject> degreeCourseSubjects)
        {
            if (!context.DegreeCourseSubjects.Any(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id) &&
                !degreeCourseSubjects.Any(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id))
            {
                degreeCourseSubjects.Add(new DegreeCourseSubject
                {
                    DegreeCourseId = degreeCourse.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private static void AddToModuleSubject(UniversityContext context, Module module, Subject subject, List<ModuleSubject> moduleSubjects)
        {
            if (!context.ModulesSubjects.Any(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id) &&
                !moduleSubjects.Any(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id))
            {
                moduleSubjects.Add(new ModuleSubject
                {
                    ModuleId = module.Id,
                    SubjectId = subject.Id
                });
            }
        }

        private static void ProcessSheetData(UniversityContext context, DegreeCourse degreeCourse, DegreePath degreePath, List<(string Course, string Module)> data)
        {
            var subjects = new List<Subject>();
            var modules = new List<Module>();
            var moduleSubjects = new List<ModuleSubject>();
            var degreeCourseSubjects = new List<DegreeCourseSubject>();

            foreach(var (subjectName, moduleName) in data)
            {
                var subject = EnsureSubject(context, subjectName, subjects);

                if(moduleName == "Kierunkowy")
                {
                    AddToDegreeCourseSubject(context, degreeCourse, subject, degreeCourseSubjects);
                }
                else if(!string.IsNullOrEmpty(moduleName))
                {
                    var module = EnsureModule(context, degreePath, moduleName, modules);
                    AddToModuleSubject(context, module, subject, moduleSubjects);
                }
            }

            context.Subjects.AddRange(subjects);
            context.Modules.AddRange(modules);
            context.SaveChanges();

            // Add moduleSubjects and degreeCourseSubjects after saving changes to avoid tracking issues
            context.ModulesSubjects.AddRange(moduleSubjects.Where(ms =>
            !context.ModulesSubjects.Any(existingMs => existingMs.ModuleId == ms.ModuleId && existingMs.SubjectId == ms.SubjectId)));

            context.DegreeCourseSubjects.AddRange(degreeCourseSubjects.Where(dcs =>
            !context.DegreeCourseSubjects.Any(existingDcs => existingDcs.DegreeCourseId == dcs.DegreeCourseId && existingDcs.SubjectId == dcs.SubjectId)));

            context.SaveChanges();
        }
    }
}
