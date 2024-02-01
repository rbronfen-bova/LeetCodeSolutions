using LeetCode.DataStructures;

namespace LeetCode.Tests.DataStructures;

[TestFixture]
public class AvlTreeTests
{
    [Test]
    public void Insert_10_Elements_InOrder_ConstructsValidTree()
    {
        var tree = new AvlTree();
        tree.Insert(Enumerable.Range(1, 10));
        tree.NodeCount.Should().Be(10);
        tree.Height.Should().Be(4);
        foreach (var i in Enumerable.Range(1, 10))
        {
            tree.Exists(i).Should().BeTrue();
        }
        tree.Exists(11).Should().BeFalse();
    }

    [Test]
    public void Insert_10_Elements_ConstructsValidTree()
    {
        var tree = new AvlTree();
        tree.Insert(1, 2, 3, 5, 4, 7, 6, 10, 9, 8);
        tree.NodeCount.Should().Be(10);
        tree.Height.Should().Be(4);
        foreach (var i in Enumerable.Range(1, 10))
        {
            tree.Exists(i).Should().BeTrue();
        }
        tree.Exists(11).Should().BeFalse();
    }

    [Test]
    public void Insert_1000000_RandomElements_ConstructsValidTree()
    {
        var n = 100000;
        var tree = new AvlTree();
        var random = new Random();
        var randomNumbers = Enumerable.Range(0, n).Select(_ => random.Next()).ToList();
        tree.Insert(randomNumbers);
        tree.NodeCount.Should().BeLessThanOrEqualTo(n);
        tree.Height.Should().BeLessThanOrEqualTo(29);
        foreach (var randomNumber in randomNumbers)
        {
            tree.Exists(randomNumber).Should().BeTrue();
        }
    }

    [Test]
    public void Delete_An_Element_ConstructsValidTree()
    {
        var tree = new AvlTree();
        tree.Insert(Enumerable.Range(1, 10));
        tree.Delete(8);
        tree.NodeCount.Should().Be(9);
        tree.Height.Should().Be(4);
        foreach (var i in Enumerable.Range(1, 10).Except(new[] { 8 }))
        {
            tree.Exists(i).Should().BeTrue();
        }
        tree.Exists(8).Should().BeFalse();
    }


    [Test]
    public void Delete_An_Element_ThatWasAddedMultipleTimes_DecreasesItsCount()
    {
        var tree = new AvlTree();
        tree.Insert(1, 2, 3, 1, 1);
        tree.Count(1).Should().Be(3);
        tree.Delete(1);
        tree.Count(1).Should().Be(2);
    }

    [Test]
    public void GetGreaterThanOrEqual_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 3, 9, 7, 5, 11);
        tree.GetGreaterThanOrEqual(3, out var res).Should().BeTrue();
        res.Should().Be(3);
        tree.GetGreaterThanOrEqual(4, out res).Should().BeTrue();
        res.Should().Be(5);
        tree.GetGreaterThanOrEqual(12, out res).Should().BeFalse();
        res.Should().Be(default);
    }

    [Test]
    public void GetGreaterThan_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 3, 9, 7, 5, 11);
        tree.GetGreaterThan(3, out var res).Should().BeTrue();
        res.Should().Be(5);
        tree.GetGreaterThan(4, out res).Should().BeTrue();
        res.Should().Be(5);
        tree.GetGreaterThan(12, out res).Should().BeFalse();
        res.Should().Be(default);
    }

    [Test]
    public void GetLessThanOrEqual_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 3, 9, 7, 5, 11);
        tree.GetLessThanOrEqual(3, out var res).Should().BeTrue();
        res.Should().Be(3);
        tree.GetLessThanOrEqual(4, out res).Should().BeTrue();
        res.Should().Be(3);
        tree.GetLessThanOrEqual(0, out res).Should().BeFalse();
        res.Should().Be(default);
    }

    [Test]
    public void GetLessThan_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 3, 9, 7, 5, 11);
        tree.GetLessThan(3, out var res).Should().BeTrue();
        res.Should().Be(1);
        tree.GetLessThan(4, out res).Should().BeTrue();
        res.Should().Be(3);
        tree.GetLessThan(0, out res).Should().BeFalse();
        res.Should().Be(default);
    }

    [Test]
    public void TotalCount_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 2, 3, 1, 1);
        tree.TotalCount.Should().Be(5);
    }

    [Test]
    public void TotalSum_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 2, 3, 1, 1);
        tree.TotalSum.Should().Be(8);
        tree.Delete(2);
        tree.TotalSum.Should().Be(6);
        tree.Delete(1);
        tree.TotalSum.Should().Be(5);
        tree.Delete(3);
        tree.TotalSum.Should().Be(2);
        tree.Delete(3);
        tree.TotalSum.Should().Be(2);
    }

    [Test]
    public void GetSumOfFirstValues_ReturnsValidValue()
    {
        var tree = new AvlTree();
        tree.Insert(1, 2, 3, 1, 1);
        tree.GetSumOfFirstValues(3).Should().Be(3);
        tree.Delete(1);
        tree.GetSumOfFirstValues(3).Should().Be(4);
        tree.Delete(1);
        tree.GetSumOfFirstValues(3).Should().Be(6);

    }
}