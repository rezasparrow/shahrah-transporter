using Shahrah.Framework.Resources;

using System.ComponentModel.DataAnnotations;

namespace Shahrah.Transporter.Api.Models;

public class PlateNumberModel
{
    public PlateNumberModel()
    {
    }

    public PlateNumberModel(string plateNumber)
    {
        PartOne = plateNumber.Substring(0, 2);
        PartTwo = plateNumber.Substring(2, 1);
        PartThree = plateNumber.Substring(3, 3);
        PartFour = plateNumber.Substring(6, 2);
    }

    [RegularExpression("^[1-9][0-9]$", ErrorMessageResourceName = "PlateNumberFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PartOne { get; set; }

    [RegularExpression(@"^[ آابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهیئ\s]+$", ErrorMessageResourceName = "PlateNumberFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PartTwo { get; set; }

    [RegularExpression("^[1-9][1-9][0-9]$", ErrorMessageResourceName = "PlateNumberFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PartThree { get; set; }

    [RegularExpression("^[1-9][0-9]$", ErrorMessageResourceName = "PlateNumberFormatNotCorrect", ErrorMessageResourceType = typeof(ErrorMessageResource))]
    public string PartFour { get; set; }

    public string GetPlateNumber()
    {
        var plateNo = new System.Text.StringBuilder();

        plateNo.Append(PartOne);
        plateNo.Append(PartTwo);
        plateNo.Append(PartThree);
        plateNo.Append(PartFour);

        return plateNo.ToString();
    }
}