using UnityEngine;
using Cyclone;

public class Aircraft : MonoBehaviour
{
    private AeroControl leftWing;
    private AeroControl rightWing;
    private AeroControl rudder;
    private Aero tail;
    private RigidBody aircraft;
    //private ForceRegistry registry;

    private Cyclone.Math.Vector3 windspeed;

    private float leftWingControl;
    private float rightWingControl;
    private float rudderControl;

    private void Start()
    {
        windspeed = new Cyclone.Math.Vector3();

        rightWing = new AeroControl
            (
            new Cyclone.Math.Matrix3(0, 0, 0, -1, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, -0.995f, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Matrix3(0, 0, 0, -1.005f, -0.5f, 0, 0, 0, 0),
            new Cyclone.Math.Vector3(-1.0f, 0.0f, 2.0f),
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
        it.SetBlockInertiaTensor(new Cyclone.Math.Vector3(2, 1, 1), 1);
        aircraft.SetInertiaTensor(it);

        aircraft.SetDamping(0.8f, 0.8f);

        aircraft.SetAcceleration(new Cyclone.Math.Vector3(0, -10, 0));
        aircraft.CalculateDerivedData();

        aircraft.SetAwake();
        aircraft.SetCanSleep(false);

        //registry.Add(aircraft, leftWing);
        //registry.add(&aircraft, &right_wing);
        //registry.add(&aircraft, &rudder);
        //registry.add(&aircraft, &tail);
    }

    private void ResetPlane()
    {
        aircraft.SetPosition(0, 0, 0);
        aircraft.SetOrientation(1, 0, 0, 0);

        aircraft.SetVelocity(0, 0, 0);
        aircraft.SetRotation(0, 0, 0);
    }
}