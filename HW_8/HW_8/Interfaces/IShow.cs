using System.Numerics;

namespace HW_8.Interfaces;

public interface IShow<T> where T : INumber<T>
{
    T Id { get; set; }
    string Value { get; set; } 
}