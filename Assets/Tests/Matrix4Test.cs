using Cyclone.Math;
using UnityEngine;

/// <summary>
/// A class to validate the matrix4 implementation.
/// Is this the best way to do this, probably not, but
/// it's better than nothing.
/// </summary>
public class Matrix4Test : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Matrix4Test.Test1 " + (Test1() ? "Succeeded" : "Failed"));
        Debug.Log("Matrix4Test.Test2 " + (Test2() ? "Succeeded" : "Failed"));
        Debug.Log("Matrix4Test.Test3 " + (Test3() ? "Succeeded" : "Failed"));
    }

    /// <summary>
    /// Tests a valid matrix multiplication.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test1()
    {
        var m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );
        var m2 = new Matrix4
            (
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0
            );

        Matrix4 m3 = m1 * m2;

        Matrix4 expected = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );

        return m3 == expected;
    }

    /// <summary>
    /// Tests an invalid matrix multiplication.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test2()
    {
        var m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );
        var m2 = new Matrix4
            (
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0
            );

        Matrix4 m3 = m1 * m2;

        Matrix4 notExpected = new Matrix4
            (
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0
            );

        return m3 != notExpected;
    }

    private bool Test3()
    {
        var m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        var expected = new Matrix4
            (
            1.0, 0.0, 0.0, -1.0,
            -2.0, 0.0, 1.0, 1.0,
            4.0, 1.0, -2.0, -4.0
            );

        var result = m1.Inverse();

        return result == expected;
    }
}
