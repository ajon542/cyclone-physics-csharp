using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BVHNodeTests
{
    // Test Insert, Remove, IsLeaf, GetPotentialContacts, GetPotentialContactsWith, Overlaps


    /// <summary>
    /// Container for a basic collision object with a bounding sphere of radius 1.
    /// </summary>
    public class CollisionObject
    {
        public Cyclone.BoundingSphere Volume { get; private set; }
        public Cyclone.RigidBody Body { get; private set; }

        public CollisionObject(Cyclone.Math.Vector3 position)
        {
            double radius = 1;
            Volume = new Cyclone.BoundingSphere(position, radius);
            Body = new Cyclone.RigidBody() { Position = position };
        }
    }

    /// <summary>
    /// Tests insertion of elements into the BVH tree.
    /// </summary>
    [Test]
    public void Test1()
    {
        CollisionObject co = new CollisionObject(new Cyclone.Math.Vector3(1, 0, 0));

        Cyclone.BVHNode root = new Cyclone.BVHNode(null, co.Volume, co.Body);

        Assert.IsTrue(Cyclone.Core.Equals(root.volume.Radius, 1));
    }

    /// <summary>
    /// Tests insertion of elements into the BVH tree.
    /// </summary>
    [Test]
    public void Test2()
    {
        CollisionObject co = new CollisionObject(new Cyclone.Math.Vector3(1, 0, 0));

        Cyclone.BVHNode root = new Cyclone.BVHNode(null, co.Volume, co.Body);

        co = new CollisionObject(new Cyclone.Math.Vector3(2, 0, 0));
        root.Insert(co.Body, co.Volume);

        Assert.IsTrue(Cyclone.Core.Equals(root.volume.Radius, 1.5));
        Assert.AreEqual(new Cyclone.Math.Vector3(1.5, 0, 0), root.volume.Center);
    }

    /// <summary>
    /// Tests insertion of elements into the BVH tree.
    /// </summary>
    [Test]
    public void Test3()
    {
        CollisionObject co = new CollisionObject(new Cyclone.Math.Vector3(-1, 0, 0));

        Cyclone.BVHNode root = new Cyclone.BVHNode(null, co.Volume, co.Body);

        co = new CollisionObject(new Cyclone.Math.Vector3(1, 0, 0));
        root.Insert(co.Body, co.Volume);

        Assert.IsTrue(Cyclone.Core.Equals(root.volume.Radius, 2));
        Assert.AreEqual(new Cyclone.Math.Vector3(0, 0, 0), root.volume.Center);
    }

    /// <summary>
    /// Tests insertion of elements into the BVH tree.
    /// </summary>
    [Test]
    public void Test20()
    {
        CollisionObject co = new CollisionObject(new Cyclone.Math.Vector3(1, 0, 0));

        Cyclone.BVHNode root = new Cyclone.BVHNode(null, co.Volume, co.Body);

        co = new CollisionObject(new Cyclone.Math.Vector3(2, 0, 0));
        root.Insert(co.Body, co.Volume);

        co = new CollisionObject(new Cyclone.Math.Vector3(8, 0, 0));
        root.Insert(co.Body, co.Volume);

        co = new CollisionObject(new Cyclone.Math.Vector3(-1, 0, 0));
        root.Insert(co.Body, co.Volume);

        co = new CollisionObject(new Cyclone.Math.Vector3(-5, 0, 0));
        root.Insert(co.Body, co.Volume);

        co = new CollisionObject(new Cyclone.Math.Vector3(10, 0, 0));
        root.Insert(co.Body, co.Volume);

        co = new CollisionObject(new Cyclone.Math.Vector3(-10, 0, 0));
        root.Insert(co.Body, co.Volume);

        co = new CollisionObject(new Cyclone.Math.Vector3(-9.5, 0, 0));
        root.Insert(co.Body, co.Volume);

        // Root bounding volume should be at 0,0,0 and have a radius of 11.
        Assert.IsTrue(Cyclone.Core.Equals(root.volume.Radius, 11));
        Assert.AreEqual(new Cyclone.Math.Vector3(0, 0, 0), root.volume.Center);
    }
}
