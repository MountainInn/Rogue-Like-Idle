using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

public class ArithmeticChain
{
    private SortedList<int, ArithmeticNode> chain;

    public event Action onRecalculateChain;

    public float Result {get; private set;}


    public ArithmeticChain(int startCapacity)
    {
        chain = new SortedList<int, ArithmeticNode>(startCapacity);
    }

    public ArithmeticChain(int startCapacity, float rootValue) : this(startCapacity)
    {
        chain.Add(chain.Count, new ArithmeticNode(rootValue));
    }


    public void Add(int key, ArithmeticNode node)
    {
        node.onMutationUpdated += RecalculateChain;
       
        chain.Add(key, node);

        node.chain = this;

        RecalculateChain(node);
    }


    public void Remove(ArithmeticNode node)
    {
        node.onMutationUpdated -= RecalculateChain;

        int removedIndex = chain.IndexOfValue(node);

        chain.RemoveAt(removedIndex);

        node.chain = null;

        RecalculateChain(chain[removedIndex]);
    }


    public void RecalculateChain()
    {
        RecalculateChain(0);
    }

    public void RecalculateChain(ArithmeticNode startingFrom)
    {
        var changedIndex = chain.IndexOfValue(startingFrom);

        RecalculateChain(changedIndex);
    }

    public void RecalculateChain(int changedIndex)
    {
        if (changedIndex < 1)
            changedIndex = 1;

        ArithmeticNode prev, next;

        for (int i = changedIndex; i < chain.Count; i++)
        {
            prev = chain.Values[i-1];
            next = chain.Values[i];

            next.Mutate(prev.Result);
        }

        _result = next.Result;

        onRecalculateChain?.Invoke();
    }

    new public string ToString()
    {
        string output = string.Empty;

        foreach (var item in chain)
        {
            output += $"{item.Key:0000} }} {item.Value.ToString()}\n";
        }

        return output;
    }
}

[JsonObjectAttribute(MemberSerialization.OptIn)]
public class ArithmeticNode
{
    Arithm arithm;

    public float mutation { get; private set; }
    public float result { get; private set; }

    public event Action<ArithmeticNode> onMutationUpdated;

    [JsonPropertyAttribute]
    public float Mutation
    {
        get => mutation;
        set {
            mutation = value;
            onMutationUpdated?.Invoke(this);
        }
    }

    private ArithmeticNode(Arithm arithm, float mutation)
    {
        this.arithm = arithm;
        this.mutation = mutation;
    }

    private ArithmeticNode(float rootVal)
    {
        this.result = rootVal;
    }

    public void Mutate(float previousVal)
    {
        result = arithm.Mutate(previousVal, mutation);
    }

    new public string ToString()
    {
        return $"{( arithm == null ? "root" : arithm.ToString() )} {mutation} = {result}";
    }

    static public ArithmeticNode CreateRoot(float initialValue = 1) => new ArithmeticNode(initialValue);

    static public ArithmeticNode CreateMult(float mutation = 1) => new ArithmeticNode(new ArithmMult(), mutation);

    static public ArithmeticNode CreateAdd() => new ArithmeticNode(new ArithmAdd(), 0);

    static public ArithmeticNode CreateLimit(float limit) => new ArithmeticNode(new ArithmLimit(), limit);
}


abstract public class Arithm
{
    abstract public float Mutate(float previousVal, float mutation);

    new abstract public string ToString();
}


public class ArithmMult : Arithm
{
    public override float Mutate(float previousVal, float mutation)
    {
        return previousVal * mutation;
    }

    public override string ToString() => "*";
}

public class ArithmAdd : Arithm
{
    public override float Mutate(float previousVal, float mutation)
    {
        return previousVal + mutation;
    }

    public override string ToString() => "+";
}


public class ArithmLimit : Arithm
{
    public override float Mutate(float previousVal, float mutation)
    {
        if (mutation <= 0)
            return Mathf.Max(previousVal, mutation);

        else
            return Mathf.Min(previousVal, mutation);
    }

    public override string ToString() => "Limit";

}
