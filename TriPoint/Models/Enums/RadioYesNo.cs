using System.ComponentModel.DataAnnotations;

namespace TriPoint.Models.Enums;

public enum RadioYesNo {
    [Display(Name = "Yes")] Yes = 1,

    [Display(Name = "No")] No = 2
}