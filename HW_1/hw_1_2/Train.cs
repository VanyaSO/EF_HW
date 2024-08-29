using System;
using System.Collections.Generic;

namespace hw_1_2;

public partial class Train
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Manufacture { get; set; } = null!;

    public string ManufactureYear { get; set; } = null!;

    public int Capacity { get; set; }

    public int MaxSpeed { get; set; }
}
