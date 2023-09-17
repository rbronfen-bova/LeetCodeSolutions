using System.Collections;

namespace LeetCode.DataStructures;

public class DisjointSet<T> : IEnumerable<IGrouping<int, T>> where T : notnull
{
    private readonly IList<int> _roots = new List<int>();
    private readonly IDictionary<T, int> _tRoots = new Dictionary<T, int>();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<IGrouping<int, T>> GetEnumerator() =>
        _tRoots.GroupBy(pair => Find(pair.Key), pair => pair.Key).GetEnumerator();

    public int Find(T @object)
    {
        if (!_tRoots.TryGetValue(@object, out var root))
        {
            root = _roots.Count;
            _roots.Add(_roots.Count);
        }

        return _tRoots[@object] = FindInternal(root);
    }

    public bool AreConnected(T object1, T object2) =>
        Find(object1) == Find(object2);

    public void Union(T object1, T object2)
    {
        var root1 = Find(object1);
        var root2 = Find(object2);
        if (root1 == root2)
        {
            return;
        }
        _roots[root1] = root2;
    }

    public void Union(T object1, params T[] objects)
    {
        foreach (var object2 in objects)
        {
            Union(object1, object2);
        }
    }

    private int FindInternal(int root)
    {
        if (root != _roots[root])
        {
            _roots[root] = FindInternal(_roots[root]);
        }
        return _roots[root];
    }
}