﻿using UnityEngine;
using Cyclone;

public class Aircraft : MonoBehaviour
{
    private AeroControl leftWing;
    private AeroControl rightWing;
    private AeroControl rudder;
    private Aero tail;
    private RigidBody aircraft;
    private ForceRegistry registry;

    private Cyclone.Math.Vector3 windspeed;

    private float leftWingControl;
    private float rightWingControl;
    private float rudderControl;

    private void Start()
    {
        aircraft = new RigidBody();
        windspeed = new Cyclone.Math.Vector3();

        rightWing = new AeroControl
            (
            new Cyclone.Math.Matrix3(0, 0, 0, -1, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, -0.995f, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, -1.005f, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Vector3(-1.0f, 0.0f, -2.0f),
            windspeed
            );

        leftWing = new AeroControl
            (
            new Cyclone.Math.Matrix3(0, 0, 0, -1, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, -0.995f, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, -1.005f, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Vector3(-1.0f, 0.0f, 2.0f),
            windspeed
            );

        rudder = new AeroControl
            (
            new Cyclone.Math.Matrix3(0, 0, 0, 0, 0, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, 0, 0, 0, 0.01f, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, 0, 0, 0, -0.01f, 0, 0),
            new Cyclone.Math.Vector3(2.0f, 0.5f, 0),
            windspeed
            );

        tail = new Aero
            (
            new Cyclone.Math.Matrix3(0, 0, 0, -1, -0.5f, 0, 0, 0, -0.1f),
            new Cyclone.Math.Vector3(2.0f, 0, 0),
            windspeed
            );

        ResetPlane();

        aircraft.Mass = 2.5f;
        Cyclone.Math.Matrix3 it = new Cyclone.Math.Matrix3();
        it.SetBlockInertiaTensor(new Cyclone.Math.Vector3(1, 1, 1), 1);
        aircraft.SetInertiaTensor(it);

        aircraft.SetDamping(0.8f, 0.8f);

        aircraft.SetAcceleration(new Cyclone.Math.Vector3(0, -10, 0));
        aircraft.CalculateDerivedData();

        aircraft.SetAwake();
        aircraft.SetCanSleep(false);

        registry = new ForceRegistry();
        registry.Add(aircraft, leftWing);
        registry.Add(aircraft, rightWing);
        registry.Add(aircraft, rudder);
        registry.Add(aircraft, tail);
    }

    private void Update()
    {
        // Update the aircraft controls.
        if(Input.GetKeyDown(KeyCode.A))
        {
            leftWingControl += 0.1f;
            rightWingControl -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            leftWingControl -= 0.1f;
            rightWingControl += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            leftWingControl -= 0.1f;
            rightWingControl -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            leftWingControl += 0.1f;
            rightWingControl += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rudderControl += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rudderControl -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            leftWingControl = 0;
            rightWingControl = 0;
            rudderControl = 0;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlane();
        }

        // Make sure the controls are in range.
        Clamp(ref leftWingControl);
        Clamp(ref rightWingControl);
        Clamp(ref rudderControl);

        leftWing.SetControl(leftWingControl);
        rightWing.SetControl(rightWingControl);
        rudder.SetControl(rudderControl);

        double duration = Time.deltaTime;

        // Add the propeller force
        Cyclone.Math.Vector3 propulsion = new Cyclone.Math.Vector3(-10, 0, 0);
        propulsion = aircraft.GetTransform().TransformDirection(propulsion);
        aircraft.AddForce(propulsion);

        // Add the forces acting on the aircraft.
        registry.UpdateForces(duration);

        // Update the aircraft's physics.
        aircraft.Integrate(duration);

        // Do a very basic collision detection and response with the ground.
        Cyclone.Math.Vector3 pos = aircraft.GetPosition();
        if (pos.y < 3.0f)
        {
            pos.y = 3.0f;
            aircraft.SetPosition(pos);
        }

        SetObjectPosition(aircraft.Position);
    }

    private void ResetPlane()
    {
        aircraft.SetPosition(0, 0, 0);
        aircraft.SetOrientation(1, 0, 0, 0);

        aircraft.SetVelocity(0, 0, 0);
        aircraft.SetRotation(0, 0, 0);

        leftWingControl = 0;
        rightWingControl = 0;
        rudderControl = 0;
    }

    /// <summary>
    /// Helper method to convert a Cyclone.Math.Vector3 to a UnityEngine.Vector3 position.
    /// </summary>
    /// <param name="position">The position.</param>
    private void SetObjectPosition(Cyclone.Math.Vector3 position)
    {
        transform.position = new Vector3((float)position.x, (float)position.y, (float)position.z);

        Cyclone.Math.Quaternion r = aircraft.GetOrientation();
        transform.rotation = new Quaternion((float)r.i, (float)r.j, (float)r.k, (float)r.r);
    }

    private void Clamp(ref float value)
    {
        if (value < -1.0f)
        {
            value = -1.0f;
        }
        else if (value > 1.0f)
        {
            value = 1.0f;
        }
    }
}