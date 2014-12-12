using Cyclone.Math;
using UnityEngine;

/// <summary>
/// A class to validate the matrix3 implementation.
/// Is this the best way to do this, probably not, but
/// it's better than nothing.
/// </summary>
public class Matrix3Test : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Matrix3Test.Test1 " + (Test1() ? "SUCCEEDED" : "FAILED"));
        Debug.Log("Matrix3Test.Test2 " + (Test2() ? "SUCCEEDED" : "FAILED"));
        Debug.Log("Matrix3Test.Test3 " + (Test3() ? "SUCCEEDED" : "FAILED"));
        Debug.Log("Matrix3Test.Test4 " + (Test4() ? "SUCCEEDED" : "FAILED"));
        Debug.Log("Matrix3Test.Test5 " + (Test5() ? "SUCCEEDED" : "FAILED"));
    }

    /// <summary>
    /// Tests the inverse of a matrix.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test1()
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

        return inverse == expected;
    }

    /// <summary>
    /// Tests if a matrix multiplied by its inverse is equal to the identity matrix.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test2()
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

        return identity == (m1 * inverse);
    }

    /// <summary>
    /// Tests the different matrix inversion methods.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test3()
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

        return inverse == m1;
    }

    /// <summary>
    /// Tests the different matrix inversion methods.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test4()
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

        return (m1 == m1Original) && (inverse == expected);
    }

    /// <summary>
    /// Tests if a multiply-equals (*=) results in the correct matrix.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test5()
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

        return (result == m1);
    }
}
