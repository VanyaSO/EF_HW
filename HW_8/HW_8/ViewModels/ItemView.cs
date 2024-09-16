using HW_8.Interfaces;

namespace HW_8.ViewModels;

public class ItemView : IShow<int>
{
    public int Id { get; set; }
    public string Value { get; set; }
}