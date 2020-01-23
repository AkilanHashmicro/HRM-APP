using System;
using System.Collections.Generic;
using System.Text;

namespace HRMAPP.Model
{
    public class MasterPageItems
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TypeTarget { get; set; }
        public MasterPageItems(string title, string icon, Type typetarget)
        {
            Title = title;
            Icon = icon;
            TypeTarget = typetarget;
        }
    }
}
