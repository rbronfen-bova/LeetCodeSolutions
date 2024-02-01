namespace LeetCode.DataStructures;

public static class SegmentTree
{
    public static SegmentTree<int> CreateIntegerMin(int[] array) =>
        new(array, int.MaxValue, (a, b) => a + b);
}

public class SegmentTree<T>
{
    private readonly T _neutralElement;
    private readonly Func<T, T, T> _operation;
    private readonly int _size;
    private readonly T[] _tree;

    public SegmentTree(int size, T neutralElement, Func<T, T, T> operation)
    {
        _size = size;
        _neutralElement = neutralElement;
        _operation = operation;
        _tree = new T[2 * size];
        Array.Fill(_tree, _neutralElement);
    }

    public SegmentTree(T[] array, T neutralElement, Func<T, T, T> operation)
        : this(array.Length, neutralElement, operation)
    {
        Build(array);
    }

    public T this[int left, int right]
    {
        get
        {
            var res = _neutralElement;

            for (left += _size, right += _size; left < right; left /= 2, right /= 2)
            {
                if (left % 2 == 1)
                {
                    res = _operation(res, _tree[left++]);
                }

                if (right % 2 == 1)
                {
                    res = _operation(res, _tree[--right]);
                }
            }

            return res;
        }
    }

    public T this[int index]
    {
        get =>
            _tree[index + _size];
        set
        {
            _tree[index + _size] = value;
            for (var i = index + _size; i > 1; i /= 2)
            {
                _tree[i / 2] = _operation(_tree[i], _tree[i ^ 1]);
            }
        }
    }

    public void Build(T[] array)
    {
        for (var i = 0; i < _size; i++)
        {
            _tree[i + _size] = array[i];
        }

        for (var i = _size - 1; i > 0; i--)
        {
            _tree[i] = _operation(_tree[2 * i], _tree[2 * i + 1]);
        }
    }
}