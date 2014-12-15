using System;
using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// A rigid body is the basic simulation object in the physics core.
    /// </summary>
    public class RigidBody
    {
        /// This data holds the state of the rigid body. There are two
        /// sets of data: characteristics and state.
        ///
        /// Characteristics are properties of the rigid body
        /// independent of its current kinematic situation. This
        /// includes mass, moment of inertia and damping
        /// properties. Two identical rigid bodys will have the same
        /// values for their characteristics.
        ///
        /// State includes all the characteristics and also includes
        /// the kinematic situation of the rigid body in the current
        /// simulation. By setting the whole state data, a rigid body's
        /// exact game state can be replicated. Note that state does
        /// not include any forces applied to the body. Two identical
        /// rigid bodies in the same simulation will not share the same
        /// state values.
        ///
        /// The state values make up the smallest set of independent
        /// data for the rigid body. Other state data is calculated
        /// from their current values. When state data is changed the
        /// dependent values need to be updated: this can be achieved
        /// either by integrating the simulation, or by calling the
        /// calculateInternals function. This two stage process is used
        /// because recalculating internals can be a costly process:
        /// all state changes should be carried out at the same time,
        /// allowing for a single call.
        #region Characteristic Data and State

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

        /**
         * Holds the inverse of the body's inertia tensor. The
         * inertia tensor provided must not be degenerate
         * (that would mean the body had zero inertia for
         * spinning along one axis). As long as the tensor is
         * finite, it will be invertible. The inverse tensor
         * is used for similar reasons to the use of inverse
         * mass.
         *
         * The inertia tensor, unlike the other variables that
         * define a rigid body, is given in body space.
         *
         * @see inverseMass
         */
        protected Matrix3 InverseInertiaTensor { get; set; }

        /**
         * Holds the amount of damping applied to angular
         * motion.  Damping is required to remove energy added
         * through numerical instability in the integrator.
         */
        protected double AngularDamping { get; set; }

        #endregion

        /// These data members hold information that is derived from
        /// the other data in the class.
        #region Derived Data

        /**
         * Holds the inverse inertia tensor of the body in world
         * space. The inverse inertia tensor member is specified in
         * the body's local space.
         *
         * @see inverseInertiaTensor
         */
        Matrix3 inverseInertiaTensorWorld;

        /**
         * Holds the amount of motion of the body. This is a recency
         * weighted mean that can be used to put a body to sleap.
         */
        double motion;

        /**
         * A body can be put to sleep to avoid it being updated
         * by the integration functions or affected by collisions
         * with the world.
         */
        bool isAwake;

        /**
         * Some bodies may never be allowed to fall asleep.
         * User controlled bodies, for example, should be
         * always awake.
         */
        bool canSleep;

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

        #endregion

        /// These data members store the current force, torque and
        /// acceleration of the rigid body. Forces can be added to the
        /// rigid body in any order, and the class decomposes them into
        /// their constituents, accumulating them for the next
        /// simulation step. At the simulation step, the accelerations
        /// are calculated and stored to be applied to the rigid body.
        #region Force and Torque Accumulators

        /**
         * Holds the accumulated force to be applied at the next
         * integration step.
         */
        Vector3 forceAccum;

        /**
         * Holds the accumulated torque to be applied at the next
         * integration step.
         */
        Vector3 torqueAccum;

        /**
          * Holds the acceleration of the rigid body.  This value
          * can be used to set acceleration due to gravity (its primary
          * use), or any other constant acceleration.
          */
        Vector3 acceleration;

        /**
         * Holds the linear acceleration of the rigid body, for the
         * previous frame.
         */
        Vector3 lastFrameAcceleration;

        #endregion

        #region Integration and Simulation Functions

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
            transformMatrix.Data[1] = 2 * orientation.i * orientation.j - 2 * orientation.r * orientation.k;
            transformMatrix.Data[2] = 2 * orientation.i * orientation.k + 2 * orientation.r * orientation.j;
            transformMatrix.Data[3] = position.x;

            transformMatrix.Data[4] = 2 * orientation.i * orientation.j + 2 * orientation.r * orientation.k;
            transformMatrix.Data[5] = 1 - 2 * orientation.i * orientation.i - 2 * orientation.k * orientation.k;
            transformMatrix.Data[6] = 2 * orientation.j * orientation.k - 2 * orientation.r * orientation.i;
            transformMatrix.Data[7] = position.y;

            transformMatrix.Data[8] = 2 * orientation.i * orientation.k - 2 * orientation.r * orientation.j;
            transformMatrix.Data[9] = 2 * orientation.j * orientation.k + 2 * orientation.r * orientation.i;
            transformMatrix.Data[10] = 1 - 2 * orientation.i * orientation.i - 2 * orientation.j * orientation.j;
            transformMatrix.Data[11] = position.z;
        }

        #endregion

        #region Accessor Functions for the Rigid Body's State
        #endregion

        #region Retrieval Functions for Dynamic Quantities
        #endregion

        #region Force, Torque and Acceleration Set-up Functions
        #endregion
    }
}
