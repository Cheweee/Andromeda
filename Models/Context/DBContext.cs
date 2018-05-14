namespace Andromeda.Models.Context
{
    using Andromeda.Models.Administration;
    using Andromeda.Models.Entities;
    using Andromeda.Models.Logs;
    using Andromeda.Models.References;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using WECr;

    /// <summary>
    /// Context of the database
    /// </summary>
    public partial class DBContext : DbContext
    {
        #region Properties
        public virtual DbSet<LogError> LogErrors { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Right> Rights { get; set; }
        public virtual DbSet<RightRole> RightRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<TypeOfEducation> TypesOfEducations { get; set; }
        public virtual DbSet<LevelOfHigherEducation> LevelsOfHigherEducation { get; set; }
        public virtual DbSet<TypeOfProject> TypesOfProject { get; set; }
        public virtual DbSet<CourseTitle> CourseTitles { get; set; }
        public virtual DbSet<AcademicTitle> AcademicTitles { get; set; }
        public virtual DbSet<AcademicDegree> AcademicDegrees { get; set; }
        public virtual DbSet<BranchOfScience> BranchesOfScience { get; set; }
        public virtual DbSet<Competence> Competences { get; set; }
        public virtual DbSet<AreaOfTraining> AreasOfTraining { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<AcademicDiscipline> AcademicDsciplines { get; set; }
        public virtual DbSet<WorkingCirriculum> WorkingCirriculums { get; set; }
        #endregion

        #region Constructors
        public DBContext() : base("name=DBContext")
        {
            Database.SetInitializer(new DefaultInitializer());
        }
        #endregion

        #region Functions
        public static DBContext Create()
        {
            return new DBContext();
        }
        #endregion
    }

    public class DefaultInitializer : CreateDatabaseIfNotExists<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            try
            {
                #region Права
                #region Права просмотра
                List<Right> viewRights = new List<Right> {
                new Right
                {
                    Id = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1"),
                    Name = "Просмотр справочников"
                },
                new Right
                {
                    Id = new Guid("E0AFAFE5-D9C8-40BE-AF1D-A2444DA1D38B"),
                    Name = "Просмотр информации сотрудников факультета"
                },
                new Right
                {
                    Id = new Guid("61683D42-2511-43A4-9C54-389BF6DD7F3A"),
                    Name = "Просмотр информации сотрудников кафедры"
                },
                new Right
                {
                    Id = new Guid("76C77E38-21FF-441D-8053-8F8915084B47"),
                    Name = "Просмотр информации о должностях факультета"
                },
                new Right
                {
                    Id = new Guid("B63143D4-88E5-44C9-A405-A06FAAA0CC06"),
                    Name = "Просмотр информации о должностях кафедры"
                },
                new Right
                {
                    Id = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5"),
                    Name = "Просмотр направлений подготовки"
                },
                new Right
                {
                    Id = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9"),
                    Name = "Просмотр рабочих планов"
                }
            };
                #endregion

                #region Права Администрирования
                List<Right> adminRights = new List<Right>
                {
                new Right
                {
                    Id = new Guid("BDD85C56-7790-42E0-B8AA-2304E81761F9"),
                    Name = "Администрирование"
                },
                new Right
                {
                    Id = new Guid("022594F5-A3CF-43E9-BC08-6678EC4A930D"),
                    Name = "Администрирование пользователей",
                },
                new Right
                {
                    Id = new Guid("5BE3F4FC-5C04-4C4C-BBB0-C4CB890408FB"),
                    Name = "Администрирование ролей",
                },
                new Right
                {
                    Id = new Guid("859F655B-4F8D-417C-AB54-CBE529373B81"),
                    Name = "Администрирование прав",
                }
                };
                #endregion

                #region Права редактирования
                List<Right> editRights = new List<Right>()
                {
                new Right
                {
                    Id = new Guid("6870BC22-BE78-48DC-A379-D833950721BF"),
                    Name = "Редактирование справочников"
                },
            new Right
                {
                    Id = new Guid("E9965941-EBB8-4FDE-84AA-F2BFC7871F06"),
                    Name = "Редактирование информации пользователей факульта"
                },
                new Right
                {
                    Id = new Guid("EA2FD1D1-F2A3-4CD7-A895-DDB9375FA279"),
                    Name = "Редактирование информации пользователей кафедры"
                },
                new Right
                {
                    Id = new Guid("91449349-05C9-4B8A-980B-0B3A5111E8C5"),
                    Name = "Редактирование информации о должностях факультета"
                },
                new Right
                {
                    Id = new Guid("E8396F8F-976B-4245-AF38-7B0307D433F9"),
                    Name = "Редактирование информации о должностях кафедры"
                },
                new Right
                {
                    Id = new Guid("324FED41-A69C-40BC-9630-6D197F717A99"),
                    Name = "Редактирование направлений подготовки кафедры"
                },
                new Right
                {
                    Id = new Guid("4B5FF91F-B937-428E-ADEC-9832FA7D9560"),
                    Name = "Редактирование рабочих планов кафедры"
                },
                new Right
                {
                    Id = new Guid("AB239C50-39E0-4CD1-B7A6-C63F8B87F1CE"),
                    Name = "Распределение нагрузки между преподавателями кафедры"
                }
                };
                #endregion
                context.Rights.AddRange(viewRights);
                context.Rights.AddRange(adminRights);
                context.Rights.AddRange(editRights);

                context.SaveChanges();
                #endregion

                #region Роли
                #region Деканат
                List<Role> deaneryRoles = new List<Role> {
                new Role
                {
                    Id = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),
                    Name = "Декан факультета"
                },
                new Role
                {
                    Id = new Guid("0FBD50CE-BAF3-4376-ACBF-24651DC79247"),
                    Name = "Заместитель декана по научной работе"
                },
                new Role
                {
                    Id = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),
                    Name = "Заместитель декана"
                }
            };
                #endregion
                #region ППС
                List<Role> professorsRoles = new List<Role> {
                new Role
                {
                    Id = new Guid("434800B0-3C75-4D6E-9375-C0BD8FDA6C30"),
                    Name = "Профессор",
                    CanTeach = true
                },
                new Role
                {
                    Id = new Guid("9BA6F885-DBDA-4755-A913-A678141FC951"),
                    Name = "Доцент",
                    CanTeach = true
                },
                new Role
                {
                    Id = new Guid("629AC417-9C60-4915-AB27-925ADF2EBB9C"),
                    Name = "Старший преподаватель",
                    CanTeach = true
                },
                new Role
                {
                    Id = new Guid("77C62DBF-4B7C-40F5-897D-A0A33A26FACF"),
                    Name = "Ассистент",
                    CanTeach = true
                }
            };
                #endregion
                #region Администраторы
                List<Role> adminRoles = new List<Role>
            {
                new Role
                {
                    Id = new Guid("556CAB08-1CC0-40E7-B665-4E59E59189E4"),
                    Name = "Администратор системы"
                },
                new Role
                {
                    Id = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),
                    Name = "Администратор контента"
                }
            };
                #endregion
                #region Сотрудники кафедры
                List<Role> departmentStaffRoles = new List<Role> {
                new Role
                {
                    Id = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),
                    Name = "Заведующий кафедрой"
                },
                new Role
                {
                    Id = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),
                    Name = "Инженер"
                },
                new Role
                {
                    Id = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),
                    Name = "Методист"
                }
            };
                #endregion
                context.Roles.AddRange(deaneryRoles);
                context.Roles.AddRange(professorsRoles);
                context.Roles.AddRange(adminRoles);
                context.Roles.AddRange(departmentStaffRoles);

                context.SaveChanges();
                #endregion

                #region Отрасли наук
                context.BranchesOfScience.AddRange(new List<BranchOfScience>
                {
                    new BranchOfScience
                    {
                        Id = new Guid("34E902C4-AEC0-4902-B670-82751A487BF2"),
                        Name = "Архитектурных наук",
                        ShortName = "Арх. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("E401E049-BA1F-4557-B068-FF0CDA626693"),
                        Name = "Биологических наук",
                        ShortName = "Биол. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("8FADF161-3D1A-4215-90DF-CB5EDEB2ACE9"),
                        Name = "Ветеринарных наук",
                        ShortName = "Вет. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("CEC0E614-31F0-42DF-AFCB-938B2D6A9731"),
                        Name = "Военных наук",
                        ShortName = "Воен. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("1B1E140B-278B-439A-9ED6-C24A7B29A603"),
                        Name = "Географических наук",
                        ShortName = "Геогр. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("72C969F6-407A-42C3-9DDA-4D85B2E82CFC"),
                        Name = "Геолого-минералогических наук",
                        ShortName = "Геол.-минерал. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("B4EFC302-D9D2-49A4-9DDF-497EF3B7C6FC"),
                        Name = "Искусствоведческих наук",
                        ShortName = "Искусств. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("4481EEDF-FC3C-4369-A400-37EA8D8DEEF9"),
                        Name = "Исторических наук",
                        ShortName = "Ист. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("E17E21F6-A52E-4203-AACB-EFA9E624A9AB"),
                        Name = "Культурологических наук",
                        ShortName = "Культ. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("DE41D40E-B94E-42D3-ADBE-8601D37FDB4C"),
                        Name = "Медицинских наук",
                        ShortName = "Мед. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("95E0ACEB-C1CD-466A-8351-C6505B0BD1E2"),
                        Name = "Педагогических наук",
                        ShortName = "Пед. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("E97323D4-2E70-429D-9B7C-0629C2C64869"),
                        Name = "Политических наук",
                        ShortName = "Полит. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("43381BC3-7866-44B6-AEFC-624410C40388"),
                        Name = "Психологических наук",
                        ShortName = "Психол. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("BFF254E3-92F3-411B-816B-102CFDBE7A59"),
                        Name = "Сельскохозяйственных наук",
                        ShortName = "С.-х. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("C6FB4FE4-726E-4BE3-9E4B-CC4B7B255145"),
                        Name = "Социологических наук",
                        ShortName = "Социол. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("9B09DA9A-4207-41B1-A448-CAA190F11BDF"),
                        Name = "Технических наук",
                        ShortName = "Техн. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("03640145-0327-4F35-83CB-221C60B652F1"),
                        Name = "Фармацевтических наук",
                        ShortName = "Фармацевт. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("D3366EC4-7A59-4291-99B2-347559291A84"),
                        Name = "Физико-математических наук",
                        ShortName = "Физ.-мат. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("9ED7A510-0838-4718-86AD-C06D1ED87923"),
                        Name = "Филологических наук",
                        ShortName = "Филол. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("096C39CC-7362-43E5-90B6-C3B33CD602AC"),
                        Name = "Философских наук",
                        ShortName = "Филос. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("7814F299-AB83-4CEC-A5C0-EEDA6A743C62"),
                        Name = "Химических наук",
                        ShortName = "Хим. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("8F65EAD1-4E30-48C1-81A0-1CA6882753FF"),
                        Name = "Экономических наук",
                        ShortName = "Экон. н."
                    },
                    new BranchOfScience
                    {
                        Id = new Guid("30DEA5D9-85DE-4B8F-B2BD-B523BCF17C25"),
                        Name = "Юридических наук",
                        ShortName = "Юрид. н."
                    },
                });
                #endregion

                #region Ученые степени
                foreach (BranchOfScience bos in context.BranchesOfScience.Local)
                {
                    context.AcademicDegrees.AddRange(new List<AcademicDegree>
                    {
                        new AcademicDegree()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Доктор",
                            ShortName = "Д.",
                            BranchOfScienceId = bos.Id
                        },
                        new AcademicDegree()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Кандидат",
                            ShortName = "К.",
                            BranchOfScienceId = bos.Id
                        }
                    });
                }
                #endregion

                #region Ученые звания
                context.AcademicTitles.AddRange(new List<AcademicTitle>
                {
                    new AcademicTitle
                    {
                        Id = new Guid("2DF29703-88E7-4D38-9E8F-70F76584AE07"),
                        Name = "Доцент",
                        ShortName = "Доц."
                    },
                    new AcademicTitle
                    {
                        Id = new Guid("18319FA0-74D7-4240-9CC6-06202A795729"),
                        Name = "Профессор",
                        ShortName = "Проф."
                    }
                });
                #endregion

                #region Уровни обучения

                context.LevelsOfHigherEducation.Add(new LevelOfHigherEducation { Name = "Бакалавриат" });
                context.LevelsOfHigherEducation.Add(new LevelOfHigherEducation { Name = "Магистратура" });
                context.LevelsOfHigherEducation.Add(new LevelOfHigherEducation { Name = "Специалитет" });
                context.LevelsOfHigherEducation.Add(new LevelOfHigherEducation { Name = "Аспирантура" });

                context.SaveChanges();
                #endregion

                #region Форма обучения

                context.TypesOfEducations.Add(new TypeOfEducation { Name = "Очная" });
                context.TypesOfEducations.Add(new TypeOfEducation { Name = "Заочная" });
                context.SaveChanges();
                #endregion

                #region Типы работ
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Лекция", ShortName = "Лек" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Лабораторная работа", ShortName = "Лаб" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Практическое занятие", ShortName = "Пр" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Семинар", ShortName = "Сем" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Самостоятельная работа студента", ShortName = "СРС" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Контроль", ShortName = "Контроль" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Экзамен", ShortName = "Экз" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Зачет", ShortName = "За" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Зачет с оценкой", ShortName = "ЗаО" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Лабораторная работа", ShortName = "Лаб" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Курсовой проект", ShortName = "КП" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Курсовая работа", ShortName = "КР" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Реферат", ShortName = "Реф" });
                context.TypesOfProject.Add(new TypeOfProject { Id = Guid.NewGuid(), Name = "Расчетно-графическая работа", ShortName = "РГР" });
                context.SaveChanges();
                #endregion

                #region Факультеты, институты и кафедры

                #region ФИТУ
                Department faculty = new Department
                {
                    Id = new Guid("6C3B8752-F8BB-43DA-886E-20E3146012CE"),
                    IsFaculty = true,
                    Name = "Факультет информационных технологий и управления",
                    ShortName = "ФИТУ"
                };
                context.Departments.Add(faculty);

                List<Department> departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Прикладная математика",
                    ShortName = "ПМ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Информационные и измерительные системы и технологии",
                    ShortName = "ИИСТ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Программное обеспечение вычислительной техники",
                    ShortName = "ПОВТ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Автоматика и Телемеханика",
                    ShortName = "АиТ"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ФИиОП
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Факультет инноватики и организации производства",
                    ShortName = "ФИиОП"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Теория государства и права и отечественная история",
                    ShortName = "ТГиПиОИ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Философия и право",
                    ShortName = "ФиП"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Социология и психология",
                    ShortName = "СиП"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Управление социальными и экономическими системами",
                    ShortName = "УСиЭС"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Производственный и инновационный менеджмент",
                    ShortName = "ПиИМ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Публично-правовые дисциплины",
                    ShortName = "ППД"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Юриспруденция",
                    ShortName = "Юр"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region МФ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Механический факультет",
                    ShortName = "МФ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Автомобили и транспортно-технологические комплексы",
                    ShortName = "АиТТК"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Международные логистические системы и комплексы",
                    ShortName = "МЛСиК"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Технология машиностроения",
                    ShortName = "ТМ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Технологические машины и оборудование",
                    ShortName = "ТМиО"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Подъемно-транспортные, строительные и дорожные машины",
                    ShortName = "ПТСиДМ"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region СФ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Строительный факультет",
                    ShortName = "СФ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Водное хозяйство, инженерные сети и защита окружающей среды",
                    ShortName = "ВХИСЗОС"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Промышленное, гражданское строительство, геотехника и фундаментостроение",
                    ShortName = "ПГСГиФ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Архитектура и дизайн",
                    ShortName = "АиД"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ТФ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Технологический факультет",
                    ShortName = "ТФ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Общая химия и технология силикатов",
                    ShortName = "ОХиТС"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Химические технологии",
                    ShortName = "ХТ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Стандартизация, сертификация и управление качеством",
                    ShortName = "ССиУК"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ЭФ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Энергетический факультет",
                    ShortName = "ЭФ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Электрические станции и электроэнергетические системы",
                    ShortName = "ЭСиЭЭС"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Теоретическая электротехника и электрооборудование",
                    ShortName = "ТЭиЭ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Тепловые электрические станции и теплотехника",
                    ShortName = "ТЭСиТ"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ЭМФ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Электромеханический факультет",
                    ShortName = "ЭМФ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Электроснабжение и электропривод",
                    ShortName = "ЭиЭ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Мехатроника и гидропневмоавтоматика",
                    ShortName = "МиГ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Электромеханика и электрические аппараты",
                    ShortName = "ЭиЭА"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ФГГиНГД
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Факультет геологии, горного и нефтегазового дела",
                    ShortName = "ФГГиНГД"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Нефтегазовые техника и технологии",
                    ShortName = "НТиТ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Горное дело",
                    ShortName = "ГД"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Прикладная геология",
                    ShortName = "ПГ"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region АФ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Агропромышленный факультет",
                    ShortName = "АФ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Возобновляемая и малая энергетика",
                    ShortName = "ВиМЭ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Автоматизация и роботизация агропромышленного комплекса и биосистемный инжиниринг",
                    ShortName = "АиРАКиБИ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Переработка и упаковка сельскохозяйственной продукции",
                    ShortName = "ПиУСП"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Гидротехнические сооружения и гидравлика",
                    ShortName = "ГСиГ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Информационные системы и технологии в агропромышленном комплексе",
                    ShortName = "ИСиТвАК"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ФОДО
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Факультет открытого и дистанционного обучения",
                    ShortName = "ФОДО"
                };
                context.Departments.Add(faculty);
                #endregion

                #region ИФИО
                faculty = new Department
                {
                    Id = new Guid("A006EC7E-A15C-49A5-B627-2F5781CC305A"),
                    IsFaculty = true,
                    Name = "Институт фундаментального инженерного образования",
                    ShortName = "ИФИО"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Иностранные языки",
                    ShortName = "ИЯ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Высшая математика",
                    ShortName = "ВМ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Информатика, инженерная и компьютерная графика",
                    ShortName = "ИИиКГ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Общеинженерные дисциплины",
                    ShortName = "ОД"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Физика и электроника",
                    ShortName = "ФиЭ"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ВИ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Военный институт",
                    ShortName = "ВИ"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Инженерные войск",
                    ShortName = "ИВ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Войска связи",
                    ShortName = "ВС"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Информационная безопасность",
                    ShortName = "ИБ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Воздушно-космические силы",
                    ShortName = "ВКС"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ИФВиС
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Институт физического воспитания и спорта",
                    ShortName = "ИФВиС"
                };
                context.Departments.Add(faculty);

                departments = new List<Department>
            {
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Общефизическая подготовка",
                    ShortName = "ОП"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Игровые виды спорта и единоборства",
                    ShortName = "ИВСиЕ"
                },
                new Department
                {
                    FacultyId = faculty.Id,
                    Id = Guid.NewGuid(),
                    IsFaculty = false,
                    Name = "Плавание и легкая атлетика",
                    ShortName = "ПиЛА"
                },
            };
                context.Departments.AddRange(departments);
                #endregion

                #region ИМО
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Институт международного образования",
                    ShortName = "ИМО"
                };
                context.Departments.Add(faculty);
                #endregion

                #region ИДО
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Институт дополнительного образования",
                    ShortName = "ИДО"
                };
                context.Departments.Add(faculty);
                #endregion

                #region ВШУ
                faculty = new Department
                {
                    Id = Guid.NewGuid(),
                    IsFaculty = true,
                    Name = "Высшая школа управления",
                    ShortName = "ВШУ"
                };
                context.Departments.Add(faculty);
                #endregion
                context.SaveChanges();
                #endregion

                #region Выделение прав ролям пользователей
                #region Выделение прав ролям деканата
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("E0AFAFE5-D9C8-40BE-AF1D-A2444DA1D38B") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("76C77E38-21FF-441D-8053-8F8915084B47") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("6870BC22-BE78-48DC-A379-D833950721BF") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("E9965941-EBB8-4FDE-84AA-F2BFC7871F06") },
                    new RightRole{ RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),RightId = new Guid("91449349-05C9-4B8A-980B-0B3A5111E8C5") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("0FBD50CE-BAF3-4376-ACBF-24651DC79247"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("0FBD50CE-BAF3-4376-ACBF-24651DC79247"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("0FBD50CE-BAF3-4376-ACBF-24651DC79247"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("E0AFAFE5-D9C8-40BE-AF1D-A2444DA1D38B") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("76C77E38-21FF-441D-8053-8F8915084B47") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("6870BC22-BE78-48DC-A379-D833950721BF") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("E9965941-EBB8-4FDE-84AA-F2BFC7871F06") },
                    new RightRole{ RoleId = new Guid("B6C64F20-36A4-4354-825E-224101282D13"),RightId = new Guid("91449349-05C9-4B8A-980B-0B3A5111E8C5") },
                });
                #endregion

                #region Выделение прав ролям кафедры
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("61683D42-2511-43A4-9C54-389BF6DD7F3A") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("B63143D4-88E5-44C9-A405-A06FAAA0CC06") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },

                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("6870BC22-BE78-48DC-A379-D833950721BF") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("EA2FD1D1-F2A3-4CD7-A895-DDB9375FA279") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("E8396F8F-976B-4245-AF38-7B0307D433F9") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("324FED41-A69C-40BC-9630-6D197F717A99") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("4B5FF91F-B937-428E-ADEC-9832FA7D9560") },
                    new RightRole{ RoleId = new Guid("6EA303EA-96CF-49E3-BB2E-56EB0ADC032C"),RightId = new Guid("AB239C50-39E0-4CD1-B7A6-C63F8B87F1CE") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },

                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("6870BC22-BE78-48DC-A379-D833950721BF") },
                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("324FED41-A69C-40BC-9630-6D197F717A99") },
                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("4B5FF91F-B937-428E-ADEC-9832FA7D9560") },
                    new RightRole{ RoleId = new Guid("3C40DC47-4D84-4D1F-BE5B-6E15E11A30D9"),RightId = new Guid("AB239C50-39E0-4CD1-B7A6-C63F8B87F1CE") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },

                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("6870BC22-BE78-48DC-A379-D833950721BF") },
                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("324FED41-A69C-40BC-9630-6D197F717A99") },
                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("4B5FF91F-B937-428E-ADEC-9832FA7D9560") },
                    new RightRole{ RoleId = new Guid("3B801C0A-C3BE-40B9-8A55-5FFB54A4E21D"),RightId = new Guid("AB239C50-39E0-4CD1-B7A6-C63F8B87F1CE") },
                });
                #endregion

                #region Выделение прав ролям администраторов
                foreach (Guid rightId in context.Rights.Select(o => o.Id))
                {
                    context.RightRoles.Add(new RightRole { RoleId = new Guid("556CAB08-1CC0-40E7-B665-4E59E59189E4"), RightId = rightId });
                }
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("E0AFAFE5-D9C8-40BE-AF1D-A2444DA1D38B") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("61683D42-2511-43A4-9C54-389BF6DD7F3A") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("76C77E38-21FF-441D-8053-8F8915084B47") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("B63143D4-88E5-44C9-A405-A06FAAA0CC06") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },

                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("6870BC22-BE78-48DC-A379-D833950721BF") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("E9965941-EBB8-4FDE-84AA-F2BFC7871F06") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("EA2FD1D1-F2A3-4CD7-A895-DDB9375FA279") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("91449349-05C9-4B8A-980B-0B3A5111E8C5") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("E8396F8F-976B-4245-AF38-7B0307D433F9") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("324FED41-A69C-40BC-9630-6D197F717A99") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("4B5FF91F-B937-428E-ADEC-9832FA7D9560") },
                    new RightRole{ RoleId = new Guid("4385812B-D4B9-4544-83F1-6B3C13E43D0B"),RightId = new Guid("AB239C50-39E0-4CD1-B7A6-C63F8B87F1CE") },
                });
                #endregion

                #region Выделение прав ролям ппс
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("434800B0-3C75-4D6E-9375-C0BD8FDA6C30"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("434800B0-3C75-4D6E-9375-C0BD8FDA6C30"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("434800B0-3C75-4D6E-9375-C0BD8FDA6C30"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("9BA6F885-DBDA-4755-A913-A678141FC951"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("9BA6F885-DBDA-4755-A913-A678141FC951"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("9BA6F885-DBDA-4755-A913-A678141FC951"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("629AC417-9C60-4915-AB27-925ADF2EBB9C"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("629AC417-9C60-4915-AB27-925ADF2EBB9C"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("629AC417-9C60-4915-AB27-925ADF2EBB9C"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                });
                context.RightRoles.AddRange(new List<RightRole>
                {
                    new RightRole{ RoleId = new Guid("77C62DBF-4B7C-40F5-897D-A0A33A26FACF"),RightId = new Guid("70D3CB1F-48BA-44C5-B551-3A19DE0F32C1") },
                    new RightRole{ RoleId = new Guid("77C62DBF-4B7C-40F5-897D-A0A33A26FACF"),RightId = new Guid("8753EF72-BE1B-48C1-996D-DFEF39C96DE5") },
                    new RightRole{ RoleId = new Guid("77C62DBF-4B7C-40F5-897D-A0A33A26FACF"),RightId = new Guid("0EF53537-45CD-4301-9DC0-F7082593C9F9") },
                });
                #endregion
                #endregion

                #region Выделение ролей факультетам, институтам и кафедрам
                foreach (Department dep in context.Departments.Local.Where(o => o.IsFaculty))
                {
                    dep.RolesInDepartment = deaneryRoles;
                }
                var departmentRoles = new List<Role>();
                departmentRoles.AddRange(departmentStaffRoles);
                departmentRoles.AddRange(professorsRoles);
                foreach (Department dep in context.Departments.Local.Where(o => !o.IsFaculty))
                {
                    dep.RolesInDepartment = departmentRoles;

                }
                context.SaveChanges();
                #endregion

                #region Создание пользователей
                Guid grinId = Guid.NewGuid();
                User grin = new User
                {
                    Id = grinId,
                    Login = "grinDV",
                    LastName = "Гринченков",
                    Password = Encryption.Encrypt("grinchDV"),
                    Patronimyc = "Валерьевич",
                    UserName = "Дмитрий",
                    UserRoles = new List<UserRoles>
                        {
                            new UserRoles()
                            {
                                UserId = grinId,
                                RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),
                                Departments = context.Departments.Local.Where(o=>o.Id == new Guid("6C3B8752-F8BB-43DA-886E-20E3146012CE")).ToList()
                            }
                        }
                };
                Guid kirpichenkovaId = new Guid();
                User kirpichenkova = new User
                {
                    Id = kirpichenkovaId,
                    Login = "kirpichenkova",
                    LastName = "Кирпиченкова",
                    Password = Encryption.Encrypt("ifioonelove"),
                    Patronimyc = "Валерьевна",
                    UserName = "Наталья",
                    UserRoles = new List<UserRoles>
                    {
                        new UserRoles()
                        {
                            UserId = kirpichenkovaId,
                            RoleId = new Guid("AE9F0C57-DBB3-455F-A578-91C4CDCA3D79"),
                            Departments = context.Departments.Local.Where(o=>o.Id == new Guid("A006EC7E-A15C-49A5-B627-2F5781CC305A")).ToList()
                        }
                    }
                };
                Guid kirillId = new Guid("099DC122-6E91-4281-BC30-1587730A1BE5");
                User kirill = new User
                {
                    Id = kirillId,
                    Login = "kni",
                    LastName = "Иванченко",
                    Password = Encryption.Encrypt("qwerty_123"),
                    Patronimyc = "Николаевич",
                    UserName = "Кирилл",
                    UserRoles = new List<UserRoles>
                        {
                            new UserRoles()
                            {
                                UserId = kirillId,
                                //DepartmentId = new Guid("6C3B8752-F8BB-43DA-886E-20E3146012CE"),
                                RoleId = new Guid("556CAB08-1CC0-40E7-B665-4E59E59189E4"),
                            }
                        }
                };
                context.Users.AddRange(new List<User>
                {
                    kirill,grin, kirpichenkova
                });
                #endregion
            }
            catch (Exception exc)
            {
                context.LogErrors.Add(new LogError {Date = DateTime.Today, Id = Guid.NewGuid(), Message = exc.Message, Method = "Initialize database" });
            }
            finally
            {
                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}
