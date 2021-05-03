using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcToDoList.Models.ViewModels
{
    public class TaskViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string DaysLeftMessage { get; set; }
        public string Status { get; set; }
        public string FinishDate { get; set; }

    }
}