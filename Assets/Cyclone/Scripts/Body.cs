using System;
using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// A rigid body is the basic simulation object in the physics core.
    /// </summary>
    public class RigidBody
    {
        /// <summary>
        /// Gets or sets the inverse mass of the rigid body.
        /// It is more useful to hold the inverse mass because
        /// integration is simpler, and because in real-time simulation
        /// it is more useful to have bodies with infinite mass than
        /// zero mass.
        /// </summary>
        protected double InverseMass { get; set; }

        /// <summary>
        /// Gets or sets the amount of damping applied to linear
        /// motion. Damping is required to remove energy added through
        /// numerical instability in the integrator.
        /// </summary>
        protected double LinearDamping { get; set; }

        /// <summary>
        /// Gets or sets the linear position of the rigid boy in world space.
        /// </summary>
        protected Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the angular orientation of the rigid body in world space.
        /// </summary>
        protected Quaternion Orientation { get; set; }

        /// <summary>
        /// Gets or sets the linear velocity of the rigid body in world space.
        /// </summary>
        protected Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the angular velocity, or rotation of the rigid body in world space.
        /// </summary>
        protected Vector3 Rotation { get; set; }

        /// <summary>
        /// Gets or sets the transform matrix for converting body space into
        /// world space and vice versa. This can be acheived by calling the
        /// getPointIn*Space methods.
        /// </summary>
        /// <remarks>
        /// This matrix should be derived from the orientation and position
        /// once per frame, to make sure it is correct. This just acts as a
        /// cache to void repeated calculations.
        /// </remarks>
        protected Matrix4 TransformMatrix { get; set; }

        /// <summary>
        /// Calculates internal data from state data. This should be called
        /// after the body's state is altered directly (is is called automatically
        /// during integration). If you change the body's state and then intend
        /// to integrate before querying any data (such as the transform matrix),
        /// then you can omit this step.
        /// </summary>
        protected void CalculateDerivedData()
        {
            Orientation.Normalize();
            CalculateTransformMatrix(TransformMatrix, Position, Orientation);
        }

        /// <summary>
        /// Create the transform matrix from position and orientation.
        /// </summary>
        /// <param name="transformMatrix">The transform matrix.</param>
        /// <param name="position">The position.</param>
        /// <param name="orientation">The orientation.</param>
        protected static void CalculateTransformMatrix(Matrix4 transformMatrix, Vector3 position, Quaternion orientation)
        {
            transformMatrix.Data[0] = 1 - 2 * orientation.j * orientation.j - 2 * orientation.k * orientation.k;
            transformMatrix.Data[1] =     2 * orientation.i * orientation.j - 2 * orientation.r * orientation.k;
            transformMatrix.Data[2] =     2 * orientation.i * orientation.k + 2 * orientation.r * orientation.j;
            transformMatrix.Data[3] = position.x;

            transformMatrix.Data[4] =     2 * orientation.i * orientation.j + 2 * orientation.r * orientation.k;
            transformMatrix.Data[5] = 1 - 2 * orientation.i * orientation.i - 2 * orientation.k * orientation.k;
            transformMatrix.Data[6] =     2 * orientation.j * orientation.k - 2 * orientation.r * orientation.i;
            transformMatrix.Data[7] = position.y;

            transformMatrix.Data[8] =      2 * orientation.i * orientation.k - 2 * orientation.r * orientation.j;
            transformMatrix.Data[9] =      2 * orientation.j * orientation.k + 2 * orientation.r * orientation.i;
            transformMatrix.Data[10] = 1 - 2 * orientation.i * orientation.i - 2 * orientation.j * orientation.j;
            transformMatrix.Data[11] = position.z;
        }
    }
}
