﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcToDoList.Models.ViewModels
{
    public class ConfirmedTaskViewModel : TaskViewModel
    {
        public int TotalDays { get; set; }
        public string ActualFinishDate { get; set; }
        public string OnTimeStats { get; set; }
        public string OnTimeStatusCollor { get; set; }


    }
}