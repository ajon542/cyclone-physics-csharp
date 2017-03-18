using Cyclone.Math;
using UnityEngine;
using NUnit.Framework;

public class Matrix3Tests
{
    /// <summary>
    /// Tests the inverse of a matrix.
    /// </summary>
    [Test]
    public void Test1()
    {
        Matrix3 m1 = new Matrix3
            (
            1.0, 2.0, 3.0,
            0.0, 1.0, 4.0,
            5.0, 6.0, 0.0
            );

        Matrix3 inverse = new Matrix3();
        inverse.SetInverse(m1);

        Matrix3 expected = new Matrix3
            (
            -24.0, 18.0, 5.0,
            20.0, -15.0, -4.0,
            -5.0, 4.0, 1.0
            );

        Assert.AreEqual(expected, inverse);
    }

    /// <summary>
    /// Tests if a matrix multiplied by its inverse is equal to the identity matrix.
    /// </summary>
    [Test]
    public void Test2()
    {
        Matrix3 m1 = new Matrix3
            (
            1.0, 2.0, 3.0,
            0.0, 1.0, 4.0,
            5.0, 6.0, 0.0
            );

        Matrix3 inverse = new Matrix3();
        inverse.SetInverse(m1);

        Matrix3 identity = new Matrix3
            (
            1.0, 0.0, 0.0,
            0.0, 1.0, 0.0,
            0.0, 0.0, 1.0
            );

        Assert.AreEqual(identity, m1 * inverse);
    }

    /// <summary>
    /// Tests the different matrix inversion methods.
    /// </summary>
    [Test]
    public void Test3()
    {
        Matrix3 m1 = new Matrix3
            (
            1.0, 2.0, 3.0,
            0.0, 1.0, 4.0,
            5.0, 6.0, 0.0
            );

        Matrix3 inverse = new Matrix3();
        inverse.SetInverse(m1);

        m1.Invert();

        Assert.AreEqual(inverse, m1);
    }

    /// <summary>
    /// Tests the different matrix inversion methods.
    /// </summary>
    [Test]
    public void Test4()
    {
        Matrix3 m1 = new Matrix3
            (
            1.0, 2.0, 3.0,
            0.0, 1.0, 4.0,
            5.0, 6.0, 0.0
            );

        Matrix3 m1Original = new Matrix3
            (
            1.0, 2.0, 3.0,
            0.0, 1.0, 4.0,
            5.0, 6.0, 0.0
            );

        Matrix3 inverse = m1.Inverse();

        Matrix3 expected = new Matrix3
            (
            -24.0, 18.0, 5.0,
            20.0, -15.0, -4.0,
            -5.0, 4.0, 1.0
            );

        Assert.AreEqual(expected, inverse);
        Assert.AreEqual(m1Original, m1);
    }

    /// <summary>
    /// Tests if a multiply-equals (*=) results in the correct matrix.
    /// </summary>
    [Test]
    public void Test5()
    {
        Matrix3 m1 = new Matrix3
            (
            1.0, -2.0, 3.0,
            5.0, -6.0, 7.0,
            9.0, -10.0, 11.0
            );

        Matrix3 m2 = new Matrix3
            (
            1.0, -2.0, 3.0,
            5.0, -6.0, 7.0,
            9.0, -10.0, 11.0
            );

        Matrix3 result = m1 * m2;

        m1 *= m1;

        Assert.AreEqual(m1, result);
    }
}
