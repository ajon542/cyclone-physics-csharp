using Cyclone.Math;
using UnityEngine;
using NUnit.Framework;

public class Matrix3Tests
{
    [Test]
    public void EditorTest()
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
}
