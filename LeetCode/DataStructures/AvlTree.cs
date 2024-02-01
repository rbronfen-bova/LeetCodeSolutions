using System.Collections;

namespace LeetCode.DataStructures;

public class AvlTree : IEnumerable<int>
{
    private Node _root;

    public int Height => _root.Height;

    public int NodeCount => _root.NodeCount;

    public int TotalCount => _root.TotalCount;

    public long TotalSum => _root.TotalSum;

    public IEnumerator<int> GetEnumerator()
    {
        var nodes = GetNodes(_root);
        return nodes.Select(_ => _.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public bool Exists(int value) =>
        Search(value) != null;

    public int Count(int value) =>
        Search(value)?.Count ?? 0;

    public bool GetGreaterThanOrEqual(int value, out int res) =>
        GetValueOrDefault(GetGreaterThanOrEqual(_root, value), out res);

    public bool GetGreaterThan(int value, out int res) =>
        GetValueOrDefault(GetGreaterThan(_root, value), out res);

    public bool GetLessThanOrEqual(int value, out int res) =>
        GetValueOrDefault(GetLessThanOrEqual(_root, value), out res);

    public bool GetLessThan(int value, out int res) =>
        GetValueOrDefault(GetLessThan(_root, value), out res);

    public void Insert(int value)
    {
        if (_root == null)
        {
            _root = new(value);
            return;
        }
        Insert(null, _root, value);
    }

    public void Insert(IEnumerable<int> values)
    {
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public void Insert(params int[] values)
    {
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public void Delete(int value) =>
        Delete(null, _root, value);

    public long GetSumOfFirstValues(int n) =>
        GetSumOfFirstValues(_root, n);

    private long GetSumOfFirstValues(Node node, int n)
    {
        if (n == 0 || node == null)
        {
            return 0;
        }
        if (node.TotalCount <= n)
        {
            return node.TotalSum;
        }
        var res = 0L;
        res += GetSumOfFirstValues(node.Left, n);
        n = Math.Max(0, n - GetTotalCount(node.Left));
        res += (long)node.Value * Math.Min(node.Count, n);
        n = Math.Max(0, n - node.Count);
        res += GetSumOfFirstValues(node.Right, n);
        return res;
    }

    private IEnumerable<Node> GetNodes(Node node)
    {
        if (node.Left != null)
        {
            foreach (var left in GetNodes(node.Left))
            {
                yield return left;
            }
        }
        for (var i = 0; i < node.Count; i++)
        {
            yield return node;
        }
        if (node.Right != null)
        {
            foreach (var right in GetNodes(node.Right))
            {
                yield return right;
            }
        }
    }

    private bool GetValueOrDefault(Node node, out int value)
    {
        value = node?.Value ?? default;
        return node != null;
    }

    private Node GetGreaterThanOrEqual(Node node, int value)
    {
        if (node == null || node.Value == value)
        {
            return node;
        }
        if (node.Value < value)
        {
            return GetGreaterThanOrEqual(node.Right, value);
        }
        return GetGreaterThanOrEqual(node.Left, value) ?? node;
    }

    private Node GetGreaterThan(Node node, int value)
    {
        if (node == null)
        {
            return null;
        }
        if (node.Value <= value)
        {
            return GetGreaterThan(node.Right, value);
        }
        return GetGreaterThan(node.Left, value) ?? node;
    }

    private Node GetLessThanOrEqual(Node node, int value)
    {
        if (node == null || node.Value == value)
        {
            return node;
        }
        if (node.Value > value)
        {
            return GetLessThanOrEqual(node.Left, value);
        }
        return GetLessThanOrEqual(node.Right, value) ?? node;
    }

    private Node GetLessThan(Node node, int value)
    {
        if (node == null)
        {
            return null;
        }
        if (node.Value >= value)
        {
            return GetLessThan(node.Left, value);
        }
        return GetLessThan(node.Right, value) ?? node;
    }

    private Node Search(int value)
    {
        var node = _root;
        while (node != null)
        {
            if (node.Value == value)
            {
                return node;
            }
            node = value < node.Value ? node.Left : node.Right;
        }
        return null;
    }

    private void Insert(Node parent, Node node, int value)
    {
        if (node.Value == value)
        {
            node.Count++;
            node.TotalCount++;
            node.TotalSum += node.Value;
            return;
        }
        if (value < node.Value)
        {
            if (node.Left == null)
            {
                node.Left = new(value);
            }
            else
            {
                Insert(node, node.Left, value);
            }
        }
        else
        {
            if (node.Right == null)
            {
                node.Right = new(value);
            }
            else
            {
                Insert(node, node.Right, value);
            }
        }
        RecalculateNodeMetadata(node);
        Balance(parent, node);
    }

    private void Delete(Node parent, Node node, int value)
    {
        if (node == null)
        {
            return;
        }
        if (node.Value == value)
        {
            if (node.Count > 1)
            {
                node.Count--;
                node.TotalCount--;
                node.TotalSum -= node.Value;
                return;
            }
            if (node.Left == null || node.Right == null)
            {
                if (parent == null)
                {
                    _root = node.Left ?? node.Right;
                }
                else if (parent.Left == node)
                {
                    parent.Left = node.Left ?? node.Right;
                }
                else
                {
                    parent.Right = node.Left ?? node.Right;
                }
                return;
            }
            var candidate = GetRightmostNode(node.Left);
            Swap(candidate, node);
            Delete(node, node.Left, value);
            RecalculateNodeMetadata(node);
            Balance(parent, node);
            return;
        }
        Delete(node, value < node.Value ? node.Left : node.Right, value);
        RecalculateNodeMetadata(node);
        Balance(parent, node);
    }

    private void Swap(Node node1, Node node2)
    {
        (node1.Value, node2.Value) = (node2.Value, node1.Value);
        (node1.Count, node2.Count) = (node2.Count, node1.Count);
    }

    private void Balance(Node parent, Node node)
    {
        var balanceFactor = GetBalanceFactor(node);
        if (balanceFactor < -1)
        {
            var right = node.Right;
            if (GetHeight(right.Left) < GetHeight(right.Right))
            {
                RotateRight(parent, node);
            }
            else
            {
                RotateRightLeft(parent, node);
            }
        }
        if (balanceFactor > 1)
        {
            var left = node.Left;
            if (GetHeight(left.Left) > GetHeight(left.Right))
            {
                RotateLeft(parent, node);
            }
            else
            {
                RotateLeftRight(parent, node);
            }
        }
    }

    private void RotateRight(Node parent, Node node)
    {
        var right = node.Right;
        node.Right = right.Left;
        right.Left = node;
        if (parent == null)
        {
            _root = right;
        }
        else
        {
            if (parent.Left == node)
            {
                parent.Left = right;
            }
            else
            {
                parent.Right = right;
            }
        }
        RecalculateNodeMetadata(node);
        RecalculateNodeMetadata(right);
    }

    private void RotateLeft(Node parent, Node node)
    {
        var left = node.Left;
        node.Left = left.Right;
        left.Right = node;
        if (parent == null)
        {
            _root = left;
        }
        else
        {
            if (parent.Left == node)
            {
                parent.Left = left;
            }
            else
            {
                parent.Right = left;
            }
        }
        RecalculateNodeMetadata(node);
        RecalculateNodeMetadata(left);
    }

    private void RotateRightLeft(Node parent, Node node)
    {
        var right = node.Right;
        var rightLeft = right.Left;
        node.Right = rightLeft.Left;
        right.Left = rightLeft.Right;
        rightLeft.Left = node;
        rightLeft.Right = right;
        if (parent == null)
        {
            _root = rightLeft;
        }
        else
        {
            if (parent.Left == node)
            {
                parent.Left = rightLeft;
            }
            else
            {
                parent.Right = rightLeft;
            }
        }
        RecalculateNodeMetadata(node);
        RecalculateNodeMetadata(right);
        RecalculateNodeMetadata(rightLeft);
    }

    private void RotateLeftRight(Node parent, Node node)
    {
        var left = node.Left;
        var leftRight = left.Right;
        node.Left = leftRight.Right;
        left.Right = leftRight.Left;
        leftRight.Right = node;
        leftRight.Left = left;
        if (parent == null)
        {
            _root = leftRight;
        }
        else
        {
            if (parent.Left == node)
            {
                parent.Left = leftRight;
            }
            else
            {
                parent.Right = leftRight;
            }
        }
        RecalculateNodeMetadata(node);
        RecalculateNodeMetadata(left);
        RecalculateNodeMetadata(leftRight);
    }

    private int GetMin(Node node) =>
        GetLeftmostNode(node).Value;

    private int GetMax(Node node) =>
        GetRightmostNode(node).Value;

    private Node GetLeftmostNode(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    private Node GetRightmostNode(Node node)
    {
        while (node.Right != null)
        {
            node = node.Right;
        }
        return node;
    }

    private void RecalculateNodeMetadata(Node node)
    {
        RecalculateHeight(node);
        RecalculateNodeCount(node);
        RecalculateTotalCount(node);
        RecalculateTotalSum(node);
    }

    private void RecalculateHeight(Node node) =>
        node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;

    private void RecalculateNodeCount(Node node) =>
        node.NodeCount = 1 + GetNodeCount(node.Left) + GetNodeCount(node.Right);

    private void RecalculateTotalCount(Node node) =>
        node.TotalCount = node.Count + GetTotalCount(node.Left) + GetTotalCount(node.Right);

    private void RecalculateTotalSum(Node node) =>
        node.TotalSum = (long)node.Value * node.Count + GetTotalSum(node.Left) + GetTotalSum(node.Right);

    private int GetBalanceFactor(Node node) =>
        GetHeight(node.Left) - GetHeight(node.Right);

    private int GetHeight(Node node) =>
        node?.Height ?? 0;

    private int GetNodeCount(Node node) =>
        node?.NodeCount ?? 0;

    private int GetTotalCount(Node node) =>
        node?.TotalCount ?? 0;

    private long GetTotalSum(Node node) =>
        node?.TotalSum ?? 0L;

    private class Node
    {
        public Node(int value)
        {
            Value = value;
            TotalSum = value;
        }

        public int Value { get; set; }

        public int Count { get; set; } = 1;

        public int Height { get; set; } = 1;

        public int NodeCount { get; set; } = 1;

        public int TotalCount { get; set; } = 1;

        public long TotalSum { get; set; }

        public Node Left { get; set; }

        public Node Right { get; set; }
    }
}