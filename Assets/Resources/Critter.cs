using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Critter : MonoBehaviour
{
    public int health = 5;
    public int damage = 2;

    public List<Trait> Traits = new List<Trait>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Traits.Count == 0)
            Traits.Add(new Trait() { Id = 0, Color = Color.white});

        GetComponent<SpriteRenderer>().color = Traits[Traits.Count - 1].Color;
    }
}

public class Trait : IComparable<Trait>
{
    public Color Color;
    public int Id;

    public int CompareTo(Trait compareTrait)
    {
        if (compareTrait == null)
            return 1;
        else
            return this.Id.CompareTo(compareTrait.Id);
    }
}