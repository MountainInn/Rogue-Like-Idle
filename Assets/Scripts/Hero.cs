using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Zenject;

[JsonObjectAttribute]
public class Hero : MonoBehaviour
{
    [JsonPropertyAttribute] public Expiriense expiriense {get; private set;}
    [JsonPropertyAttribute] public Level level {get; private set;}

    [Inject]
    [JsonPropertyAttribute] public Unit unit;

    public event Action<Hero> onHeroInitialized;

    private void Awake()
    {
        this.level = new Level(1);
        this.expiriense = new Expiriense(level, (level)=> level * 100);
        this.unit.level = level;

        onHeroInitialized?.Invoke(this);
    }

    static public implicit operator Unit(Hero hero) => hero.unit;
}
