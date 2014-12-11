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

        Matrix4 m3 = m1*m2;

        Matrix4 expected = new Matrix4
            (
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0,
            1.0, 2.0, 3.0, 4.0
            );
        bool result = (m3 == expected);
        Debug.Log(result);
    }
}
