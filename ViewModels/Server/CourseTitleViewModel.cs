﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public class CourseTitleViewModel : IKeyViewModel<Guid>, INameViewModel, IShortNameViewModel
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string DepartmentName { get; set; }
    }
}
