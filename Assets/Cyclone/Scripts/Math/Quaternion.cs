using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclone.Math
{
    public class Quaternion
    {
        /// <summary>
        /// Gets or sets the real component of the quaternion.
        /// </summary>
        public double r { get; set; }

        /// <summary>
        /// Gets or sets the first complex component of the quaternion.
        /// </summary>
        public double i { get; set; }

        /// <summary>
        /// Gets or sets the second complex component of the quaternion.
        /// </summary>
        public double j { get; set; }

        /// <summary>
        /// Gets or sets the third complex component of the quaternion.
        /// </summary>
        public double k { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="Quaternion"/> class.
        /// </summary>
        public Quaternion()
        {
            r = 0;
            i = 0;
            j = 0;
            k = 0;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Quaternion"/> class.
        /// </summary>
        /// <param name="r">The real component of the quaternion.</param>
        /// <param name="i">The first complex component of the quaternion.</param>
        /// <param name="j">The second complex component of the quaternion.</param>
        /// <param name="k">The third complex component of the quaternion.</param>
        public Quaternion(double r, double i, double j, double k)
        {
            this.r = r;
            this.i = i;
            this.j = j;
            this.k = k;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Quaternion"/> class.
        /// </summary>
        /// <param name="other">The other quaternion.</param>
        public Quaternion(Quaternion other)
        {
            r = other.r;
            i = other.i;
            j = other.j;
            k = other.k;
        }

        /// <summary>
        /// Normalises the quaternion to unit length, making it a valid
        /// orientation quaternion.
        /// </summary>
        public void Normalize()
        {
            double d = r * r + i * i + j * j + k * k;

            // Check for zero length quaternion, and use the no-rotation
            // quaternion in that case.
            if (Core.Equals(d, 0.0))
            {
                r = 1;
                return;
            }

            d = (1.0) / System.Math.Sqrt(d);
            r *= d;
            i *= d;
            j *= d;
            k *= d;
        }

        /// <summary>
        /// Multiplication operator.
        /// </summary>
        /// <param name="lhs">The left quaternion.</param>
        /// <param name="rhs">The right quaternion.</param>
        /// <returns>The resulting quaternion from the multiplication.</returns>
        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            Quaternion q = new Quaternion();
            q.r = lhs.r * rhs.r - lhs.i * rhs.i -
                  lhs.j * rhs.j - lhs.k * rhs.k;
            q.i = lhs.r * rhs.i + lhs.i * rhs.r +
                  lhs.j * rhs.k - lhs.k * rhs.j;
            q.j = lhs.r * rhs.j + lhs.j * rhs.r +
                  lhs.k * rhs.i - lhs.i * rhs.k;
            q.k = lhs.r * rhs.k + lhs.k * rhs.r +
                  lhs.i * rhs.j - lhs.j * rhs.i;

            return q;
        }

        /// <summary>
        /// Adds the given vector to this, scaled by the given amount.
        /// This is used to update the orientation quaternion by a rotation
        /// and time.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        /// <param name="scale">The amount of the vector to add.</param>
        public void AddScaledVector(Vector3 vector, double scale)
        {
            Quaternion q = new Quaternion
                (
                0,
                vector.x * scale,
                vector.y * scale,
                vector.z * scale
                );

            q *= this;
            r += q.r * 0.5;
            i += q.i * 0.5;
            j += q.j * 0.5;
            k += q.k * 0.5;
        }

        /// <summary>
        /// Rotate the quaternion by a given vector.
        /// </summary>
        /// <param name="vector">The amount to rotate.</param>
        public void RotateByVector(Vector3 vector)
        {
            Quaternion q = new Quaternion(0, vector.x, vector.y, vector.z);
            Quaternion thisQuaternion = this;
            thisQuaternion *= q;
            r = thisQuaternion.r;
            i = thisQuaternion.i;
            j = thisQuaternion.j;
            k = thisQuaternion.k;
        }

        /// <summary>
        /// Convert to a string representation.
        /// </summary>
        /// <returns>A string representation of the quaternion.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("({0}, {1}, {2}, {3})", i, j, k, r);
            return sb.ToString();
        }
    }
}
