using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Infrastructure.Data;
using Infrastructure.Services;
using System.Collections.Generic;
using Domain.Entities.EducationEntities;

namespace Infrastructure.Seeding
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, string excelFilePath)
        {
            using (var context = serviceProvider.GetRequiredService<UniversityContext>())
            {
                var excelService = new ExcelService();

                // Read data from all sheets
                var sheetData = excelService.ReadFromExcel<(string Course, string Module)>(excelFilePath, (worksheet, row) =>
                {
                    return (
                        Course: worksheet.Cells[row, 1].Text, // "Przedmiot" column
                        Module: worksheet.Cells[row, 2].Text  // "Rodzaj" column
                    );
                });

                foreach (var sheet in sheetData)
                {
                    var degreePathName = sheet.Key;
                    var data = sheet.Value;

                    var degreeProgram = context.Programs.FirstOrDefault(dp => dp.Name == degreePathName);
                    if (degreeProgram == null)
                    {
                        degreeProgram = new DegreeCourse
                        {
                            Id = Guid.NewGuid(),
                            Name = degreePathName
                        };
                        context.Programs.Add(degreeProgram);
                        context.SaveChanges();
                    }

                    var degreePath = context.DegreePaths.FirstOrDefault(dp => dp.Name == degreePathName && dp.DegreeCourseId == degreeProgram.Id);
                    if (degreePath == null)
                    {
                        degreePath = new DegreePath
                        {
                            Id = Guid.NewGuid(),
                            Name = degreePathName,
                            DegreeCourseId = degreeProgram.Id
                        };
                        context.DegreePaths.Add(degreePath);
                        context.SaveChanges();
                    }

                    var courses = new List<Subject>();
                    var modules = new List<Module>();
                    var moduleCourses = new List<ModuleSubject>();

                    foreach (var (courseName, moduleName) in data)
                    {
                        var course = courses.FirstOrDefault(c => c.Name == courseName);
                        if (course == null)
                        {
                            course = new Subject
                            {
                                Id = Guid.NewGuid(),
                                Name = courseName
                            };
                            courses.Add(course);
                        }

                        var module = modules.FirstOrDefault(m => m.Name == moduleName);
                        if (module == null)
                        {
                            module = new Module
                            {
                                Id = Guid.NewGuid(),
                                Name = moduleName,
                                DegreePathId = degreePath.Id
                            };
                            modules.Add(module);
                        }

                        moduleCourses.Add(new ModuleSubject
                        {
                            ModuleId = module.Id,
                            SubjectId = course.Id
                        });
                    }

                    context.Courses.AddRange(courses);
                    context.Modules.AddRange(modules);
                    context.ModulesCourses.AddRange(moduleCourses);
                    context.SaveChanges();
                }
            }
        }
    }
}
