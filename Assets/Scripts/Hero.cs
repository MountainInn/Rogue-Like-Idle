using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObjectAttribute]
public class Hero : MonoBehaviour
{
    [JsonPropertyAttribute] public Expiriense expiriense {get; private set;}
    [JsonPropertyAttribute] public Unit unit {get; private set;}

    private Awake()
    {
        Level level = new Level(1);
        this.unit.level = level;
        this.expiriense = new Expiriense(level, (level)=> level * 100);
    }

        this.unit = new Unit(10, 10, null);

    static public implicit operator Unit(Hero hero) => hero.unit;
}
