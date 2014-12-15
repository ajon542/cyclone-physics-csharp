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

        /// <summary>
        /// Holds the inverse of the body's inertia tensor. The
        /// inertia tensor provided must not be degenerate
        /// (that would mean the body had zero inertia for
        /// spinning along one axis). As long as the tensor is
        /// finite, it will be invertible. The inverse tensor
        /// is used for similar reasons to the use of inverse
        /// mass.
        ///
        /// The inertia tensor, unlike the other variables that
        /// define a rigid body, is given in body space.
        /// </summary>
        protected Matrix3 InverseInertiaTensor { get; set; }

        /// <summary>
        /// Holds the amount of damping applied to angular
        /// motion. Damping is required to remove energy added
        /// through numerical instability in the integrator.
        /// </summary>
        protected double AngularDamping { get; set; }

        #endregion

        /// These data members hold information that is derived from
        /// the other data in the class.
        #region Derived Data

        /// <summary>
        /// Holds the inverse inertia tensor of the body in world
        /// space. The inverse inertia tensor member is specified in
        /// the body's local space.
        /// </summary>
        Matrix3 inverseInertiaTensorWorld;

        /// <summary>
        /// Holds the amount of motion of the body. This is a recency
        /// weighted mean that can be used to put a body to sleap.
        /// </summary>
        double motion;

        /// <summary>
        /// A body can be put to sleep to avoid it being updated
        /// by the integration functions or affected by collisions
        /// with the world.
        /// </summary>
        bool isAwake;

        /// <summary>
        /// Some bodies may never be allowed to fall asleep.
        /// User controlled bodies, for example, should be
        /// always awake.
        /// </summary>
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

        /// <summary>
        /// Holds the accumulated force to be applied at the next
        /// integration step.
        /// </summary> 
        Vector3 forceAccum;

        /// <summary>
        /// Holds the accumulated torque to be applied at the next
        /// integration step.
        /// </summary>
        Vector3 torqueAccum;

        /// <summary>
        /// Holds the acceleration of the rigid body.  This value
        /// can be used to set acceleration due to gravity (its primary
        /// use), or any other constant acceleration.
        /// </summary>
        Vector3 acceleration;

        /// <summary>
        /// Holds the linear acceleration of the rigid body, for the
        /// previous frame.
        /// </summary>
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
            throw new NotImplementedException();
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

        /// <summary>
        /// Integrates the rigid body forward in time by the given amount.
        /// This function uses a Newton-Euler integration method, which is a
        /// linear approximation to the correct integral. For this reason it
        /// may be inaccurate in some cases.
        /// </summary>
        /// <param name="duration">
        /// Time interval over which to update the position and velocity.
        /// This is currently the time between frames.
        /// </param>
        void Integrate(double duration)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Accessor Functions for the Rigid Body's State

        /// <summary>
        /// Sets the mass of the rigid body.
        /// </summary>
        /// <remarks>
        /// This invalidates internal data for the rigid body.
        /// Either an integration function, or the calculateInternals
        /// function should be called before trying to get any settings
        /// from the rigid body.
        /// </remarks>
        /// <param name="mass">
        /// The new mass of the body. This may not be zero.
        /// Small masses can produce unstable rigid bodies under
        /// simulation.
        /// </param>
        void setMass(double mass)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the mass of the rigid body.
        /// </summary>
        /// <returns>The current mass of the rigid body.</returns>
        double getMass()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the inverse mass of the rigid body.
        /// @param inverseMass The new inverse mass of the body. This
        /// may be zero, for a body with infinite mass (i.e. unmovable).
        /// </summary>
        /// <remarks>
        /// This invalidates internal data for the rigid body.
        /// Either an integration function, or the calculateInternals
        /// function should be called before trying to get any settings
        /// from the rigid body.
        /// </remarks>
        /// <param name="inverseMass"></param>
        void setInverseMass(double inverseMass)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the inverse mass of the rigid body.
        /// </summary>
        /// <returns>The current inverse mass of the rigid body.</returns>
        double getInverseMass()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if the mass of the body is not-infinite.
        /// </summary>
        /// <returns></returns>
        bool hasFiniteMass()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the intertia tensor for the rigid body.
        /// </summary>
        /// <remarks>
        /// This invalidates internal data for the rigid body.
        /// Either an integration function, or the calculateInternals
        /// function should be called before trying to get any settings
        /// from the rigid body.
        /// </remarks>
        /// <param name="inertiaTensor">
        /// The inertia tensor for the rigid
        /// body. This must be a full rank matrix and must be
        /// invertible.
        /// </param>
        void setInertiaTensor(Matrix3 inertiaTensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the current inertia tensor of the rigid body into
        /// the given matrix.
        /// </summary>
        /// <param name="inertiaTensor">
        /// A matrix to hold the current inertia tensor of the rigid body.
        /// The inertia tensor is expressed in the rigid body's local space.
        /// </param>
        void getInertiaTensor(Matrix3 inertiaTensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a copy of the current inertia tensor of the rigid body.
        /// </summary>
        /// <returns>
        /// A new matrix containing the current intertia tensor.
        /// The inertia tensor is expressed in the rigid body's
        /// local space.
        /// </returns>
        Matrix3 getInertiaTensor()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the current inertia tensor of the rigid body into
        /// the given matrix.
        /// </summary>
        /// <param name="inertiaTensor">
        /// A matrix to hold the current inertia tensor of the rigid body.
        /// The inertia tensor is expressed in world space.
        /// </param>
        void getInertiaTensorWorld(Matrix3 inertiaTensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a copy of the current inertia tensor of the rigid body.
        /// </summary>
        /// <returns>
        /// A new matrix containing the current intertia tensor.
        /// The inertia tensor is expressed in world space.
        /// </returns>
        Matrix3 getInertiaTensorWorld()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the inverse intertia tensor for the rigid body.
        /// </summary>
        /// <remarks>
        /// This invalidates internal data for the rigid body.
        /// Either an integration function, or the calculateInternals
        /// function should be called before trying to get any settings
        /// from the rigid body.
        /// </remarks>
        /// <param name="inverseInertiaTensor">
        /// The inverse inertia tensor for the rigid body.
        /// This must be a full rank matrix and must be invertible.
        /// </param>
        void setInverseInertiaTensor(Matrix3 inverseInertiaTensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the current inverse inertia tensor of the rigid body
        /// into the given matrix.
        /// </summary>
        /// <param name="inverseInertiaTensor">
        /// A matrix to hold the current inverse inertia tensor of the rigid body.
        /// The inertia tensor is expressed in the rigid body's local space.
        /// </param>
        void getInverseInertiaTensor(Matrix3 inverseInertiaTensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a copy of the current inverse inertia tensor of the
        /// rigid body.
        /// </summary>
        /// <returns>
        /// A new matrix containing the current inverse
        /// intertia tensor. The inertia tensor is expressed in the
        /// rigid body's local space.
        /// </returns>
        Matrix3 getInverseInertiaTensor()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the current inverse inertia tensor of the rigid body
        /// into the given matrix.
        /// </summary>
        /// <param name="inverseInertiaTensor">
        /// A matrix to hold the current inverse inertia tensor of the rigid body.
        /// The inertia tensor is expressed in world space.
        /// </param>
        void getInverseInertiaTensorWorld(Matrix3 inverseInertiaTensor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a copy of the current inverse inertia tensor of the
        /// rigid body.
        /// </summary>
        /// <returns>
        /// A new matrix containing the current inverse
        /// intertia tensor. The inertia tensor is expressed in world
        /// space.
        /// </returns>
        Matrix3 getInverseInertiaTensorWorld()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets both linear and angular damping in one function call.
        /// </summary>
        /// <param name="linearDamping">The speed that velocity is shed from the rigid body.</param>
        /// <param name="angularDamping">The speed that rotation is shed from the rigid body.</param>
        void setDamping(double linearDamping, double angularDamping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the linear damping for the rigid body.
        /// </summary>
        /// <param name="linearDamping">The speed that velocity is shed from the rigid body.</param>
        void setLinearDamping(double linearDamping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the current linear damping value.
        /// </summary>
        /// <returns>The current linear damping value.</returns>
        double getLinearDamping()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the angular damping for the rigid body.
        /// </summary>
        /// <param name="angularDamping">The speed that rotation is shed from the rigid body.</param>
        void setAngularDamping(double angularDamping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the current angular damping value.
        /// </summary>
        /// <returns>The current angular damping value.</returns>
        double getAngularDamping()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the position of the rigid body.
        /// </summary>
        /// <param name="position">The new position of the rigid body.</param>
        void setPosition(Vector3 position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the position of the rigid body by component.
        /// </summary>
        /// <param name="x">The x coordinate of the new position of the rigid body.</param>
        /// <param name="y">The y coordinate of the new position of the rigid body.</param>
        /// <param name="z">The z coordinate of the new position of the rigid body.</param>
        void setPosition(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given vector with the position of the rigid body.
        /// </summary>
        /// <param name="position">A pointer to a vector into which to write the position.</param>
        void getPosition(Vector3 position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the position of the rigid body.
        /// </summary>
        /// <returns>The position of the rigid body.</returns>
        Vector3 getPosition()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the orientation of the rigid body.
        /// </summary>
        /// <remarks>
        /// The given orientation does not need to be normalised,
        /// and can be zero. This function automatically constructs a
        /// valid rotation quaternion with (0,0,0,0) mapping to
        /// (1,0,0,0). 
        /// </remarks>
        /// <param name="orientation">The new orientation of the rigid body.</param>
        void setOrientation(Quaternion orientation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the orientation of the rigid body by component.
        /// </summary>
        /// <remarks>
        /// The given orientation does not need to be normalised,
        /// and can be zero. This function automatically constructs a
        /// valid rotation quaternion with (0,0,0,0) mapping to
        /// </remarks>
        /// <param name="r">The real component of the rigid body's orientation quaternion.</param>
        /// <param name="i">The first complex component of the rigid body's orientation quaternion.</param>
        /// <param name="j">The second complex component of the rigid body's orientation quaternion.</param>
        /// <param name="k">The third complex component of the rigid body's orientation quaternion.</param>
        void setOrientation(double r, double i, double j, double k)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given quaternion with the current value of the
        /// rigid body's orientation.
        /// </summary>
        /// <param name="orientation">A quaternion to receive the orientation data.</param>
        void getOrientation(Quaternion orientation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the orientation of the rigid body.
        /// </summary>
        /// <returns>The orientation of the rigid body.</returns>
        Quaternion getOrientation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given matrix with a transformation representing
        /// the rigid body's orientation.
        /// </summary>
        /// <remarks>
        /// Transforming a direction vector by this matrix turns
        /// it from the body's local space to world space.
        /// </remarks>
        /// <param name="matrix">A matrix to fill.</param>
        void getOrientation(Matrix3 matrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given matrix data structure with a transformation
        /// representing the rigid body's orientation.
        /// </summary>
        /// <remarks>
        /// Transforming a direction vector by this matrix turns
        /// it from the body's local space to world space.
        /// </remarks>
        /// <param name="matrix">The matrix to fill.</param>
        void getOrientation(/*double matrix[9]*/ double[] matrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given matrix with a transformation representing
        /// the rigid body's position and orientation.
        /// </summary>
        /// <remarks>
        /// Transforming a vector by this matrix turns it from
        /// the body's local space to world space.
        /// </remarks>
        /// <param name="transform">The matrix to fill.</param>
        void getTransform(Matrix4 transform)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given matrix data structure with a
        /// transformation representing the rigid body's position and
        /// orientation.
        /// </summary>
        /// <remarks>
        /// Transforming a vector by this matrix turns it from
        /// the body's local space to world space.
        /// </remarks>
        /// <param name="matrix">The matrix to fill.</param>
        void getTransform( /*real matrix[16]*/ double[] matrix)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Fills the given matrix data structure with a
        /// transformation representing the rigid body's position and
        /// orientation. The matrix is transposed from that returned
        /// by getTransform. This call returns a matrix suitable
        /// for applying as an OpenGL transform.
        /// </summary>
        /// <remarks>
        /// Transforming a vector by this matrix turns it from
        /// the body's local space to world space.
        /// </remarks>
        /// <param name="matrix">A matrix to fill.</param>
        void getGLTransform( /*float matrix[16]*/ float[] matrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a transformation representing the rigid body's
        /// position and orientation.
        /// </summary>
        /// <remarks>
        /// Transforming a vector by this matrix turns it from
        /// the body's local space to world space.
        /// </remarks>
        /// <returns>The transform matrix for the rigid body.</returns>
        Matrix4 getTransform()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given point from world space into the body's
        /// local space.
        /// </summary>
        /// <param name="point">The point to covert, given in world space.</param>
        /// <returns>The converted point, in local space.</returns>
        Vector3 getPointInLocalSpace(Vector3 point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given point from world space into the body's
        /// local space.
        /// </summary>
        /// <param name="point">The point to covert, given in local space.</param>
        /// <returns>The converted point, in world space.</returns>
        Vector3 getPointInWorldSpace(Vector3 point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given direction from world space into the
        /// body's local space.
        /// @note When a direction is converted between frames of
        /// reference, there is no translation required.
        /// </summary>
        /// <param name="direction">The direction to covert, given in world space.</param>
        /// <returns>The converted direction, in local space.</returns>
        Vector3 getDirectionInLocalSpace(Vector3 direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given direction from world space into the
        /// body's local space.
        /// @note When a direction is converted between frames of
        /// reference, there is no translation required.
        /// </summary>
        /// <param name="direction">The direction to covert, given in local space.</param>
        /// <returns>The converted direction, in world space.</returns>
        Vector3 getDirectionInWorldSpace(Vector3 direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the velocity of the rigid body.
        /// </summary>
        /// <param name="velocity">
        /// The new velocity of the rigid body. The velocity is given in world space.
        /// </param>
        void setVelocity(Vector3 velocity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the velocity of the rigid body by component. The
        /// velocity is given in world space.
        /// </summary>
        /// <param name="x">The x coordinate of the new velocity of the rigid body.</param>
        /// <param name="y">The y coordinate of the new velocity of the rigid body.</param>
        /// <param name="z">The z coordinate of the new velocity of the rigid body.</param>
        void setVelocity(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given vector with the velocity of the rigid body.
        /// </summary>
        /// <param name="velocity">
        /// A vector into which to write the velocity.
        /// The velocity is given in world local space.
        /// </param>
        void getVelocity(Vector3 velocity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the velocity of the rigid body.
        /// </summary>
        /// <returns>
        /// The velocity of the rigid body. The velocity is
        /// given in world local space.
        /// </returns>
        Vector3 getVelocity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies the given change in velocity.
        /// </summary>
        /// <param name="deltaVelocity"></param>
        void addVelocity(Vector3 deltaVelocity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the rotation of the rigid body.
        /// </summary>
        /// <param name="rotation">
        /// The new rotation of the rigid body. The rotation is given in world space.
        /// </param>
        void setRotation(Vector3 rotation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the rotation of the rigid body by component. The
        /// rotation is given in world space.
        /// </summary>
        /// <param name="x">The x coordinate of the new rotation of the rigid body.</param>
        /// <param name="y">The y coordinate of the new rotation of the rigid body.</param>
        /// <param name="z">The z coordinate of the new rotation of the rigid body.</param>
        void setRotation(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given vector with the rotation of the rigid body.
        /// </summary>
        /// <param name="rotation">
        /// A vector into which to write the rotation. 
        /// The rotation is given in world local space.
        /// </param>
        void getRotation(Vector3 rotation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the rotation of the rigid body.
        /// </summary>
        /// <returns>
        /// The rotation of the rigid body. The rotation is
        /// given in world local space.
        /// </returns>
        Vector3 getRotation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies the given change in rotation.
        /// </summary>
        /// <param name="deltaRotation"></param>
        void addRotation(Vector3 deltaRotation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if the body is awake and responding to integration.
        /// </summary>
        /// <returns></returns>
        bool getAwake()
        {
            return isAwake;
        }

        /// <summary>
        /// Sets the awake state of the body. If the body is set to be
        /// not awake, then its velocities are also cancelled, since
        /// a moving body that is not awake can cause problems in the
        /// simulation.
        /// </summary>
        /// <param name="awake">The new awake state of the body.</param>
        void setAwake(bool awake = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if the body is allowed to go to sleep at any time.
        /// </summary>
        /// <returns></returns>
        bool GetCanSleep()
        {
            return canSleep;
        }

        /// <summary>
        /// Sets whether the body is ever allowed to go to sleep. Bodies
        /// under the player's control, or for which the set of
        /// transient forces applied each frame are not predictable,
        /// should be kept awake.
        /// </summary>
        /// <param name="canSleep">Whether the body can now be put to sleep.</param>
        void SetCanSleep(bool canSleep=true)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Retrieval Functions for Dynamic Quantities

        /// <summary>
        /// Fills the given vector with the current accumulated value
        /// for linear acceleration. The acceleration accumulators
        /// are set during the integration step. They can be read to
        /// determine the rigid body's acceleration over the last
        /// integration step. The linear acceleration is given in world
        /// space.
        /// </summary>
        /// <param name="linearAcceleration">
        /// A vector to receive the linear acceleration data.
        /// </param>
        void getLastFrameAcceleration(Vector3 linearAcceleration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the current accumulated value for linear
        /// acceleration. The acceleration accumulators are set during
        /// the integration step. They can be read to determine the
        /// rigid body's acceleration over the last integration
        /// step. The linear acceleration is given in world space.
        /// </summary>
        /// <returns>The rigid body's linear acceleration.</returns>
        Vector3 getLastFrameAcceleration()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Force, Torque and Acceleration Set-up Functions

        /// <summary>
        /// Clears the forces and torques in the accumulators. This will
        /// be called automatically after each intergration step.
        /// </summary>
        void clearAccumulators()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the given force to centre of mass of the rigid body.
        /// The force is expressed in world-coordinates.
        /// </summary>
        /// <param name="force">The force to apply.</param>
        void addForce(Vector3 force)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the given force to the given point on the rigid body.
        /// Both the force and the application point are given in world space.
        /// Because the force is not applied at the centre of mass, it may be split
        /// into both a force and torque.
        /// </summary>
        /// <param name="force">The force to apply.</param>
        /// <param name="point">The location at which to apply the force, in world-coordinates.</param>
        void addForceAtPoint(Vector3 force, Vector3 point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the given force to the given point on the rigid body.
        /// The direction of the force is given in world coordinates,
        /// but the application point is given in body space. This is
        /// useful for spring forces, or other forces fixed to the
        /// body.
        /// </summary>
        /// <param name="force">The force to apply.</param>
        /// <param name="point">The location at which to apply the force, in body-coordinates.</param>
        void addForceAtBodyPoint(Vector3 force, Vector3 point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the given torque to the rigid body.
        /// The force is expressed in world-coordinates.
        /// </summary>
        /// <param name="torque">The torque to apply.</param>
        void addTorque(Vector3 torque)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the constant acceleration of the rigid body.
        /// </summary>
        /// <param name="acceleration">The new acceleration of the rigid body.</param>
        void setAcceleration(Vector3 acceleration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the constant acceleration of the rigid body by component.
        /// </summary>
        /// <param name="x">The x coordinate of the new acceleration of the rigid body.</param>
        /// <param name="y">The y coordinate of the new acceleration of the rigid body.</param>
        /// <param name="z">The z coordinate of the new acceleration of the rigid body.</param>
        void setAcceleration(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the given vector with the acceleration of the rigid body.
        /// </summary>
        /// <param name="acceleration">
        /// A vector into which to write the acceleration. The acceleration is given in world local space.
        /// </param>
        void getAcceleration(Vector3 acceleration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the acceleration of the rigid body.
        /// </summary>
        /// <returns>
        /// The acceleration of the rigid body. The acceleration is
        /// given in world local space.
        /// </returns>
        Vector3 getAcceleration()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
