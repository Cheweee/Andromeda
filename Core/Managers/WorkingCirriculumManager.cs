using Andromeda.Core.Logs;
using Andromeda.Models.Context;
using Andromeda.Common.Extensions;
using Andromeda.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web;
using Andromeda.Models.References;
using Andromeda.Models.Entities;

namespace Andromeda.Core.Managers
{
    public class WorkingCirriculumManager
    {
        public static IViewModel GetWorkingCirriculums(int page, int limit, string order, bool isAscending, string search)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<WorkingCirriculumViewModel> data = new EntitiesViewModel<WorkingCirriculumViewModel>
                    {
                        Result = Result.Ok
                    };

                    var tempEntities = context.Departments.Where(o=> !o.IsFaculty).Join(context.WorkingCirriculums.AsNoTracking(), 
                        d=> d.Id,
                        w=> w.DepartmentId,
                        (d, w) => new WorkingCirriculumViewModel
                        {
                            Id = w.Id,
                            AreaOfTrainingId = w.AreaOfTrainingId,
                            StartTraining = w.StartTraining,
                            TrainingPeriod = w.TrainingPeriod,
                            TypeOfEducationName = w.TypeOfEducationName,
                            AreaOfTrainingName = context.AreasOfTraining.Where(a => a.Id == w.AreaOfTrainingId).Select(a => a.Name).FirstOrDefault(),
                            DepartmentId = d.Id,
                            DepartmentName = d.Name,
                            EducationalStandart = w.EducationalStandart,
                            FacultyId = context.Departments.Where(f => f.IsFaculty && f.Id == d.FacultyId).Select(f => f.Id).FirstOrDefault(),
                            FacultyName = context.Departments.Where(f => f.IsFaculty && f.Id == d.FacultyId).Select(f => f.Name).FirstOrDefault()
                        }).ToList();
                    tempEntities = tempEntities.Where(o => o.AreaOfTrainingName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.DepartmentName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.EducationalStandart.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.FacultyName.ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.StartTraining.ToString().ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.TrainingPeriod.ToString().ToLower().Contains((search ?? string.Empty).ToLower()) ||
                    o.TypeOfEducationName.ToLower().Contains((search ?? string.Empty).ToLower())).ToList();

                    data.Total = tempEntities.Count;

                    data.Entities = isAscending ? tempEntities
                    .OrderBy(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList() :
                     tempEntities
                    .OrderByDescending(order)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToList();
                    data.Page = page;

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetWorkingCirriculum(Guid id)
        {
            try
            {
                using (DBContext context= new DBContext())
                {
                    EntityViewModel<WorkingCirriculumViewModel> data = new EntityViewModel<WorkingCirriculumViewModel>
                    {
                        Result = Result.Ok
                    };

                    data.Entity = context.WorkingCirriculums.Where(o => o.Id == id).AsNoTracking().Select(o => new WorkingCirriculumViewModel
                    {
                        Id = o.Id,
                        AreaOfTrainingId = o.AreaOfTrainingId,
                        DepartmentId = o.DepartmentId,
                        EducationalStandart = o.EducationalStandart,
                        StartTraining = o.StartTraining,
                        TrainingPeriod = o.TrainingPeriod,
                        TypeOfEducationName = o.TypeOfEducationName,
                        DepartmentName = context.Departments.Where(d => !d.IsFaculty && d.Id == o.DepartmentId).Select(d => d.Name).FirstOrDefault(),
                        AreaOfTrainingName = context.AreasOfTraining.Where(a => a.Id == o.AreaOfTrainingId).Select(a => a.Name).FirstOrDefault()
                    }).FirstOrDefault();

                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetWorkingCirriculumAcademicDisciplines(int page, int limit, string order, bool isAscending, string search, Guid? id)
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntitiesViewModel<AcademicDisciplineViewModel> model = new EntitiesViewModel<AcademicDisciplineViewModel>
                    {
                        Result = Result.Ok
                    };

                    return model;
                }
            }
            catch(Exception exc)
            {
                return new ResultViewModel { Result = Result.Error, Message = exc.Message };
            }
        }
        public static IViewModel UploadWorkingCirriculumFile()
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    EntityViewModel<WorkingCirriculumViewModel> data = new EntityViewModel<WorkingCirriculumViewModel>
                    {
                        Result = Result.Ok
                    };

                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    foreach(string fileName in files)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[fileName];

                        if(file.ContentLength > 0)
                        {
                            string workingCirriculumFileName = file.FileName.Substring(0, file.FileName.LastIndexOf('.') -1);
                            string workingCirriculumFileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                            int fileSize = file.ContentLength;
                            byte[] fileData = new byte[fileSize];
                            file.InputStream.Read(fileData, 0, fileSize);

                            MemoryStream stream = new MemoryStream(fileData);

                            data.Entity = AnalizeWorkingCirriculumFile(context, stream);

                            WorkingCirriculumFile workingCirriculumFile = new WorkingCirriculumFile
                            {
                                Id = Guid.NewGuid(),
                                FileName = workingCirriculumFileName,
                                FileExtension = workingCirriculumFileExtension,
                                FileSize = fileSize,
                                FileData = fileData
                            };
                        }
                    }


                    return data;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }

        private static WorkingCirriculumViewModel AnalizeWorkingCirriculumFile(DBContext context, Stream stream)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
            {
                WorkingCirriculumViewModel data = AnalizeTitleSheet(context, document);
                data.AcademicDisciplines = AnalizePlanSheet(context, document);

                return data;
            }
        }

        private static WorkingCirriculumViewModel AnalizeTitleSheet(DBContext context, SpreadsheetDocument document)
        {
            string sheetName = "Титул";
            WorkbookPart workbookPart = document.WorkbookPart;
            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            WorkingCirriculumViewModel workingCirriculum = new WorkingCirriculumViewModel();
            AreaOfTrainingViewModel areaOfTraining = new AreaOfTrainingViewModel();

            string typeOfEducationSearch = "форма обучения";
            string trainingPeriodSearch = "срок обучения";
            string startPeriodSearch = "год начала подготовки";
            string educationalStandartSearch = "образовательный стандарт";
            string departmentWord = "кафедра";
            string levelOfHigherEducationWord = "квалификация";

            int areaOfTrainingRowIndex = 0;

            bool needToStopForStartPeriod = false;
            bool needToStopForEducationalStandart = false;

            bool needToSearchDepartmentId = false;
            bool needToStopForAreaOfTrainingCode = true;
            bool needSearchAreaOfTraining = false;

            foreach (Row row in sheetData.Elements<Row>())
            {
                foreach (Cell cell in row.Elements<Cell>())
                {
                    string value = GetCellValue(workbookPart, cell);

                    if (value.ToLower().Contains(typeOfEducationSearch))
                    {
                        workingCirriculum.TypeOfEducationName = GetTypeOfEducationName(value);
                    }
                    else if (value.ToLower().Contains(trainingPeriodSearch))
                    {
                        workingCirriculum.TrainingPeriod = GetTrainingPeriod(value);
                    }
                    else if (value.ToLower().Contains(startPeriodSearch))
                    {
                        needToStopForStartPeriod = true;
                    }
                    else if (int.TryParse(value, out int tempStartYear) && needToStopForStartPeriod)
                    {
                        workingCirriculum.StartTraining = tempStartYear;
                        needToStopForStartPeriod = false;
                    }
                    else if (value.ToLower().Contains(educationalStandartSearch))
                    {
                        needToStopForEducationalStandart = true;
                    }
                    else if (!string.IsNullOrEmpty(value) && needToStopForEducationalStandart)
                    {
                        workingCirriculum.EducationalStandart = value;
                        needToStopForEducationalStandart = false;
                    }

                    else if (needToStopForAreaOfTrainingCode && value.Split('.').Length > 1 && value.Length <= 8)
                    {
                        if (context.AreasOfTraining.Where(o => o.Code == value).AsNoTracking().Count() > 0)
                        {
                            areaOfTraining = context.AreasOfTraining.Where(o => o.Code == value).AsNoTracking().Select(o => new AreaOfTrainingViewModel
                            {
                                Id = o.Id,
                                Code = o.Code,
                                Directionaly = o.Directionaly,
                                LevelOfHigherEducationName = o.LevelOfHigherEducationName,
                                Name = o.Name,
                                ShortName = o.ShortName
                            })
                            .FirstOrDefault();

                            needSearchAreaOfTraining = false;
                        }
                        else
                        {
                            areaOfTraining.Code = value;
                            needSearchAreaOfTraining = true;
                        }
                        needToStopForAreaOfTrainingCode = false;
                        areaOfTrainingRowIndex = int.Parse(row.RowIndex.ToString());
                    }
                    else if (needSearchAreaOfTraining && (int.Parse(row.RowIndex) - areaOfTrainingRowIndex == 1) && !string.IsNullOrEmpty(value))
                    {
                        areaOfTraining.Name = GetAreaOfTrainingName(value);
                        areaOfTraining.ShortName = GetAreaOfTrainingShortName(areaOfTraining.Name);
                    }
                    else if (needSearchAreaOfTraining &&
                        (int.Parse(row.RowIndex) - areaOfTrainingRowIndex == 2) &&
                        !string.IsNullOrEmpty(value))
                    {
                        areaOfTraining.Directionaly = GetAreaOfTrainingDirectionaly(value);
                    }
                    else if (needSearchAreaOfTraining && value.ToLower().Contains(levelOfHigherEducationWord))
                    {
                        string levelOfHigherEducationName = GetLevelOfHigherEducationName(value);
                        areaOfTraining.LevelOfHigherEducationName = context.LevelsOfHigherEducation.Where(o =>
                        o.Name.ToLower().Contains(levelOfHigherEducationName.ToLower())).AsNoTracking().FirstOrDefault().Name;
                    }
                    else if (value.ToLower().Contains(departmentWord))
                    {
                        needToSearchDepartmentId = true;
                    }
                    else if (needToSearchDepartmentId && !string.IsNullOrEmpty(value))
                    {
                        var tempDepartment = context.Departments.Where(o => o.Name.ToLower().Contains(value.ToLower())).Select(o => new { o.Id, o.Name }).FirstOrDefault();
                        workingCirriculum.DepartmentId = tempDepartment.Id;
                        workingCirriculum.DepartmentName = tempDepartment.Name;
                        areaOfTraining.DepartmentId = tempDepartment.Id;
                        areaOfTraining.DepartmentName = tempDepartment.Name;
                        needToSearchDepartmentId = false;
                    }
                }
            }

            workingCirriculum.AreaOfTraining = areaOfTraining;
            return workingCirriculum;
        }
        private static List<AcademicDisciplineViewModel> AnalizePlanSheet(DBContext context, SpreadsheetDocument document)
        {
            List<AcademicDisciplineViewModel> data = new List<AcademicDisciplineViewModel>();

            string sheetName = "План";
            WorkbookPart workbookPart = document.WorkbookPart;
            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            Dictionary<string, string> columns = new Dictionary<string, string>
            {

                { "По плану",  string.Empty },
                { "По ЗЕТ",  string.Empty},
                { "Экспертное",  string.Empty},
                { "Факт",  string.Empty},
                { "Итого часов в электронной форме",  string.Empty},
                { "Итого часов в интерактивной форме",  string.Empty},
                { "Контакт. раб.",  string.Empty},
                { "СРС",  string.Empty},
                { "Контроль",  string.Empty},
                { "Наименование",  string.Empty},
                { "Индекс",  string.Empty},
                { "Компетенции",  string.Empty}
            };

            GetColumnNames(workbookPart, sheetData, ref columns);

            string searchBasePart = "б1.б";
            string searchVariativePart = "б1.в.од";
            string searchTrainingPracticePart = "б2.у";
            string searchResearchWorkPart = "б2.н";
            string searchInternshipPart = "б2.п";
            data.AddRange(SearchPart(workbookPart, sheetData, searchBasePart, columns));
            data.AddRange(SearchPart(workbookPart, sheetData, searchVariativePart, columns));
            data.AddRange(SearchOptionallyPart(workbookPart, sheetData, columns));
            data.AddRange(SearchPart(workbookPart, sheetData, searchTrainingPracticePart, columns));
            data.AddRange(SearchPart(workbookPart, sheetData, searchResearchWorkPart, columns));
            data.AddRange(SearchPart(workbookPart, sheetData, searchInternshipPart, columns));

            return data;
        }

        private static string GetTypeOfEducationName(string value)
        {
            string tempTypeOfEducationName = string.Empty;
            if (value.Contains(':'))
            {
                tempTypeOfEducationName = value.Substring(value.LastIndexOf(':') + 1);
                if (string.IsNullOrWhiteSpace(tempTypeOfEducationName.FirstOrDefault().ToString()))
                {
                    tempTypeOfEducationName = tempTypeOfEducationName.Substring(1, tempTypeOfEducationName.Length - 1);
                }
            }
            else
            {
                tempTypeOfEducationName = value.Substring(value.LastIndexOf(' ') + 1);
            }

            if(tempTypeOfEducationName.Length > 0)
            {
                tempTypeOfEducationName = tempTypeOfEducationName[0].ToString().ToUpper() + tempTypeOfEducationName.Substring(1);
            }

            return tempTypeOfEducationName;
        }
        private static int GetTrainingPeriod(string value)
        {
            string tempTrainingPeriod = value;
            if (tempTrainingPeriod.Contains(':'))
            {
                tempTrainingPeriod = tempTrainingPeriod.Substring(tempTrainingPeriod.LastIndexOf(':') + 1);
                if (string.IsNullOrWhiteSpace(tempTrainingPeriod.FirstOrDefault().ToString()))
                {
                    tempTrainingPeriod = tempTrainingPeriod.Substring(1, tempTrainingPeriod.Length - 1);
                }
            }
            else
            {
                tempTrainingPeriod = tempTrainingPeriod.Substring(tempTrainingPeriod.LastIndexOf(' ') + 1);
            }
            if (tempTrainingPeriod.Contains('г'))
            {
                tempTrainingPeriod = tempTrainingPeriod.Substring(0, tempTrainingPeriod.LastIndexOf('г'));
            }

            int.TryParse(tempTrainingPeriod, out int trainingPeriod);

            return trainingPeriod;
        }
        private static string GetAreaOfTrainingName(string value)
        {
            string areaOfTrainingName = string.Empty;
            string directionalyWord = "направленность";
            if (value.ToLower().Contains(directionalyWord))
            {
                areaOfTrainingName = value.Substring(directionalyWord.Length + 1);
                return areaOfTrainingName;
            }
            directionalyWord = "направление";
            if (value.ToLower().Contains(directionalyWord))
            {
                areaOfTrainingName = value.Substring(directionalyWord.Length + 1);
                return areaOfTrainingName;
            }
            areaOfTrainingName = value;

            return areaOfTrainingName;
        }
        private static string GetAreaOfTrainingShortName(string value)
        {
            string areaOfTrainingShortName = string.Empty;
            string notUsing = "\"\'()1234567890";
            IEnumerable<string> words = value.Split(' ').Where(o=> o.Length > 1);
            
            foreach(string word in words)
            {
                string firstChar = word.FirstOrDefault().ToString();
                if(notUsing.Contains(firstChar))
                {
                    continue;
                }
                areaOfTrainingShortName += firstChar;
            }

            return areaOfTrainingShortName;
        }
        private static string GetAreaOfTrainingDirectionaly(string value)
        {
            string directionaly = string.Empty;
            string directionalyWord = "направленность";
            if (value.ToLower().Contains(directionalyWord))
            {
                directionaly = value.Substring(directionalyWord.Length + 1);
            }
            directionalyWord = "профиль";
            if (value.ToLower().Contains(directionalyWord))
            {
                directionaly = value.Substring(directionalyWord.Length + 1);
            }

            return directionaly;
        }
        private static string GetLevelOfHigherEducationName(string value)
        {
            string levelOfHigherEducationName = string.Empty;
            if (value.Contains(':'))
            {
                levelOfHigherEducationName = value.Substring(value.LastIndexOf(':') + 1);
                if (string.IsNullOrWhiteSpace(levelOfHigherEducationName.FirstOrDefault().ToString()))
                {
                    levelOfHigherEducationName = levelOfHigherEducationName.Substring(1, levelOfHigherEducationName.Length - 1);
                }
            }
            else
            {
                levelOfHigherEducationName = value.Substring(value.LastIndexOf(' ') + 1);
            }

            return levelOfHigherEducationName;
        }
        private static void GetColumnNames(WorkbookPart workbookPart, SheetData sheetData, ref Dictionary<string, string> columns)
        {
            foreach (Row row in sheetData.Elements<Row>())
            {
                foreach (Cell cell in row.Elements<Cell>())
                {
                    string cellValue = GetCellValue(workbookPart, cell);
                    var column = columns.Where(o => cellValue.ToLower().Contains(o.Key.ToLower())).FirstOrDefault();
                    if(!string.IsNullOrEmpty(column.Key) && string.IsNullOrEmpty(column.Value))
                    {
                        columns[column.Key] = GetColumnName(cell.CellReference);
                    }

                    if(columns.All(o=> !string.IsNullOrEmpty(o.Value)))
                    {
                        return;
                    }
                }
            }
        }
        private static List<AcademicDisciplineViewModel> SearchPart(WorkbookPart workbookPart, SheetData sheetData, string partName, Dictionary<string, string> columns)
        {
            List<AcademicDisciplineViewModel> data = new List<AcademicDisciplineViewModel>();

            foreach (Row row in sheetData.Elements<Row>())
            {
                AcademicDisciplineViewModel academicdiscipline = new AcademicDisciplineViewModel();
                bool needToAddAcademicDiscipline = false;
                string rowIndex = string.Empty;

                foreach (Cell cell in row.Elements<Cell>())
                {
                    string cellValue = GetCellValue(workbookPart, cell);

                    if (cellValue.ToLower() == partName)
                    {
                        break;
                    }
                    if (cellValue.ToLower().Contains(partName))
                    {
                        rowIndex = row.RowIndex;
                    }

                    if (row.RowIndex == rowIndex)
                    {
                        string columnName = GetColumnName(cell.CellReference);

                        if (columnName == columns["Индекс"])
                        {
                            academicdiscipline.Code = cellValue;
                        }
                        else if (columnName == columns["По плану"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.TotalOursOnPlan = value;
                        }
                        else if (columnName == columns["По ЗЕТ"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.SUTTotalOurs = value;
                        }
                        else if (columnName == columns["Экспертное"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.SUTExpert = value;
                        }
                        else if (columnName == columns["Факт"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.SUTFactual = value;
                        }
                        else if (columnName == columns["Итого часов в электронной форме"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.OursInElectronicalForm = value;
                        }
                        else if (columnName == columns["Итого часов в интерактивной форме"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.OursInInteractiveForm = value;
                        }
                        else if (columnName == columns["Контакт. раб."])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.ContactOurs = value;
                        }
                        else if (columnName == columns["СРС"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.IWOSOurs = value;
                        }
                        else if (columnName == columns["Контроль"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.ControlOurs = value;
                        }
                        else if (columnName == columns["Наименование"])
                        {
                            academicdiscipline.CourseTitle = cellValue;
                        }

                        needToAddAcademicDiscipline = string.Compare(columnName, columns["Компетенции"]) == 0;
                    }
                }

                if (needToAddAcademicDiscipline)
                {
                    data.Add(academicdiscipline);
                }
            }

            return data;
        }
        private static List<AcademicDisciplineViewModel> SearchOptionallyPart(WorkbookPart workbookPart, SheetData sheetData, Dictionary<string, string> columns)
        {
            string partName = "б1.в.дв";
            List<AcademicDisciplineViewModel> data = new List<AcademicDisciplineViewModel>();
            bool needToSearchPartNameAcademicDiscipline = false;

            foreach (Row row in sheetData.Elements<Row>())
            {
                AcademicDisciplineViewModel academicdiscipline = new AcademicDisciplineViewModel();
                academicdiscipline.Code = partName.ToUpper();
                bool needToAddAcademicDiscipline = false;
                string rowIndex = string.Empty;

                foreach (Cell cell in row.Elements<Cell>())
                {
                    string cellValue = GetCellValue(workbookPart, cell);
                    string columnName = GetColumnName(cell.CellReference);

                    if (cellValue.ToLower().Contains(partName))
                    {
                        needToSearchPartNameAcademicDiscipline = true;
                        break;
                    }
                    if(needToSearchPartNameAcademicDiscipline && columnName == columns["Наименование"] && !string.IsNullOrEmpty(cellValue))
                    {
                        rowIndex = row.RowIndex;
                        needToSearchPartNameAcademicDiscipline = false;
                    }
                    if (cellValue.ToLower().Contains("*"))
                    {
                        rowIndex = string.Empty;
                    }

                    if (row.RowIndex == rowIndex)
                    {
                        if (columnName == columns["Индекс"])
                        {
                            academicdiscipline.Code += !string.IsNullOrEmpty(cellValue) ? "." + cellValue : string.Empty;
                        }
                        else if (columnName == columns["По плану"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.TotalOursOnPlan = value;
                        }
                        else if (columnName == columns["По ЗЕТ"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.SUTTotalOurs = value;
                        }
                        else if (columnName == columns["Экспертное"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.SUTExpert = value;
                        }
                        else if (columnName == columns["Факт"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.SUTFactual = value;
                        }
                        else if (columnName == columns["Итого часов в электронной форме"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.OursInElectronicalForm = value;
                        }
                        else if (columnName == columns["Итого часов в интерактивной форме"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.OursInInteractiveForm = value;
                        }
                        else if (columnName == columns["Контакт. раб."])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.ContactOurs = value;
                        }
                        else if (columnName == columns["СРС"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.IWOSOurs = value;
                        }
                        else if (columnName == columns["Контроль"])
                        {
                            int.TryParse(cellValue, out int value);

                            academicdiscipline.ControlOurs = value;
                        }
                        else if (columnName == columns["Наименование"])
                        {
                            academicdiscipline.CourseTitle = cellValue;
                        }

                        needToAddAcademicDiscipline = string.Compare(columnName, columns["Компетенции"]) == 0;
                    }
                }

                if (needToAddAcademicDiscipline)
                {
                    data.Add(academicdiscipline);
                }
            }

            return data;
        }

        private static string GetCellValue(WorkbookPart workbookPart, Cell cell)
        {
            string value = string.Empty;
            if (cell != null)
            {
                value = cell.InnerText;

                if (cell.DataType != null)
                {
                    switch (cell.DataType.Value)
                    {
                        case CellValues.SharedString:

                            var stringTable =
                                workbookPart.GetPartsOfType<SharedStringTablePart>()
                                .FirstOrDefault();
                            if (stringTable != null)
                            {
                                value =
                                    stringTable.SharedStringTable
                                    .ElementAt(int.Parse(value)).InnerText;
                            }
                            break;

                        case CellValues.Boolean:
                            switch (value)
                            {
                                case "0":
                                    value = "FALSE";
                                    break;
                                default:
                                    value = "TRUE";
                                    break;
                            }
                            break;
                    }
                }
            }

            return value;
        }
        public static string GetColumnName(string cellReference)
        {
            string columnName = string.Empty;

            for(int i =0; i < cellReference.Length; i++)
            {
                string symbol = cellReference[i].ToString();
                if (int.TryParse(symbol, out int result))
                {
                    break;
                }

                columnName += symbol;
            }

            return columnName;
        }

        public static int GetRowIndex(string cellReference)
        {
            int rowIndex = -1;

            for (int i = 0; i < cellReference.Length; i++)
            {
                string subString = cellReference.Substring(i);
                if (!int.TryParse(subString, out rowIndex))
                {
                    continue;
                }
                break;
            }

            return rowIndex;
        }
    }
}