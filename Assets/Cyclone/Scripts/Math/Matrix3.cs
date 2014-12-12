
namespace Cyclone.Math
{
    /// <summary>
    /// Holds an inertia tensor, consisting of a 3x3 row-major matrix.
    /// This matrix is not padding to produce an aligned structure, since
    /// it is most commonly used with a mass (single real) and two
    /// damping coefficients to make the 12-element characteristics array
    /// of a rigid body.
    /// </summary>
    public class Matrix3
    {
        /// <summary>
        /// The matrix has 9 elements.
        /// </summary>
        private const int ElementCount = 9;

        /// <summary>
        /// Gets the matrix data.
        /// </summary>
        public double[] Data { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Matrix3"/> class.
        /// </summary>
        public Matrix3()
        {
            Data = new double[ElementCount];
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Matrix3"/> class.
        /// </summary>
        /// <param name="compOne">The first component.</param>
        /// <param name="compTwo">The second component.</param>
        /// <param name="compThree">The third component.</param>
        public Matrix3(Vector3 compOne, Vector3 compTwo, Vector3 compThree)
        {
            Data = new double[ElementCount];
            SetComponents(compOne, compTwo, compThree);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Matrix3"/> class.
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
        public Matrix3(double c0, double c1, double c2, double c3, double c4, double c5, double c6, double c7, double c8)
        {
            Data = new[] { c0, c1, c2, c3, c4, c5, c6, c7, c8 };
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">The other matrix.</param>
        public Matrix3(Matrix3 other)
        {
            Data = new double[ElementCount];
            other.Data.CopyTo(Data, 0);
        }

        /// <summary>
        /// Sets the matrix to be a diagonal matrix with the given
        /// values along the leading diagonal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        void SetDiagonal(double a, double b, double c)
        {
            SetInertiaTensorCoeffs(a, b, c);
        }

        /// <summary>
        /// Sets the value of the matrix from inertia tensor values.
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        /// <param name="ixy"></param>
        /// <param name="ixz"></param>
        /// <param name="iyz"></param>
        void SetInertiaTensorCoeffs(double ix, double iy, double iz, double ixy = 0, double ixz = 0, double iyz = 0)
        {
            Data[0] = ix;
            Data[1] = Data[3] = -ixy;
            Data[2] = Data[6] = -ixz;
            Data[4] = iy;
            Data[5] = Data[7] = -iyz;
            Data[8] = iz;
        }

        /// <summary>
        /// Sets the value of the matrix as an inertia tensor of
        /// a rectangular block aligned with the body's coordinate
        /// system with the given axis half-sizes and mass.
        /// </summary>
        /// <param name="halfSizes"></param>
        /// <param name="mass"></param>
        void SetBlockInertiaTensor(Vector3 halfSizes, double mass)
        {
            Vector3 squares = halfSizes.ComponentProduct(halfSizes);
            SetInertiaTensorCoeffs
                (
                0.3f * mass * (squares.y + squares.z),
                0.3f * mass * (squares.x + squares.z),
                0.3f * mass * (squares.x + squares.y)
                );
        }

        /// <summary>
        /// Sets the matrix to be a skew symmetric matrix based on
        /// the given vector. The skew symmetric matrix is the equivalent
        /// of the vector product. So if a,b are vectors. a x b = A_s b
        /// where A_s is the skew symmetric form of a.
        /// </summary>
        /// <param name="vector">Vector defining the skew symmetric.</param>
        void SetSkewSymmetric(Vector3 vector)
        {
            Data[0] = Data[4] = Data[8] = 0;
            Data[1] = -vector.z;
            Data[2] = vector.y;
            Data[3] = vector.z;
            Data[5] = -vector.x;
            Data[6] = -vector.y;
            Data[7] = vector.x;
        }

        /// <summary>
        /// Sets the matrix values from the given three vector components.
        /// These are arranged as the three columns of the vector.
        /// </summary>
        /// <param name="compOne">The first component.</param>
        /// <param name="compTwo">The second component.</param>
        /// <param name="compThree">The third component.</param>
        void SetComponents(Vector3 compOne, Vector3 compTwo, Vector3 compThree)
        {
            Data[0] = compOne.x;
            Data[1] = compTwo.x;
            Data[2] = compThree.x;
            Data[3] = compOne.y;
            Data[4] = compTwo.y;
            Data[5] = compThree.y;
            Data[6] = compOne.z;
            Data[7] = compTwo.z;
            Data[8] = compThree.z;
        }

        /// <summary>
        /// Perform multiplication of a matrix and a vector.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>A new vector as a result of multiplication of the given matrix and vector.</returns>
        public static Vector3 operator *(Matrix3 lhs, Vector3 vector)
        {
            return new Vector3
                (
                vector.x * lhs.Data[0] + vector.y * lhs.Data[1] + vector.z * lhs.Data[2],
                vector.x * lhs.Data[3] + vector.y * lhs.Data[4] + vector.z * lhs.Data[5],
                vector.x * lhs.Data[6] + vector.y * lhs.Data[7] + vector.z * lhs.Data[8]
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
        /// Transform the given vector by the transpose of this matrix.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The resulting vector.</returns>
        public Vector3 TransformTranspose(Vector3 vector)
        {
            return new Vector3
                (
                vector.x * Data[0] + vector.y * Data[3] + vector.z * Data[6],
                vector.x * Data[1] + vector.y * Data[4] + vector.z * Data[7],
                vector.x * Data[2] + vector.y * Data[5] + vector.z * Data[8]
                );
        }

        /// <summary>
        /// Gets a vector representing one row in the matrix.
        /// </summary>
        /// <param name="i">The row index.</param>
        /// <returns>A vector representing a row in the matrix.</returns>
        public Vector3 GetRowVector(int i)
        {
            return new Vector3(Data[i * 3], Data[i * 3 + 1], Data[i * 3 + 2]);
        }

        /// <summary>
        /// Gets a vector representing one axis (i.e. one column) in the matrix.
        /// </summary>
        /// <param name="i">The row index.</param>
        /// <returns>A vector representing an axis in the matrix.</returns>
        public Vector3 GetAxisVector(int i)
        {
            return new Vector3(Data[i], Data[i + 3], Data[i + 6]);
        }

        /// <summary>
        /// Sets the matrix to be the inverse of the given matrix.
        /// </summary>
        /// <param name="m">The matrix to invert.</param>
        void SetInverse(Matrix3 m)
        {
            double t4 = m.Data[0] * m.Data[4];
            double t6 = m.Data[0] * m.Data[5];
            double t8 = m.Data[1] * m.Data[3];
            double t10 = m.Data[2] * m.Data[3];
            double t12 = m.Data[1] * m.Data[6];
            double t14 = m.Data[2] * m.Data[6];

            // Calculate the determinant
            double t16 = (t4 * m.Data[8] - t6 * m.Data[7] - t8 * m.Data[8] +
                        t10 * m.Data[7] + t12 * m.Data[5] - t14 * m.Data[4]);

            // Make sure the determinant is non-zero.
            if (Core.Equals(t16, 0.0))
            {
                return;
            }
            double t17 = 1 / t16;

            Data[0] = (m.Data[4] * m.Data[8] - m.Data[5] * m.Data[7]) * t17;
            Data[1] = -(m.Data[1] * m.Data[8] - m.Data[2] * m.Data[7]) * t17;
            Data[2] = (m.Data[1] * m.Data[5] - m.Data[2] * m.Data[4]) * t17;
            Data[3] = -(m.Data[3] * m.Data[8] - m.Data[5] * m.Data[6]) * t17;
            Data[4] = (m.Data[0] * m.Data[8] - t14) * t17;
            Data[5] = -(t6 - t10) * t17;
            Data[6] = (m.Data[3] * m.Data[7] - m.Data[4] * m.Data[6]) * t17;
            Data[7] = -(m.Data[0] * m.Data[7] - t12) * t17;
            Data[8] = (t4 - t8) * t17;
        }

        /// <summary>
        /// Returns a new matrix containing the inverse of this matrix.
        /// </summary>
        /// <returns>A new matrix containing the inverse of this matrix.</returns>
        public Matrix3 Inverse()
        {
            Matrix3 result = new Matrix3();
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
            Matrix3 thisMatrixCopy = new Matrix3(this);
            SetInverse(thisMatrixCopy);
        }

        /**
         * Sets the matrix to be the transpose of the given matrix.
         *
         * @param m The matrix to transpose and use to set this.
         */
        void SetTranspose(Matrix3 m)
        {
            Data[0] = m.Data[0];
            Data[1] = m.Data[3];
            Data[2] = m.Data[6];
            Data[3] = m.Data[1];
            Data[4] = m.Data[4];
            Data[5] = m.Data[7];
            Data[6] = m.Data[2];
            Data[7] = m.Data[5];
            Data[8] = m.Data[8];
        }

        /** Returns a new matrix containing the transpose of this matrix. */
        Matrix3 Transpose()
        {
            Matrix3 result = new Matrix3();
            result.SetTranspose(this);
            return result;
        }

        /// <summary>
        /// Perform matrix multiplication.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="rhs">The right matrix.</param>
        /// <returns>A new matrix as a result of multiplication of the left and right matrix.</returns>
        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 result = new Matrix3
                (
                lhs.Data[0] * rhs.Data[0] + lhs.Data[1] * rhs.Data[3] + lhs.Data[2] * rhs.Data[6],
                lhs.Data[0] * rhs.Data[1] + lhs.Data[1] * rhs.Data[4] + lhs.Data[2] * rhs.Data[7],
                lhs.Data[0] * rhs.Data[2] + lhs.Data[1] * rhs.Data[5] + lhs.Data[2] * rhs.Data[8],

                lhs.Data[3] * rhs.Data[0] + lhs.Data[4] * rhs.Data[3] + lhs.Data[5] * rhs.Data[6],
                lhs.Data[3] * rhs.Data[1] + lhs.Data[4] * rhs.Data[4] + lhs.Data[5] * rhs.Data[7],
                lhs.Data[3] * rhs.Data[2] + lhs.Data[4] * rhs.Data[5] + lhs.Data[5] * rhs.Data[8],

                lhs.Data[6] * rhs.Data[0] + lhs.Data[7] * rhs.Data[3] + lhs.Data[8] * rhs.Data[6],
                lhs.Data[6] * rhs.Data[1] + lhs.Data[7] * rhs.Data[4] + lhs.Data[8] * rhs.Data[7],
                lhs.Data[6] * rhs.Data[2] + lhs.Data[7] * rhs.Data[5] + lhs.Data[8] * rhs.Data[8]
                );

            return result;
        }

        /// <summary>
        /// Perform multiplication of a matrix and a vector.
        /// </summary>
        /// <param name="lhs">The left matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>A new vector as a result of multiplication of the given matrix and vector.</returns>
        public static Matrix3 operator *(Matrix3 lhs, double scalar)
        {
            Matrix3 result = new Matrix3(lhs);

            result.Data[0] *= scalar; result.Data[1] *= scalar; result.Data[2] *= scalar;
            result.Data[3] *= scalar; result.Data[4] *= scalar; result.Data[5] *= scalar;
            result.Data[6] *= scalar; result.Data[7] *= scalar; result.Data[8] *= scalar;

            return result;
        }

        public static Matrix3 operator +(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 result = new Matrix3(lhs);

            result.Data[0] += rhs.Data[0]; result.Data[1] += rhs.Data[1]; result.Data[2] += rhs.Data[2];
            result.Data[3] += rhs.Data[3]; result.Data[4] += rhs.Data[4]; result.Data[5] += rhs.Data[5];
            result.Data[6] += rhs.Data[6]; result.Data[7] += rhs.Data[7]; result.Data[8] += rhs.Data[8];

            return result;
        }

        public void SetOrientation(Quaternion q)
        {
            Data[0] = 1 - (2 * q.j * q.j + 2 * q.k * q.k);
            Data[1] = 2 * q.i * q.j + 2 * q.k * q.r;
            Data[2] = 2 * q.i * q.k - 2 * q.j * q.r;
            Data[3] = 2 * q.i * q.j - 2 * q.k * q.r;
            Data[4] = 1 - (2 * q.i * q.i + 2 * q.k * q.k);
            Data[5] = 2 * q.j * q.k + 2 * q.i * q.r;
            Data[6] = 2 * q.i * q.k + 2 * q.j * q.r;
            Data[7] = 2 * q.j * q.k - 2 * q.i * q.r;
            Data[8] = 1 - (2 * q.i * q.i + 2 * q.j * q.j);
        }

        public Matrix3 LinearInterpolate(Matrix3 a, Matrix3 b, double prop)
        {
            Matrix3 result = new Matrix3();
            for (int i = 0; i < ElementCount; i++)
            {
                result.Data[i] = a.Data[i] * (1 - prop) + b.Data[i] * prop;
            }
            return result;
        }
    }
}
