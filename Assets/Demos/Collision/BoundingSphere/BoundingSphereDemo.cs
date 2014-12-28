using UnityEngine;
using Cyclone;

/// <summary>
/// A simple demo of bounding spheres.
/// </summary>
public class BoundingSphereDemo : MonoBehaviour
{
    public Transform sphere1;
    public Transform sphere2;

    private void Start()
    {
        BoundingSphere bs1 = new BoundingSphere(new Cyclone.Math.Vector3(sphere1.transform.position.x, sphere1.transform.position.y, sphere1.transform.position.z), 0.5);
        BoundingSphere bs2 = new BoundingSphere(new Cyclone.Math.Vector3(sphere2.transform.position.x, sphere2.transform.position.y, sphere2.transform.position.z), 0.5);

        BoundingSphere bounds = new BoundingSphere(bs1, bs2);

        transform.position = new Vector3((float)bounds.Center.x, (float)bounds.Center.y, (float)bounds.Center.z);
        transform.localScale = new Vector3((float)bounds.Radius*2, (float)bounds.Radius*2, (float)bounds.Radius*2);
    }
}