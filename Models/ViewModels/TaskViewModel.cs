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

        // number of days left for deadline or message: "missed" if deadline is missed
        public string DaysLeftMessage { get; set; }
        public string Status { get; set; }
        public string PlannedFinishDate { get; set; }
        public string ModifyButtonLinkText { get; set; }
        public string ButtonStyle { get; set; }
        public string ThumbnailBackgroundColour { get; set; }




    }
}