using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Other
{
    public class Page
    {
        private const string accentColor = "accent-500";
        private const string primaryColor = "primary-500";
        public string HRef { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public Page()
        {
            HRef = string.Empty;
            Name = string.Empty;
            Text = string.Empty;
            Icon = string.Empty;
            Color = primaryColor;
        }

        public Page(string HRef, string Name, string Text, string Icon)
        {
            this.HRef = HRef;
            this.Name = Name;
            this.Text = Text;
            this.Icon = Icon;
            this.Color = primaryColor;
        }
    }
}
