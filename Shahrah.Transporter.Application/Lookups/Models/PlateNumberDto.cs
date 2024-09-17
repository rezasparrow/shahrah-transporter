namespace Shahrah.Transporter.Application.Lookups.Models;

public class PlateNumberDto
{
    public PlateNumberDto()
    { }

    public PlateNumberDto(string plateNumber)
    {
        FillPlateNumber(plateNumber);
    }

    public string PartOne { get; set; }
    public string PartTwo { get; set; }
    public string PartThree { get; set; }
    public string PartFour { get; set; }

    private void FillPlateNumber(string plateNumber)
    {
        PartOne = plateNumber.Substring(0, 2);
        if (plateNumber.Length == 8)
        {
            PartTwo = plateNumber.Substring(2, 1);
            PartThree = plateNumber.Substring(3, 3);
            PartFour = plateNumber.Substring(6, 2);
        }
        else
        {
            PartTwo = plateNumber.Substring(2, 3);
            PartThree = plateNumber.Substring(5, 3);
            PartFour = plateNumber.Substring(8, 2);
        }
    }

    public override string ToString()
    {
        var plateNo = new System.Text.StringBuilder();

        plateNo.Append(PartOne);
        plateNo.Append(PartTwo);
        plateNo.Append(PartThree);
        plateNo.Append(PartFour);
        return plateNo.ToString();
    }
}