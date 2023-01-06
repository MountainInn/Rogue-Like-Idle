using System.Collections;
using System.Collections.Generic;

public class Node<T> : IEnumerable
{
    public T Value;
    public Node<T> parent;
    public List<Node<T>> children;

    protected (int , int) coordinates;

    public Node(T value)
    {
        Value = value;
    }

    public void SetValue(T Value)
    {
        this.Value = Value;
    }

    public void Add(Node<T> child)
    {
        children.Add(child);
    }
    public void Add(T child)
    {
        children.Add(new Node<T>(child));
    }

    public IEnumerator GetEnumerator()
    {
        return children.GetEnumerator();
    }

    public void ForEach(System.Action<Node<T>> action)
    {
        foreach(var item in children)
        {
            action.Invoke(item.Value);

            item.ForEach(action);
        }
    }

    public void ConstructTree()
    {
        (int x, int y) = coordinates;

        y++;

        foreach (var item in children)
        {
            item.coordinates = (x, y);

            item.ConstructTree();

            x++;
        }
    }

    public void SetViewPosition(Transform view, uint squareSize)
    {
        view.localPosition = new Vector3(coordinates.Item1, coordinates.Item2, 0) * squareSize;
    }

    static public implicit operator Node<T>(T Value) => new Node<T>(Value);
}
