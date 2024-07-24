using Domain.Entities.EducationEntities;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeding
{
    public class SeedDataFromFile
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExcelService _excelService;
        private readonly ILogger<SeedDataFromFile> _logger;

        public SeedDataFromFile(IUnitOfWork unitOfWork, ExcelService excelService, ILogger<SeedDataFromFile> logger)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _logger = logger;
        }

        public async Task InitializeAsync(string excelFilePath)
        {
            _logger.LogInformation($"Seeding database from file: {excelFilePath}");

            var sheetData = _excelService.ReadFromExcel<(string Subject, string Module)>(excelFilePath, (worksheet, row) =>
            {
                return (
                Subject: worksheet.Cells[row, 1].Text,
                Module: worksheet.Cells[row, 2].Text);
            });

            _logger.LogInformation($"Number of sheets: {sheetData.Count()}");

            var degreeCourseName = Path.GetFileNameWithoutExtension(excelFilePath);
            //_logger.LogInformation($"Processing {degreeCourseName}");

            var degreeCourse = await EnsureDegreeCourseAsync(degreeCourseName);

            foreach (var sheet in sheetData)
            {
                var degreePathName = sheet.Key;
                var data = sheet.Value;

                //_logger.LogInformation($"Degree path: {degreePathName}");

                var degreePath = await EnsureDegreePathAsync(degreeCourse, degreePathName);
                await ProcessSheetDataAsync(degreeCourse, degreePath, data);
            }
            _logger.LogInformation($"Seeding from file completed, disposing");
        }

        private async Task<DegreeCourse> EnsureDegreeCourseAsync(string degreeCourseName)
        {
            var degreeCourse = await _unitOfWork.DegreeCourses.FindAsync(dc => dc.Name == degreeCourseName);
            if (degreeCourse is null)
            {
                degreeCourse = new DegreeCourse { Name = degreeCourseName };
                await _unitOfWork.DegreeCourses.CreateAsync(degreeCourse);
                await _unitOfWork.CompleteAsync();
                //_logger.LogInformation($"Created new DegreeCourse: {degreeCourse.Name}");
            }
            return degreeCourse;
        }

        private async Task<DegreePath> EnsureDegreePathAsync(DegreeCourse degreeCourse, string degreePathName)
        {
            var degreePath = await _unitOfWork.DegreePaths.FindAsync(dp => dp.Name == degreePathName && dp.DegreeCourseId == degreeCourse.Id);
            if (degreePath is null)
            {
                degreePath = new DegreePath
                {
                    Name = degreePathName,
                    DegreeCourseId = degreeCourse.Id
                };
                await _unitOfWork.DegreePaths.CreateAsync(degreePath);
                await _unitOfWork.CompleteAsync();
                //_logger.LogInformation($"Created new DegreePath: {degreePath.Name}");
            }
            return degreePath;
        }

        private async Task<Module> EnsureModuleAsync(DegreePath degreePath, string moduleName)
        {
            var module = await _unitOfWork.Modules.FindAsync(m => m.Name == moduleName && m.DegreePathId == degreePath.Id);
            if (module is null)
            {
                module = new Module
                {
                    Name = moduleName,
                    DegreePathId = degreePath.Id
                };
                await _unitOfWork.Modules.CreateAsync(module);
                await _unitOfWork.CompleteAsync();
                //_logger.LogInformation($"Created new Module: {module.Name}");
            }
            return module;
        }

        private async Task<Subject> EnsureSubjectAsync(string subjectName)
        {
            var subject = await _unitOfWork.Subjects.FindAsync(s => s.Name == subjectName);
            if (subject is null)
            {
                subject = new Subject { Name = subjectName };
                await _unitOfWork.Subjects.CreateAsync(subject);
                await _unitOfWork.CompleteAsync();
                //_logger.LogInformation($"Created new Subject: {subject.Name}");
            }
            return subject;
        }

        private async Task AddToDegreeCourseSubjectAsync(DegreeCourse degreeCourse, Subject subject, List<DegreeCourseSubject> degreeCourseSubjects)
        {
            var exists = await _unitOfWork.DegreeCourseSubjects.ExistsAsync(dcs => dcs.DegreeCourseId == degreeCourse.Id && dcs.SubjectId == subject.Id);
            if (!exists)
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

            var exists = await _unitOfWork.ModuleSubjects.ExistsAsync(ms => ms.ModuleId == module.Id && ms.SubjectId == subject.Id);
            if (!exists)
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
            var moduleSubjects = new List<ModuleSubject>();
            var degreeCourseSubjects = new List<DegreeCourseSubject>();

            foreach (var (subjectName, moduleName) in data)
            {
                if (subjectName.Contains("Razem Semestr") || string.IsNullOrEmpty(moduleName))
                {
                    //_logger.LogInformation($"Skipping subject {subjectName}, Module {moduleName}");
                    continue;
                }
                //_logger.LogInformation($"Subject before ensuring: {subjectName}, Module: {moduleName}");

                var subject = await EnsureSubjectAsync(subjectName);
                //_logger.LogInformation($"Subject after fetching: {subject.Name}, {subject.Id}");

                if (moduleName == "Kierunkowy")
                {
                    await AddToDegreeCourseSubjectAsync(degreeCourse, subject, degreeCourseSubjects);
                }
                else
                {
                    var module = await EnsureModuleAsync(degreePath, moduleName);

                    await AddToModuleSubjectAsync(module, subject, moduleSubjects);
                }
            }

            if (moduleSubjects.Any())
            {
                await _unitOfWork.ModuleSubjects.AddRangeAsync(moduleSubjects.DistinctBy(ms => new { ms.ModuleId, ms.SubjectId }));
            }
            if (degreeCourseSubjects.Any())
            {
                await _unitOfWork.DegreeCourseSubjects.AddRangeAsync(degreeCourseSubjects.DistinctBy(dcs => new { dcs.DegreeCourseId, dcs.SubjectId }));

            }
            await _unitOfWork.CompleteAsync();
        }
    }
}
