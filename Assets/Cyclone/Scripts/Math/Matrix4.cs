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
        /// <param name="c0">Matrix data.</param>
        /// <param name="c1">Matrix data.</param>
        /// <param name="c2">Matrix data.</param>
        /// <param name="c3">Matrix data.</param>
        /// <param name="c4">Matrix data.</param>
        /// <param name="c5">Matrix data.</param>
        /// <param name="c6">Matrix data.</param>
        /// <param name="c7">Matrix data.</param>
        /// <param name="c8">Matrix data.</param>
        /// <param name="c9">Matrix data.</param>
        /// <param name="c10">Matrix data.</param>
        /// <param name="c11">Matrix data.</param>
        public Matrix4
            (
            double c0, double c1, double c2, double c3,
            double c4, double c5, double c6, double c7,
            double c8, double c9, double c10, double c11
            )
        {
            Data = new[] { c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11 };
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
        /// Creates a new instance of the <see cref="Matrix4"/> class.
        /// </summary>
        /// <param name="values">Matrix data.</param>
        public Matrix4(double[] values)
        {
            if (values.Length != ElementCount)
            {
                throw new Exception("not enough values to initialize this matrix");
            }
            Data = new double[ElementCount];
            values.CopyTo(Data, 0);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">The other matrix.</param>
        public Matrix4(Matrix4 other)
        {
            Data = new double[ElementCount];
            other.Data.CopyTo(Data, 0);
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

        /// <summary>
        /// Perform matrix multiplication.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="rhs">The right matrix.</param>
        /// <returns>A new matrix as a result of multiplication of the left and right matrix.</returns>
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

        /// <summary>
        /// Perform multiplication of a matrix and a vector.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>A new vector as a result of multiplication of the given matrix and vector.</returns>
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

        /// <summary>
        /// Transform the given vector.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        public Vector3 Transform(Vector3 vector)
        {
            return this * vector;
        }

        /// <summary>
        /// Get the determinant of this matrix.
        /// </summary>
        /// <returns>The determinant of this matrix.</returns>
        public double GetDeterminant()
        {
            return -Data[2] * Data[5] * Data[8] +
                    Data[1] * Data[6] * Data[8] +
                    Data[2] * Data[4] * Data[9] -
                    Data[0] * Data[6] * Data[9] -
                    Data[1] * Data[4] * Data[10] +
                    Data[0] * Data[5] * Data[10];
        }

        /// <summary>
        /// Sets this matrix to the inverse of the given matrix.
        /// </summary>
        /// <param name="m">The matrix to invert.</param>
        public void SetInverse(Matrix4 m)
        {
            // Make sure the determinant is non-zero.
            double det = m.GetDeterminant();
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

        /// <summary>
        /// Returns a new matrix containing the inverse of this matrix.
        /// </summary>
        /// <returns>A new matrix containing the inverse of this matrix.</returns>
        public Matrix4 Inverse()
        {
            Matrix4 result = new Matrix4();
            result.SetInverse(this);
            return result;
        }

        /// <summary>
        /// Inverts this matrix.
        /// </summary>
        public void Invert()
        {
            // The reason for the copy of the matrix, is due to the
            // fact everything is passed by reference.
            Matrix4 thisMatrixCopy = new Matrix4(this);
            SetInverse(thisMatrixCopy);
        }

        /// <summary>
        /// Transform the given direction vector by this matrix.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
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

        /// <summary>
        /// Transform the given direction vector by the
        /// transformational inverse of this matrix.
        /// </summary>
        /// <remarks>
        /// This function relies on the fact that the inverse of
        /// a pure rotation matrix is its transpose. It separates the
        /// translational and rotation components, transposes the
        /// rotation, and multiplies out. If the matrix is not a
        /// scale and shear free transform matrix, then this function
        /// will not give correct results.
        /// </remarks>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
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

        /// <summary>
        /// Transform the given direction vector by the
        /// transformational inverse of this matrix.
        /// </summary>
        /// <remarks>
        /// This function relies on the fact that the inverse of
        /// a pure rotation matrix is its transpose. It separates the
        /// translational and rotation components, transposes the
        /// rotation, and multiplies out. If the matrix is not a
        /// scale and shear free transform matrix, then this function
        /// will not give correct results.
        /// </remarks>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
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

        /// <summary>
        /// Gets a vector representing one axis (i.e. one column) in the matrix.
        /// </summary>
        /// <param name="i">
        /// The row to return. Row 3 corresponds to the position
        /// of the transform matrix.
        /// </param>
        /// <returns>The vector.</returns>
        public Vector3 GetAxisVector(int i)
        {
            return new Vector3(Data[i], Data[i + 4], Data[i + 8]);
        }

        /// <summary>
        /// Sets this matrix to be the rotation matrix corresponding to the given quaternion.
        /// </summary>
        /// <param name="q">The quaternion.</param>
        /// <param name="pos">The position vector.</param>
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
