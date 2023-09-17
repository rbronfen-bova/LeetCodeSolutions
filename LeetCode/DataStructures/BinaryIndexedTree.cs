namespace LeetCode.DataStructures;

public static class BinaryIndexedTree
{
    public static BinaryIndexedTree<int> CreateIntegerSum(int size) =>
        new(size, 0, (a, b) => a + b);

    public static BinaryIndexedTree<int> CreateIntegerMultiplication(int size) =>
        new(size, 1, (a, b) => a * b);
}

public class BinaryIndexedTree<T>
{
    private readonly T _neutralElement;
    private readonly Func<T, T, T> _operation;
    private readonly T[] _tree;

    public BinaryIndexedTree(int size, T neutralElement, Func<T, T, T> operation)
    {
        _neutralElement = neutralElement;
        _operation = operation;
        _tree = Enumerable.Repeat(_neutralElement, size + 1).ToArray();
    }

    public T this[int index]
    {
        get
        {
            if (index < 0)
            {
                index %= _tree.Length;
                index += _tree.Length - 1;
            }
            var res = _neutralElement;
            for (index += 1; index > 0; index ^= index & -index)
            {
                res = _operation(res, _tree[index]);
            }
            return res;
        }
        set
        {
            if (index < 0)
            {
                index %= _tree.Length;
                index += _tree.Length - 1;
            }
            for (index += 1; index < _tree.Length; index += index & -index)
            {
                _tree[index] = _operation(_tree[index], value);
            }
        }
    }
}