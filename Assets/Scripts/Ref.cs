public class Ref<T>
    where T : unmanaged
{

    public T Value {get ; private set; }

    public Ref(T value)
    {
        Value = value;
    }

    static public implicit operator T(Ref<T> reft) => reft.Value;
    static public implicit operator Ref<T>(T t)
    {
        return new Ref<T>(t);
    }
}
