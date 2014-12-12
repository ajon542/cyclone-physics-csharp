using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
