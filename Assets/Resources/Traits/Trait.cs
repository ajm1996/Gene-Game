using UnityEngine;
using System;

public abstract class Trait : IComparable<Trait>
{
    public Color Color { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Abstract method to apply the trait effect
    public abstract void ApplyEffect(Drone drone);

    public int CompareTo(Trait other)
    {
        return other == null ? 1 : Id.CompareTo(other.Id);
    }
}