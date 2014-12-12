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
        Debug.Log("Matrix4Test.Test4 " + (Test4() ? "Succeeded" : "Failed"));
        Debug.Log("Matrix4Test.Test5 " + (Test5() ? "Succeeded" : "Failed"));
        Debug.Log("Matrix4Test.Test6 " + (Test6() ? "Succeeded" : "Failed"));
        Debug.Log("Matrix4Test.Test7 " + (Test7() ? "Succeeded" : "Failed"));
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
        Matrix4 m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );
        Matrix4 m2 = new Matrix4
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

    /// <summary>
    /// Tests the determinant of a matrix.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test3()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        double det = m1.GetDeterminant();

        return det == -1.0;
    }

    /// <summary>
    /// Tests the determinant of a matrix.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test4()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            5.0, 6.0, 7.0, 8.0,
            9.0, 10.0, 11.0, 12.0
            );

        double det = m1.GetDeterminant();

        return det == 0.0;
    }

    private bool Test5()
    {
        Matrix4 m1 = new Matrix4
            (
            1.0, 0.0, 0.0, 1.0,
            0.0, 2.0, 1.0, 2.0,
            2.0, 1.0, 0.0, 1.0
            );

        double det = m1.GetDeterminant();

        return det == -1.0;
    }

    private bool Test6()
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

        return inverse == expected;
    }

    private bool Test7()
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

        return identity == (m1 * inverse);
    }
}
