using Cyclone.Math;
using UnityEngine;
using NUnit.Framework;

public class Matrix4Tests
{
    /// <summary>
    /// Tests a valid matrix multiplication.
    /// </summary>
    [Test]
    public void Test1()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );

        Matrix4 m2 = new Matrix4();

        Matrix4 m3 = m1 * m2;

        Matrix4 expected = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );

        Assert.AreEqual(expected, m3);
    }

    /// <summary>
    /// Tests an invalid matrix multiplication.
    /// </summary>
    [Test]
    public void Test2()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );

        Matrix4 m2 = new Matrix4();

        Matrix4 m3 = m1 * m2;

        Matrix4 notExpected = new Matrix4
            (
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0
            );

        Assert.AreNotEqual(notExpected, m3);
    }

    /// <summary>
    /// Tests the determinant of a matrix.
    /// </summary>
    [Test]
    public void Test3()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        double det = m1.GetDeterminant();

        Assert.AreEqual(-1.0, det);
    }

    /// <summary>
    /// Tests the determinant of a matrix.
    /// </summary>
    [Test]
    public void Test4()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            5.0, 6.0, 7.0, 8.0,
            9.0, 10.0, 11.0, 12.0
            );

        double det = m1.GetDeterminant();
        Assert.AreEqual(0.0, det);
    }

    /// <summary>
    /// Tests the determinant of a matrix.
    /// </summary>
    [Test]
    public void Test5()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        double det = m1.GetDeterminant();

        Assert.AreEqual(-1.0, det);
    }

    /// <summary>
    /// Tests the inverse of a matrix.
    /// </summary>
    [Test]
    public void Test6()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        Matrix4 inverse = new Matrix4();
        inverse.SetInverse(m1);

        Matrix4 expected = new Matrix4
            (
            1.0, 0.0, 0.0, -1.0,
            -2.0, 0.0, 1.0, 1.0,
            4.0, 1.0, -2.0, -4.0
            );

        Assert.AreEqual(expected, inverse);
    }

    /// <summary>
    /// Tests if a matrix multiplied by its inverse is equal to the identity matrix.
    /// </summary>
    [Test]
    public void Test7()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        Matrix4 inverse = new Matrix4();
        inverse.SetInverse(m1);

        Matrix4 identity = new Matrix4();

        Assert.AreEqual(identity, m1 * inverse);
    }

    /// <summary>
    /// Tests the different matrix inversion methods.
    /// </summary>
    [Test]
    public void Test8()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        Matrix4 inverse = new Matrix4();
        inverse.SetInverse(m1);

        m1.Invert();

        Assert.AreEqual(inverse, m1);
    }

    /// <summary>
    /// Tests the different matrix inversion methods.
    /// </summary>
    [Test]
    public void Test9()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        Matrix4 m1Original = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        Matrix4 inverse = m1.Inverse();

        Matrix4 expected = new Matrix4
            (
            1.0, 0.0, 0.0, -1.0,
            -2.0, 0.0, 1.0, 1.0,
            4.0, 1.0, -2.0, -4.0
            );

        Assert.AreEqual(expected, inverse);
        Assert.AreEqual(m1Original, m1);
    }

    /// <summary>
    /// Tests if a multiply-equals (*=) results in the correct matrix.
    /// </summary>
    [Test]
    public void Test10()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, -2.0, 3.0, -4.0,
            5.0, -6.0, 7.0, -8.0,
            9.0, -10.0, 11.0, -12.0
            );

        Matrix4 m2 = new Matrix4
            (
            1.0, -2.0, 3.0, -4.0,
            5.0, -6.0, 7.0, -8.0,
            9.0, -10.0, 11.0, -12.0
            );

        Matrix4 result = m1 * m2;

        m1 *= m1;

        Assert.AreEqual(m1, result);
    }
}
