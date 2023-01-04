using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Zenject;

[JsonObjectAttribute]
public class Hero : MonoBehaviour
{
    [JsonPropertyAttribute] public Expiriense expiriense {get; private set;}
    [JsonPropertyAttribute] public Unit unit {get; private set;}

    public event Action<Hero> onHeroInitialized;

    [Inject]
    private void Awake()
    {
        Level level = new Level(1);

        this.expiriense = new Expiriense(level, (level)=> level * 100);

        this.unit = new Unit(10, 10, null);
        this.unit.level = level;
        onHeroInitialized?.Invoke(this);
    }

    static public implicit operator Unit(Hero hero) => hero.unit;
}
