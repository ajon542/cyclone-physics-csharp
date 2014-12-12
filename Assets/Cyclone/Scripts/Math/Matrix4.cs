using System;
using System.Collections.Generic;
using System.Linq;

namespace Cyclone.Math
{
    /// <summary>
    /// Holds a transform matrix, consisting of a rotation matrix and
    /// a position.
    /// </summary>
    public class Matrix4
    {
        /// <summary>
        /// The matrix has 12 elements, it is assumed that the
        /// remaining four are (0,0,0,1); producing a homogenous matrix.
        /// </summary>
        private const int ElementCount = 12;

        /// <summary>
        /// Gets the matrix data.
        /// </summary>
        public double[] Data { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Matrix4"/> class.
        /// </summary>
        public Matrix4()
        {
            Data = new double[ElementCount];
            Data[0] = Data[5] = Data[10] = 1;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Matrix4"/> class.
        /// </summary>
        /// <param name="m0">Matrix data.</param>
        /// <param name="m1">Matrix data.</param>
        /// <param name="m2">Matrix data.</param>
        /// <param name="m3">Matrix data.</param>
        /// <param name="m4">Matrix data.</param>
        /// <param name="m5">Matrix data.</param>
        /// <param name="m6">Matrix data.</param>
        /// <param name="m7">Matrix data.</param>
        /// <param name="m8">Matrix data.</param>
        /// <param name="m9">Matrix data.</param>
        /// <param name="m10">Matrix data.</param>
        /// <param name="m11">Matrix data.</param>
        public Matrix4
            (
            double m0, double m1, double m2, double m3,
            double m4, double m5, double m6, double m7,
            double m8, double m9, double m10, double m11
            )
        {
            Data = new[] { m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11 };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Matrix4"/> class.
        /// </summary>
        /// <param name="values">Matrix data.</param>
        public Matrix4(List<double> values)
        {
            if (values.Count != ElementCount)
            {
                throw new Exception("not enough values to initialize this matrix");
            }
            Data = values.ToArray();
        }

        /// <summary>
        /// Sets the matrix to be a diagonal matrix with the given coefficients.
        /// </summary>
        /// <param name="a">Matrix diagonal data.</param>
        /// <param name="b">Matrix diagonal data.</param>
        /// <param name="c">Matrix diagonal data.</param>
        public void SetDiagonal(double a, double b, double c)
        {
            Data[0] = a;
            Data[5] = b;
            Data[10] = c;
        }

        public static Matrix4 operator *(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 result = new Matrix4();
            result.Data[0] = (rhs.Data[0] * lhs.Data[0]) + (rhs.Data[4] * lhs.Data[1]) + (rhs.Data[8] * lhs.Data[2]);
            result.Data[4] = (rhs.Data[0] * lhs.Data[4]) + (rhs.Data[4] * lhs.Data[5]) + (rhs.Data[8] * lhs.Data[6]);
            result.Data[8] = (rhs.Data[0] * lhs.Data[8]) + (rhs.Data[4] * lhs.Data[9]) + (rhs.Data[8] * lhs.Data[10]);

            result.Data[1] = (rhs.Data[1] * lhs.Data[0]) + (rhs.Data[5] * lhs.Data[1]) + (rhs.Data[9] * lhs.Data[2]);
            result.Data[5] = (rhs.Data[1] * lhs.Data[4]) + (rhs.Data[5] * lhs.Data[5]) + (rhs.Data[9] * lhs.Data[6]);
            result.Data[9] = (rhs.Data[1] * lhs.Data[8]) + (rhs.Data[5] * lhs.Data[9]) + (rhs.Data[9] * lhs.Data[10]);

            result.Data[2] = (rhs.Data[2] * lhs.Data[0]) + (rhs.Data[6] * lhs.Data[1]) + (rhs.Data[10] * lhs.Data[2]);
            result.Data[6] = (rhs.Data[2] * lhs.Data[4]) + (rhs.Data[6] * lhs.Data[5]) + (rhs.Data[10] * lhs.Data[6]);
            result.Data[10] = (rhs.Data[2] * lhs.Data[8]) + (rhs.Data[6] * lhs.Data[9]) + (rhs.Data[10] * lhs.Data[10]);

            result.Data[3] = (rhs.Data[3] * lhs.Data[0]) + (rhs.Data[7] * lhs.Data[1]) + (rhs.Data[11] * lhs.Data[2]) + lhs.Data[3];
            result.Data[7] = (rhs.Data[3] * lhs.Data[4]) + (rhs.Data[7] * lhs.Data[5]) + (rhs.Data[11] * lhs.Data[6]) + lhs.Data[7];
            result.Data[11] = (rhs.Data[3] * lhs.Data[8]) + (rhs.Data[7] * lhs.Data[9]) + (rhs.Data[11] * lhs.Data[10]) + lhs.Data[11];

            return result;
        }

        public static Vector3 operator *(Matrix4 lhs, Vector3 vector)
        {
            return new Vector3
                (
                vector.x * lhs.Data[0] +
                vector.y * lhs.Data[1] +
                vector.z * lhs.Data[2] + lhs.Data[3],

                vector.x * lhs.Data[4] +
                vector.y * lhs.Data[5] +
                vector.z * lhs.Data[6] + lhs.Data[7],

                vector.x * lhs.Data[8] +
                vector.y * lhs.Data[9] +
                vector.z * lhs.Data[10] + lhs.Data[11]
                );
        }

        public Vector3 Transform(Vector3 vector)
        {
            return this * vector;
        }

        public double GetDeterminant()
        {
            return -Data[8] * Data[5] * Data[2] +
                    Data[4] * Data[9] * Data[2] +
                    Data[8] * Data[1] * Data[6] -
                    Data[0] * Data[9] * Data[6] -
                    Data[4] * Data[1] * Data[10] +
                    Data[0] * Data[5] * Data[10];
        }

        // 0, 1, 2, 3
        // 4, 5, 6, 7
        // 8, 9,10,11
        //12,13,14,15
        // 12 = 0
        // 13 = 0
        // 14 = 0
        // 15 = 1
        public void SetInverse2(Matrix4 m)
        {
            // Make sure the determinant is non-zero.
            double det = GetDeterminant();
            if (Core.Equals(det, 0.0))
            {
                return;
            }
            det = (1.0) / det;

            Data[0] =
                m.Data[5] * m.Data[10] * 1 -
                m.Data[5] * m.Data[11] * 0 -
                m.Data[9] * m.Data[6] * 1 +
                m.Data[9] * m.Data[7] * 0 +
                   0 * m.Data[6] * m.Data[11] -
                   0 * m.Data[7] * m.Data[10];

            Data[4] =
               -m.Data[4] * m.Data[10] * 1 +
                m.Data[4] * m.Data[11] * 0 +
                m.Data[8] * m.Data[6] * 1 -
                m.Data[8] * m.Data[7] * 0 -
                   0 * m.Data[6] * m.Data[11] +
                   0 * m.Data[7] * m.Data[10];

            Data[8] =
                m.Data[4] * m.Data[9] * 1 -
                m.Data[4] * m.Data[11] * 0 -
                m.Data[8] * m.Data[5] * 1 +
                m.Data[8] * m.Data[7] * 0 +
                   0 * m.Data[5] * m.Data[11] -
                   0 * m.Data[7] * m.Data[9];

            Data[1] =
               -m.Data[1] * m.Data[10] * 1 +
                m.Data[1] * m.Data[11] * 0 +
                m.Data[9] * m.Data[2] * 1 -
                m.Data[9] * m.Data[3] * 0 -
                0 * m.Data[2] * m.Data[11] +
                0 * m.Data[3] * m.Data[10];

            Data[5] = m.Data[0] * m.Data[10] * 1 -
             m.Data[0] * m.Data[11] * 0 -
             m.Data[8] * m.Data[2] * 1 +
             m.Data[8] * m.Data[3] * 0 +
             0 * m.Data[2] * m.Data[11] -
             0 * m.Data[3] * m.Data[10];

            Data[9] =
                -m.Data[0] * m.Data[9] * 1 +
              m.Data[0] * m.Data[11] * 0 +
              m.Data[8] * m.Data[1] * 1 -
              m.Data[8] * m.Data[3] * 0 -
              0 * m.Data[1] * m.Data[11] +
              0 * m.Data[3] * m.Data[9];

            Data[2] =
                m.Data[1] * m.Data[6] * 1 -
             m.Data[1] * m.Data[7] * 0 -
             m.Data[5] * m.Data[2] * 1 +
             m.Data[5] * m.Data[3] * 0 +
             0 * m.Data[2] * m.Data[7] -
             0 * m.Data[3] * m.Data[6];

            Data[6] =
                -m.Data[0] * m.Data[6] * 1 +
              m.Data[0] * m.Data[7] * 0 +
              m.Data[4] * m.Data[2] * 1 -
              m.Data[4] * m.Data[3] * 0 -
              0 * m.Data[2] * m.Data[7] +
              0 * m.Data[3] * m.Data[6];

            Data[10] =
                m.Data[0] * m.Data[5] * 1 -
              m.Data[0] * m.Data[7] * 0 -
              m.Data[4] * m.Data[1] * 1 +
              m.Data[4] * m.Data[3] * 0 +
              0 * m.Data[1] * m.Data[7] -
              0 * m.Data[3] * m.Data[5];

            Data[3] = -m.Data[1] * m.Data[6] * m.Data[11] +
          m.Data[1] * m.Data[7] * m.Data[10] +
          m.Data[5] * m.Data[2] * m.Data[11] -
          m.Data[5] * m.Data[3] * m.Data[10] -
          m.Data[9] * m.Data[2] * m.Data[7] +
          m.Data[9] * m.Data[3] * m.Data[6];

            Data[7] = m.Data[0] * m.Data[6] * m.Data[11] -
                     m.Data[0] * m.Data[7] * m.Data[10] -
                     m.Data[4] * m.Data[2] * m.Data[11] +
                     m.Data[4] * m.Data[3] * m.Data[10] +
                     m.Data[8] * m.Data[2] * m.Data[7] -
                     m.Data[8] * m.Data[3] * m.Data[6];

            Data[11] = -m.Data[0] * m.Data[5] * m.Data[11] +
                       m.Data[0] * m.Data[7] * m.Data[9] +
                       m.Data[4] * m.Data[1] * m.Data[11] -
                       m.Data[4] * m.Data[3] * m.Data[9] -
                       m.Data[8] * m.Data[1] * m.Data[7] +
                       m.Data[8] * m.Data[3] * m.Data[5];

            for (int i = 0; i < ElementCount; i++)
                Data[i] *= det;
        }

        /*bool gluInvertMatrix(const double m[16], double invOut[16])
{
    double inv[16], det;
    int i;

    inv[0] = m[5]  * m[10] * m[15] - 
             m[5]  * m[11] * m[14] - 
             m[9]  * m[6]  * m[15] + 
             m[9]  * m[7]  * m[14] +
             m[13] * m[6]  * m[11] - 
             m[13] * m[7]  * m[10];

    inv[4] = -m[4]  * m[10] * m[15] + 
              m[4]  * m[11] * m[14] + 
              m[8]  * m[6]  * m[15] - 
              m[8]  * m[7]  * m[14] - 
              m[12] * m[6]  * m[11] + 
              m[12] * m[7]  * m[10];

    inv[8] = m[4]  * m[9] * m[15] - 
             m[4]  * m[11] * m[13] - 
             m[8]  * m[5] * m[15] + 
             m[8]  * m[7] * m[13] + 
             m[12] * m[5] * m[11] - 
             m[12] * m[7] * m[9];

    inv[12] = -m[4]  * m[9] * m[14] + 
               m[4]  * m[10] * m[13] +
               m[8]  * m[5] * m[14] - 
               m[8]  * m[6] * m[13] - 
               m[12] * m[5] * m[10] + 
               m[12] * m[6] * m[9];

    inv[1] = -m[1]  * m[10] * m[15] + 
              m[1]  * m[11] * m[14] + 
              m[9]  * m[2] * m[15] - 
              m[9]  * m[3] * m[14] - 
              m[13] * m[2] * m[11] + 
              m[13] * m[3] * m[10];

    inv[5] = m[0]  * m[10] * m[15] - 
             m[0]  * m[11] * m[14] - 
             m[8]  * m[2] * m[15] + 
             m[8]  * m[3] * m[14] + 
             m[12] * m[2] * m[11] - 
             m[12] * m[3] * m[10];

    inv[9] = -m[0]  * m[9] * m[15] + 
              m[0]  * m[11] * m[13] + 
              m[8]  * m[1] * m[15] - 
              m[8]  * m[3] * m[13] - 
              m[12] * m[1] * m[11] + 
              m[12] * m[3] * m[9];

    inv[13] = m[0]  * m[9] * m[14] - 
              m[0]  * m[10] * m[13] - 
              m[8]  * m[1] * m[14] + 
              m[8]  * m[2] * m[13] + 
              m[12] * m[1] * m[10] - 
              m[12] * m[2] * m[9];

    inv[2] = m[1]  * m[6] * m[15] - 
             m[1]  * m[7] * m[14] - 
             m[5]  * m[2] * m[15] + 
             m[5]  * m[3] * m[14] + 
             m[13] * m[2] * m[7] - 
             m[13] * m[3] * m[6];

    inv[6] = -m[0]  * m[6] * m[15] + 
              m[0]  * m[7] * m[14] + 
              m[4]  * m[2] * m[15] - 
              m[4]  * m[3] * m[14] - 
              m[12] * m[2] * m[7] + 
              m[12] * m[3] * m[6];

    inv[10] = m[0]  * m[5] * m[15] - 
              m[0]  * m[7] * m[13] - 
              m[4]  * m[1] * m[15] + 
              m[4]  * m[3] * m[13] + 
              m[12] * m[1] * m[7] - 
              m[12] * m[3] * m[5];

    inv[14] = -m[0]  * m[5] * m[14] + 
               m[0]  * m[6] * m[13] + 
               m[4]  * m[1] * m[14] - 
               m[4]  * m[2] * m[13] - 
               m[12] * m[1] * m[6] + 
               m[12] * m[2] * m[5];

    inv[3] = -m[1] * m[6] * m[11] + 
              m[1] * m[7] * m[10] + 
              m[5] * m[2] * m[11] - 
              m[5] * m[3] * m[10] - 
              m[9] * m[2] * m[7] + 
              m[9] * m[3] * m[6];

    inv[7] = m[0] * m[6] * m[11] - 
             m[0] * m[7] * m[10] - 
             m[4] * m[2] * m[11] + 
             m[4] * m[3] * m[10] + 
             m[8] * m[2] * m[7] - 
             m[8] * m[3] * m[6];

    inv[11] = -m[0] * m[5] * m[11] + 
               m[0] * m[7] * m[9] + 
               m[4] * m[1] * m[11] - 
               m[4] * m[3] * m[9] - 
               m[8] * m[1] * m[7] + 
               m[8] * m[3] * m[5];

    inv[15] = m[0] * m[5] * m[10] - 
              m[0] * m[6] * m[9] - 
              m[4] * m[1] * m[10] + 
              m[4] * m[2] * m[9] + 
              m[8] * m[1] * m[6] - 
              m[8] * m[2] * m[5];

    det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];

    if (det == 0)
        return false;

    det = 1.0 / det;

    for (i = 0; i < 16; i++)
        invOut[i] = inv[i] * det;

    return true;
}*/

        public void SetInverse(Matrix4 m)
        {
            // Make sure the determinant is non-zero.
            double det = GetDeterminant();
            if (Core.Equals(det, 0.0))
            {
                return;
            }
            det = (1.0) / det;

            Data[0] = (-m.Data[9] * m.Data[6] + m.Data[5] * m.Data[10]) * det;
            Data[4] = (m.Data[8] * m.Data[6] - m.Data[4] * m.Data[10]) * det;
            Data[8] = (-m.Data[8] * m.Data[5] + m.Data[4] * m.Data[9]) * det;

            Data[1] = (m.Data[9] * m.Data[2] - m.Data[1] * m.Data[10]) * det;
            Data[5] = (-m.Data[8] * m.Data[2] + m.Data[0] * m.Data[10]) * det;
            Data[9] = (m.Data[8] * m.Data[1] - m.Data[0] * m.Data[9]) * det;

            Data[2] = (-m.Data[5] * m.Data[2] + m.Data[1] * m.Data[6]) * det;
            Data[6] = (+m.Data[4] * m.Data[2] - m.Data[0] * m.Data[6]) * det;
            Data[10] = (-m.Data[4] * m.Data[1] + m.Data[0] * m.Data[5]) * det;

            Data[3] = (m.Data[9] * m.Data[6] * m.Data[3]
                       - m.Data[5] * m.Data[10] * m.Data[3]
                       - m.Data[9] * m.Data[2] * m.Data[7]
                       + m.Data[1] * m.Data[10] * m.Data[7]
                       + m.Data[5] * m.Data[2] * m.Data[11]
                       - m.Data[1] * m.Data[6] * m.Data[11]) * det;
            Data[7] = (-m.Data[8] * m.Data[6] * m.Data[3]
                       + m.Data[4] * m.Data[10] * m.Data[3]
                       + m.Data[8] * m.Data[2] * m.Data[7]
                       - m.Data[0] * m.Data[10] * m.Data[7]
                       - m.Data[4] * m.Data[2] * m.Data[11]
                       + m.Data[0] * m.Data[6] * m.Data[11]) * det;
            Data[11] = (m.Data[8] * m.Data[5] * m.Data[3]
                       - m.Data[4] * m.Data[9] * m.Data[3]
                       - m.Data[8] * m.Data[1] * m.Data[7]
                       + m.Data[0] * m.Data[9] * m.Data[7]
                       + m.Data[4] * m.Data[1] * m.Data[11]
                       - m.Data[0] * m.Data[5] * m.Data[11]) * det;
        }

        public Matrix4 Inverse()
        {
            Matrix4 result = new Matrix4();
            result.SetInverse(this);
            return result;
        }

        public void Invert()
        {
            SetInverse(this);
        }

        public Vector3 TransformDirection(Vector3 vector)
        {
            return new Vector3
               (
                vector.x * Data[0] +
                vector.y * Data[1] +
                vector.z * Data[2],

                vector.x * Data[4] +
                vector.y * Data[5] +
                vector.z * Data[6],

                vector.x * Data[8] +
                vector.y * Data[9] +
                vector.z * Data[10]
                );
        }

        public Vector3 TransformInverseDirection(Vector3 vector)
        {
            return new Vector3
                (
                vector.x * Data[0] +
                vector.y * Data[4] +
                vector.z * Data[8],

                vector.x * Data[1] +
                vector.y * Data[5] +
                vector.z * Data[9],

                vector.x * Data[2] +
                vector.y * Data[6] +
                vector.z * Data[10]
                );
        }

        public Vector3 TransformInverse(Vector3 vector)
        {
            Vector3 tmp = vector;
            tmp.x -= Data[3];
            tmp.y -= Data[7];
            tmp.z -= Data[11];
            return new Vector3
                (
                tmp.x * Data[0] +
                tmp.y * Data[4] +
                tmp.z * Data[8],

                tmp.x * Data[1] +
                tmp.y * Data[5] +
                tmp.z * Data[9],

                tmp.x * Data[2] +
                tmp.y * Data[6] +
                tmp.z * Data[10]
                );
        }

        public Vector3 GetAxisVector(int i)
        {
            return new Vector3(Data[i], Data[i + 4], Data[i + 8]);
        }

        public void SetOrientationAndPos(Quaternion q, Vector3 pos)
        {
            //Data[0] = 1 - (2 * q.j * q.j + 2 * q.k * q.k);
            //Data[1] = 2 * q.i * q.j + 2 * q.k * q.r;
            //Data[2] = 2 * q.i * q.k - 2 * q.j * q.r;
            //Data[3] = pos.x;

            //Data[4] = 2 * q.i * q.j - 2 * q.k * q.r;
            //Data[5] = 1 - (2 * q.i * q.i + 2 * q.k * q.k);
            //Data[6] = 2 * q.j * q.k + 2 * q.i * q.r;
            //Data[7] = pos.y;

            //Data[8] = 2 * q.i * q.k + 2 * q.j * q.r;
            //Data[9] = 2 * q.j * q.k - 2 * q.i * q.r;
            //Data[10] = 1 - (2 * q.i * q.i + 2 * q.j * q.j);
            //Data[11] = pos.z;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="rhs">The right matrix.</param>
        /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix4 lhs, Matrix4 rhs)
        {
            for (int i = 0; i < lhs.Data.Length; i++)
            {
                if (!Core.Equals(lhs.Data[i], rhs.Data[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a hash representation of the object.
        /// </summary>
        /// <returns>The hash representation of the object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines if the matrix is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Matrix4 m = obj as Matrix4;
            if ((System.Object)m == null)
            {
                return false;
            }

            // Return true if the matrices match.
            return (this == m);
        }

        /// <summary>
        /// Determines if the matrix is equal to the given matrix.
        /// </summary>
        /// <param name="m">The matrix to be compared.</param>
        /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix4 m)
        {
            // If parameter is null return false:
            if ((object)m == null)
            {
                return false;
            }

            // Return true if the matrices match.
            return (this == m);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="rhs">The right matrix.</param>
        /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix4 lhs, Matrix4 rhs)
        {
            return !(lhs == rhs);
        }
    }
}
