using Cyclone.Math;

namespace Cyclone
{
    /// <summary>
    /// A force generator can be asked to add a force to one or more bodies.
    /// </summary>
    public class Gravity : IForceGenerator
    {
        /// <summary>
        /// Holds the acceleration due to gravity.
        /// </summary>
        private Vector3 gravity;

        /// <summary>
        /// Creates a new instance of the <see cref="Gravity"/> class.
        /// </summary>
        /// <param name="gravity">The acceleration due to gravity.</param>
        public Gravity(Vector3 gravity)
        {
            this.gravity = gravity;
        }

        /// <summary>
        /// Applies the gravitational force to the given rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(RigidBody body, double duration)
        {
            // Check that we do not have infinite mass.
            if(body.HasFiniteMass() == false)
            {
                return;
            }

            // Apply the mass-scaled force to the body.
            body.AddForce(gravity * body.Mass);
        }
    }

    /// <summary>
    /// A force generator that applies a Spring force.
    /// </summary>
    public class Spring : IForceGenerator
    {
        /// <summary>
        /// The point of connection of the spring, in local
        /// coordinates.
        /// </summary>
        private Vector3 connectionPoint;

        /// <summary>
        /// The point of connection of the spring to the other object,
        /// in that object's local coordinates.
        /// </summary>
        private Vector3 otherConnectionPoint;

        /// <summary>
        /// The particle at the other end of the spring.
        /// </summary>
        private RigidBody other;

        /// <summary>
        /// The spring constant.
        /// </summary>
        private double springConstant;

        /// <summary>
        /// The rest length of the spring.
        /// </summary>
        private double restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="Spring"/> class.
        /// </summary>
        /// <param name="connectionPoint">The point of connection of the spring, in local
        /// coordinates.</param>
        /// <param name="other">The particle at the other end of the spring.</param>
        /// <param name="otherConnectionPoint">The point of connection of the spring to the other object,
        /// in that object's local coordinates.</param>
        /// <param name="springConstant">The spring constant.</param>
        /// <param name="restLength">The rest length of the spring.</param>
        public Spring(Vector3 connectionPoint,
               RigidBody other,
               Vector3 otherConnectionPoint,
               double springConstant,
               double restLength)
        {
            this.connectionPoint = connectionPoint;
            this.otherConnectionPoint = otherConnectionPoint;
            this.other = other;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Applies the spring force to the given rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public virtual void UpdateForce(RigidBody body, double duration)
        {
            // Calculate the two ends in world space
            Vector3 lws = body.GetPointInWorldSpace(connectionPoint);
            Vector3 ows = other.GetPointInWorldSpace(otherConnectionPoint);

            // Calculate the vector of the spring
            Vector3 force = lws - ows;

            // Calculate the magnitude of the force
            double magnitude = force.Magnitude;
            // TODO: Not sure why this Abs calculation is used here.
            // If the distance between the two particles is less than the restLength,
            // the particles have a force which pulls them together. I would have expected
            // the two particles to push apart.
            //magnitude = System.Math.Abs(magnitude - restLength);
            magnitude -= restLength;
            magnitude *= springConstant;

            // Calculate the final force and apply it
            force.Normalize();
            force *= -magnitude;
            body.AddForceAtPoint(force, lws);
        }
    };

    /// <summary>
    /// A force generator that applies an aerodynamic force.
    /// </summary>
    public class Aero : IForceGenerator
    {

        /// <summary>
        /// Gets or sets the aerodynamic tensor for the surface in body
        /// space.
        /// </summary>
        protected Matrix3 Tensor { get; set; }

        /// <summary>
        /// Gets or sets the relative position of the aerodynamic surface in
        /// body coordinates.
        /// </summary>
        private Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the windspeed of the environment. This is easier
        /// than managing a separate windspeed vector per generator
        /// and having to update it manually as the wind changes.
        /// </summary>
        private Vector3 Windspeed { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Aero"/> class.
        /// </summary>
        /// <param name="tensor">The aero dynamic tensor for the surface.</param>
        /// <param name="position">The position of the aerodynamic surface.</param>
        /// <param name="windspeed">The windspeed of the environment.</param>
        public Aero(Matrix3 tensor, Vector3 position, Vector3 windspeed)
        {
            Tensor = tensor;
            Position = position;
            Windspeed = windspeed;
        }

        /// <summary>
        /// Applies the aerodynamic force to the given rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public virtual void UpdateForce(RigidBody body, double duration)
        {
            UpdateForceFromTensor(body, duration, Tensor);
        }

        /// <summary>
        /// Uses an explicit tensor matrix to update the force on
        /// the given rigid body. This is exactly the same as for UpdateForce
        /// only it takes an explicit tensor.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">The time interval over which to update the force.</param>
        /// <param name="tensor">The tensor matrix to apply.</param>
        protected void UpdateForceFromTensor(RigidBody body, double duration, Matrix3 tensor)
        {
            // Calculate total velocity (windspeed and body's velocity).
            Vector3 velocity = body.GetVelocity();
            velocity += Windspeed;

            // Calculate the velocity in body coordinates
            Vector3 bodyVel = body.GetTransform().TransformInverseDirection(velocity);

            // Calculate the force in body coordinates
            Vector3 bodyForce = tensor.Transform(bodyVel);
            Vector3 force = body.GetTransform().TransformDirection(bodyForce);

            // Apply the force
            body.AddForceAtBodyPoint(force, Position);
        }
    }

    /// <summary>
    /// A force generator with a control aerodynamic surface. This
    /// requires three inertia tensors, for the two extremes and
    /// 'resting' position of the control surface.  The latter tensor is
    /// the one inherited from the base class, the two extremes are
    /// defined in this class.
    /// </summary>
    public class AeroControl : Aero
    {
        /// <summary>
        /// The aerodynamic tensor for the surface, when the control is at
        /// its maximum value.
        /// </summary>
        private Matrix3 maxTensor;

        /// <summary>
        /// The aerodynamic tensor for the surface, when the control is at
        /// its minimum value.
        /// </summary>
        private Matrix3 minTensor;

        /// <summary>
        /// The current position of the control for this surface. This
        /// should range between -1 (in which case the minTensor value
        /// is used), through 0 (where the base-class tensor value is
        /// used) to +1 (where the maxTensor value is used).
        /// </summary>
        private double controlSetting;

        /// <summary>
        /// Calculates the final aerodynamic tensor for the current
        /// control setting.
        /// </summary>
        /// <returns>The aerodynamic tensor for the current control setting.</returns>
        private Matrix3 GetTensor()
        {
            if (controlSetting <= -1.0f)
            {
                return minTensor;
            }
            else if (controlSetting >= 1.0f)
            {
                return maxTensor;
            }
            else if (controlSetting < 0)
            {
                return Matrix3.LinearInterpolate(minTensor, Tensor, controlSetting + 1.0f);
            }
            else if (controlSetting > 0)
            {
                return Matrix3.LinearInterpolate(Tensor, maxTensor, controlSetting);
            }
            
            return Tensor;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AeroControl"/> class.
        /// </summary>
        /// <param name="baseTensor">The aero dynamic tensor for the surface.</param>
        /// <param name="min">
        /// The aerodynamic tensor for the surface, when the control is at
        /// its minimum value.
        /// </param>
        /// <param name="max">
        /// The aerodynamic tensor for the surface, when the control is at
        /// its maximum value.
        /// </param>
        /// <param name="position">The position of the aerodynamic surface.</param>
        /// <param name="windspeed">The windspeed of the environment.</param>
        public AeroControl(Matrix3 baseTensor, Matrix3 min, Matrix3 max, Vector3 position, Vector3 windspeed)
            : base(baseTensor, position, windspeed)
        {
            minTensor = min;
            maxTensor = max;
            controlSetting = 0.0f;
        }

        /// <summary>
        /// Sets the control position of this control.
        /// </summary>
        /// <remarks>
        /// This value given should be between -1 (in which case the minTensor
        /// value is used), through 0 (where the base-class tensor value is used)
        /// to +1 (where the maxTensor value is used). Values outside that
        /// range give undefined results.
        /// </remarks>
        /// <param name="value">The control position of this control.</param>
        public void SetControl(double value)
        {
            controlSetting = value;
        }

        /// <summary>
        /// Applies the aerodynamic force to the given rigid body.
        /// </summary>
        /// <param name="body">The rigid body.</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public override void UpdateForce(RigidBody body, double duration)
        {
            Matrix3 tensor = GetTensor();
            UpdateForceFromTensor(body, duration, tensor);
        }
    };
}
