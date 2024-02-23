using System.Collections;

namespace NextShip.Api.Utilities;

// form Nebula
public class StackFullCoroutine
{
    private readonly List<IEnumerator> stack = new();

    public StackFullCoroutine(IEnumerator enumerator)
    {
        stack.Add(enumerator);
    }

    public bool CanMove()
    {
        if (stack.Count == 0) return false;

        return stack.Count > 0;
    }

    public void MoveNext()
    {
        var current = stack[^1];
        if (!current.MoveNext())
            stack.RemoveAt(stack.Count - 1);
        else if (current.Current is IEnumerator child)
            stack.Add(child);
    }
}