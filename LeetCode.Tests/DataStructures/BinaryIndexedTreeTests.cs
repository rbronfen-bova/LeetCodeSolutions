using LeetCode.DataStructures;

namespace LeetCode.Tests.DataStructures;

[TestFixture]
public class BinaryIndexedTreeTests
{
    [Test]
    public void UpdateSingleElement_AndFindPrefixSum_ReturnsThisElement()
    {
        var bit = BinaryIndexedTree.CreateIntegerSum(10);

        bit[5] = 12;

        bit[-1].Should().Be(12);
    }

    [Test]
    public void UpdateTwoElements_AndFindPrefixSum_ReturnsCorrectSum()
    {
        var bit = BinaryIndexedTree.CreateIntegerSum(10);

        bit[5] = 12;
        bit[4] = 8;

        bit[6].Should().Be(20);
    }

    [Test]
    public void UpdateTwoElements_AndFindFullSum_ReturnsCorrectSum()
    {
        var bit = BinaryIndexedTree.CreateIntegerSum(10);

        bit[5] = 12;
        bit[4] = 8;

        bit[-1].Should().Be(20);
    }

    [Test]
    public void UpdateManyElements_AndFindPrefixSum_ReturnsCorrectSum()
    {
        var bit = BinaryIndexedTree.CreateIntegerSum(10);

        bit[1] = 12;
        bit[4] = 8;
        bit[5] = 7;
        bit[6] = 15;

        bit[4].Should().Be(20);
    }

    [Test]
    public void UpdateTreeElements_NegativeIndexCornerCase_ReturnsCorrectSum()
    {
        var bit = BinaryIndexedTree.CreateIntegerSum(10);

        bit[4] = 8;
        bit[-2] = 6;
        bit[9] = 7;

        bit[-2].Should().Be(14);
    }

    [Test]
    public void UpdateManyElements_AndFindPrefixMultiplication_ReturnsCorrectMultiplication()
    {
        var bit = BinaryIndexedTree.CreateIntegerMultiplication(10);

        bit[1] = 12;
        bit[4] = 8;
        bit[5] = 7;
        bit[6] = 15;

        bit[5].Should().Be(672);
    }
}