namespace hw_1;

public class Train
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Model { get; set; }
    public string Manufacture { get; set; }
    public string ManufactureYear { get; set; }
    public int Capacity { get; set; }
    public int MaxSpeed { get; set; }

    public static Train[] GetMockData() => new Train[]
    {
        new Train
        {
            Type = "Passenger", Model = "Siemens Velaro", Manufacture = "Siemens", ManufactureYear = "2006",
            Capacity = 600, MaxSpeed = 300
        },
        new Train
        {
            Type = "Freight", Model = "GE ES44AC", Manufacture = "GE Transportation", ManufactureYear = "2004",
            Capacity = 200, MaxSpeed = 120
        },
        new Train
        {
            Type = "High-Speed", Model = "TGV Duplex", Manufacture = "Alstom", ManufactureYear = "1996",
            Capacity = 1000, MaxSpeed = 320
        },
    };
}