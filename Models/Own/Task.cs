using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcToDoList.Models.Own
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Task name is required")]
        [Display(Name = "Task name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Planned finish date")]
        [Required(ErrorMessage = "Finish date is required")]
        [DataType(DataType.Date)]
        public DateTime PlannedFinishDate { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public int ProgressState { get; set; } = (int)ProgressStatesEnum.InProgress;

        // navigation props
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public enum ProgressStatesEnum : int
        {
            InProgress = 1,
            Done = 2
        }

        public static Dictionary<int, string> ProgressStatesDict = new Dictionary<int, string>
        {
            { (int)ProgressStatesEnum.InProgress, "in progress"},
            { (int)ProgressStatesEnum.Done, "done"}
        };

    }
}