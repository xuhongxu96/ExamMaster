using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ExamMaster.Wpf.ViewModels
{
    public class AppModel 
    {
        public DocumentList DocumentList { get; } = new DocumentList("");
    }
}
