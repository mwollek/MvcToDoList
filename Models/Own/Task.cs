using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcToDoList.Models.Own
{
    public class Task
    {
        public readonly struct ProgressStates
        {
            public const int InProgress = 1;
            public const int Done = 2;
            public const int Confirmed = 3;
        }

        /*public enum ProgressStatesEnum : int
        {
            InProgress = 1,
            Done = 2,
            Confirmed = 3
        }*/

        public static Dictionary<int, string> ProgressStatesTextDict = new Dictionary<int, string>
        {
            { ProgressStates.InProgress, "in progress"},
            { ProgressStates.Done, "done"},
            { ProgressStates.Confirmed, "confirmed as done"}
        };

        // DB structure
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Task name is required")]
        [Display(Name = "Task name")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Planned finish date")]
        [Required(ErrorMessage = "Finish date is required")]
        [DataType(DataType.Date)]
        public DateTime PlannedFinishDate { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime? ActualFinishedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool? Missed { get; set; }

        public int ProgressState { get; set; } = ProgressStates.InProgress;

        // navigation props
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        

    }
}