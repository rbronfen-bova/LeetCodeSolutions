namespace LeetCode.DataStructures;

public class Matrix
{
    private readonly long[,] _data;

    public Matrix(long[,] data)
    {
        _data = data;
    }

    public Matrix(Matrix other)
    {
        _data = new long[other.Height, other.Width];
        for (var i = 0; i < other.Height; i++)
        {
            for (var j = 0; j < other.Width; j++)
            {
                _data[i, j] += other[i, j];
            }
        }
    }

    public long this[int i, int j]
    {
        get => _data[i, j];
        set => _data[i, j] = value;
    }

    public int Width => _data.GetLength(1);

    public int Height => _data.GetLength(0);

    public static Matrix operator *(Matrix a, Matrix b)
    {
        var mod = 1000000007L;
        var data = new long[a.Height, b.Width];
        for (var i = 0; i < a.Height; i++)
        {
            for (var j = 0; j < b.Width; j++)
            {
                var t = 0L;
                for (var k = 0; k < a.Width; k++)
                {
                    t += a[i, k] * b[k, j] % mod;
                    t %= mod;
                }
                data[i, j] = t;
            }
        }
        return new(data);
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        var mod = 1000000007L;
        var data = new long[a.Height, a.Width];
        for (var i = 0; i < a.Height; i++)
        {
            for (var j = 0; j < b.Width; j++)
            {
                data[i, j] = (a[i, j] + b[i, j]) % mod;
            }
        }
        return new(data);
    }

    public long Sum()
    {
        var mod = 1000000007L;
        var res = 0L;
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                res = (res + _data[i, j]) % mod;
            }
        }
        return res;
    }

    public Matrix Pow(int power)
    {
        var cumulative = new Matrix(new long[Height, Width]);
        for (var i = 0; i < Height; i++)
        {
            cumulative[i, i] = 1;
        }
        var t = new Matrix(this);
        while (power > 0)
        {
            if (power % 2 == 1)
            {
                cumulative *= t;
            }
            t *= t;
            power /= 2;
        }
        return cumulative;
    }
}