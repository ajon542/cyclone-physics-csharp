using System;

namespace Cyclone.Math
{
    /// <summary>
    /// Vector3 implementation and manipulation.
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// Gets or sets the X component.
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// Gets or sets the Y component.
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Gets or sets the Z component.
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Vector3()
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="Vector3"/> class.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Vector3"/> class.
        /// </summary>
        /// <param name="vector">The vector to copy.</param>
        public Vector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        /// <summary>
        /// Gets the corresponding vector component.
        /// </summary>
        /// <param name="i">The index into the vector components.</param>
        /// <returns>The corresponding vector component.</returns>
        public double this[int i]
        {
            get
            {
                if (i < 0 || i > 2)
                {
                    throw new IndexOutOfRangeException("Index " + i + " does not correspond to a Vector3 component.");
                }

                if (i == 0)
                {
                    return x;
                }

                if (i == 1)
                {
                    return y;
                }

                return z;
            }
        }

        /// <summary>
        /// Addition operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns>The resulting vector from the addition.</returns>
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        /// <summary>
        /// Subtraction operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns>The resulting vector from the subtraction.</returns>
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        /// <summary>
        /// Multiplication operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="value">The value to multiply the vector by.</param>
        /// <returns>The resulting vector from the multiplication.</returns>
        public static Vector3 operator *(Vector3 lhs, double value)
        {
            return new Vector3(lhs.x * value, lhs.y * value, lhs.z * value);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return Core.Equals(lhs.x, rhs.x) && Core.Equals(lhs.y, rhs.y) && Core.Equals(lhs.z, rhs.z);
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
        /// Determines if the vector is equal to the given object.
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
            Vector3 v = obj as Vector3;
            if ((System.Object)v == null)
            {
                return false;
            }

            // Return true if the vectors match.
            return Core.Equals(x, v.x) && Core.Equals(y, v.y) && Core.Equals(z, v.z);
        }

        /// <summary>
        /// Determines if the vector is equal to the given vector.
        /// </summary>
        /// <param name="obj">The vector to be compared.</param>
        /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector3 v)
        {
            // If parameter is null return false:
            if ((object)v == null)
            {
                return false;
            }

            // Return true if the vectors match.
            return Core.Equals(x, v.x) && Core.Equals(y, v.y) && Core.Equals(z, v.z);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Less than operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns><c>true</c> if less than; otherwise, <c>false</c>.</returns>
        public static bool operator <(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.x < rhs.x) && (lhs.y < rhs.y) && (lhs.z < rhs.z);
        }

        /// <summary>
        /// Greater than operator.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns><c>true</c> if greater than; otherwise, <c>false</c>.</returns>
        public static bool operator >(Vector3 lhs, Vector3 rhs)
        {
            return (lhs.x > rhs.x) && (lhs.y > rhs.y) && (lhs.z > rhs.z);
        }

        /// <summary>
        /// Calculate the component product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A new vector representing the component product.</returns>
        public Vector3 ComponentProduct(Vector3 vector)
        {
            return new Vector3(x * vector.x, y * vector.y, z * vector.z);
        }

        /// <summary>
        /// Calculate the component product and update this vector with the result.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public void ComponentProductUpdate(Vector3 vector)
        {
            x *= vector.x;
            y *= vector.y;
            z *= vector.z;
        }

        /// <summary>
        /// Calculate the cross product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A new vector representing the cross product.</returns>
        public Vector3 VectorProduct(Vector3 vector)
        {
            return new Vector3(y * vector.z - z * vector.y,
                               z * vector.x - x * vector.z,
                               x * vector.y - y * vector.x);
        }

        /// <summary>
        /// Calculate the cross product and update this vector with the result.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public void VectorProductUpdate(Vector3 vector)
        {
            Vector3 tmp = VectorProduct(vector);
            x = tmp.x;
            y = tmp.y;
            z = tmp.z;
        }

        /// <summary>
        /// Calculate the dot product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The dot product.</returns>
        public double ScalarProduct(Vector3 vector)
        {
            return x * vector.x + y * vector.y + z * vector.z;
        }

        /// <summary>
        /// Add a scaled vector to this vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scale">The amount to scale.</param>
        public void AddScaledVector(Vector3 vector, double scale)
        {
            x += vector.x * scale;
            y += vector.y * scale;
            z += vector.z * scale;
        }

        /// <summary>
        /// Gets the magnitude or length of this vector.
        /// </summary>
        public double Magnitude
        {
            get { return System.Math.Sqrt(x * x + y * y + z * z); }
        }

        /// <summary>
        /// Gets the square of the magnitude.
        /// </summary>
        public double SquareMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        /// <summary>
        /// Trims this vector to a certain size.
        /// </summary>
        /// <param name="size">The size to trim this vector.</param>
        public void Trim(double size)
        {
            if (SquareMagnitude > size * size)
            {
                Normalize();
                x *= size;
                y *= size;
                z *= size;
            }
        }

        /// <summary>
        /// Normalize this vector.
        /// </summary>
        public void Normalize()
        {
            double length = Magnitude;
            if (length > 0)
            {
                x *= 1 / length;
                y *= 1 / length;
                z *= 1 / length;
            }
        }

        /// <summary>
        /// Calculate the unit or normalized vector.
        /// </summary>
        /// <returns>A new vector representing the unit vector.</returns>
        public Vector3 Unit()
        {
            Vector3 unit = new Vector3(this);
            unit.Normalize();
            return unit;
        }

        /// <summary>
        /// Clear this vector to (0, 0, 0).
        /// </summary>
        public void Clear()
        {
            x = y = z = 0;
        }

        /// <summary>
        /// Invert this vector to (-x, -y, -z).
        /// </summary>
        public void Invert()
        {
            x = -x;
            y = -y;
            z = -z;
        }
    }
}
