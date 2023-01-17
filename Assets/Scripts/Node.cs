using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T> : IEnumerable
{
    public T Value;
    public Node<T> parent;
    public List<Node<T>> children;

    public (int, int) coordinates {get; protected set;}
    protected Vector2 position;

    public Node(T value)
    {
        Value = value;
        coordinates = (0,0);
        children = new List<Node<T>>();
    }

    public void SetValue(T Value)
    {
        this.Value = Value;
    }

    public void Add(T child)
    {
        Add(new Node<T>(child));
    }
    public void Add(Node<T> child)
    {
        child.parent = this;

        child.coordinates = (this.children.Count, this.coordinates.Item2 + 1);

        children.Add(child);
    }

    public IEnumerator GetEnumerator()
    {
        return children.GetEnumerator();
    }

    public void ForEach_BreadthFirst(System.Action<Node<T>> action)
    {
        Queue<Node<T>> childQueue = new Queue<Node<T>>();
        childQueue.Enqueue(this);

        while (childQueue.Peek() is not null)
        {
            var item = childQueue.Dequeue();

            action.Invoke(item);

            foreach (var child in item.children)
            {
                childQueue.Enqueue(child);
            }
        }
    }
    public void ForEach(System.Action<Node<T>> action)
    {
        action.Invoke(this);

        foreach (var item in children)
        {
            action.Invoke(item.Value);

            item.ForEach(action);
        }
    }

    public void ConstructTree(TalentView talentViewPrefab, Transform content, uint squareSize)
    {
        (int x, int y) = coordinates;
        position = new Vector2(x, y) * squareSize;

        int halfChildrenWidth = (parent is null) ? 0 : Mathf.CeilToInt(children.Count / 2f);

        var view = GameObject.Instantiate(talentViewPrefab, content).GetComponent<RectTransform>();

        view.anchoredPosition = position - new Vector2(halfChildrenWidth, 0);

        y++;

        foreach (var item in children)
        {
            item.coordinates = (x, y);

            item.ConstructTree(talentViewPrefab, content, squareSize);

            x++;
        }
    }

    static public implicit operator Node<T>(T Value) => new Node<T>(Value);
}
