using LeetCode.DataStructures;

namespace LeetCode.Tests.DataStructures;

[TestFixture]
public class DisjointSetTests
{
    [Test]
    public void Find_ForNewObject_ReturnsNewValues()
    {
        var disjointSet = new DisjointSet<string>();

        disjointSet.Find("First").Should().Be(0);
        disjointSet.Find("Second").Should().Be(1);
        disjointSet.Find("Third").Should().Be(2);
    }

    [Test]
    public void AreConnected_ForConnectedObjects_ReturnsTrue()
    {
        var disjointSet = new DisjointSet<string>();

        disjointSet.Union("First", "Second");
        disjointSet.Union("Second", "Third");
        disjointSet.Union("Third", "Fourth");
        disjointSet.Union("Fifth", "Fourth");

        disjointSet.AreConnected("First", "Fifth").Should().BeTrue();
    }

    [Test]
    public void Union_ForMultipleObjects_WorksTheSameAsUnionForCouples()
    {
        var disjointSet = new DisjointSet<string>();

        disjointSet.Union("First", "Second", "Third", "Fourth", "Fifth");

        disjointSet.AreConnected("First", "Fifth").Should().BeTrue();
        disjointSet.AreConnected("Second", "Fourth").Should().BeTrue();
        disjointSet.AreConnected("Second", "Sixth").Should().BeFalse();
    }

    [Test]
    public void AreConnected_ForDisconnectedObjects_ReturnsFalse()
    {
        var disjointSet = new DisjointSet<string>();

        disjointSet.AreConnected("First", "Second").Should().BeFalse();
    }

    [Test]
    public void Foreach_ReturnsObjectsGroupedByRoots()
    {
        var group1 = new[] { 0, 1, 2 };
        var group2 = new[] { 3, 4 };
        var group3 = new[] { 5, 6, 7, 8 };

        var disjointSet = new DisjointSet<int>();
        disjointSet.Union(group1[0], group1[1..]);
        disjointSet.Union(group2[0], group2[1..]);
        disjointSet.Union(group3[0], group3[1..]);

        var root1 = disjointSet.Find(group1[0]);
        var root2 = disjointSet.Find(group2[0]);
        var root3 = disjointSet.Find(group3[0]);

        var groups = disjointSet.ToList();
        groups.Should().ContainEquivalentOf(group1);
        groups.Should().ContainEquivalentOf(group2);
        groups.Should().ContainEquivalentOf(group3);

        groups.Select(_ => _.Key).Should().BeEquivalentTo(new[] { root1, root2, root3 });
    }
}