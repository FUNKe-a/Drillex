using Godot;

public partial class Pair<T1, T2> : RefCounted
{
    public T1 First { get; set; }
    public T2 Second { get; set; }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }

    public Pair()
    {
        First = default(T1);
        Second = default(T2);
    }
}