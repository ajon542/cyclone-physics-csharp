using System.Text;

namespace Cyclone
{
    /// <summary>
    /// Represents a bounding sphere that can be tested for overlap.
    /// </summary>
    public class BoundingSphere
    {
        /// <summary>
        /// Gets or sets the center position of this bounding sphere.
        /// </summary>
        public Math.Vector3 Center { get; set; }

        /// <summary>
        /// Gets or sets the radius of this bounding sphere.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Gets the volume of this bounding volume. This is used
        /// to calculate how to recurse into the bounding volume tree.
        /// For a bounding sphere it is a simple calculation.
        /// </summary>
        public double Size
        {
            get
            {
                return 1.333333 * 3.141593 * Radius * Radius * Radius;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BoundingSphere"/> class
        /// given a center and radius.
        /// </summary>
        /// <param name="center">The center of the bounding sphere.</param>
        /// <param name="radius">The radius of the bounding sphere.</param>
        public BoundingSphere(Math.Vector3 center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BoundingSphere"/> class.
        /// to enclose the two given bounding spheres.
        /// </summary>
        /// <param name="one">The first bounding sphere to enclose.</param>
        /// <param name="two">The second bounding sphere to enclose.</param>
        public BoundingSphere(BoundingSphere one, BoundingSphere two)
        {
            Math.Vector3 centreOffset = two.Center - one.Center;
            double distance = centreOffset.SquareMagnitude;
            double radiusDiff = two.Radius - one.Radius;

            // Check if the larger sphere encloses the small one.
            if (radiusDiff * radiusDiff >= distance)
            {
                if (one.Radius > two.Radius)
                {
                    Center = one.Center;
                    Radius = one.Radius;
                }
                else
                {
                    Center = two.Center;
                    Radius = two.Radius;
                }
            }

            // Otherwise we need to work with partially
            // overlapping spheres.
            else
            {
                distance = System.Math.Sqrt(distance);
                Radius = (distance + one.Radius + two.Radius) * 0.5;

                // The new centre is based on one's centre, moved towards
                // two's centre by an ammount proportional to the spheres'
                // radii.
                Center = one.Center;
                if (distance > 0)
                {
                    Center += centreOffset * ((Radius - one.Radius) / distance);
                }
            }
        }

        /// <summary>
        /// Checks if the bounding sphere overlaps with the other given
        /// bounding sphere.
        /// </summary>
        /// <param name="other">The other bounding sphere.</param>
        /// <returns><c>true</c> if the bounding spheres overlap; otherwise, <c>false</c>.</returns>
        public bool Overlaps(BoundingSphere other)
        {
            double distanceSquared = (Center - other.Center).SquareMagnitude;

            return distanceSquared < (Radius + other.Radius) * (Radius + other.Radius);
        }

        /// <summary>
        /// Reports how much this bounding sphere would have to grow
        /// by to incorporate the given bounding sphere. Note that this
        /// calculation returns a value not in any particular units (i.e.
        /// its not a volume growth). In fact the best implementation
        /// takes into account the growth in surface area (after the
        /// Goldsmith-Salmon algorithm for tree construction).
        /// </summary>
        /// <param name="other">The other bounding sphere.</param>
        /// <returns>The amount the bounding sphere has to grow.</returns>
        public double GetGrowth(BoundingSphere other)
        {
            BoundingSphere newSphere = new BoundingSphere(this, other);

            // We return a value proportional to the change in surface
            // area of the sphere.
            return newSphere.Radius * newSphere.Radius - Radius * Radius;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("C: {0}, R: {1}", Center, Radius);
            return sb.ToString();
        }
    };
}
