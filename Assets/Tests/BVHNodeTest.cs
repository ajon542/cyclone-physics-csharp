using UnityEngine;
/*
public class BVHNodeTest : MonoBehaviour
{
    private void Start()
    {
        TestLog.Instance.LogResult("BVHNodeTest.Test1", Test1());
    }

    public class CollisionObject
    {
        public Cyclone.BoundingSphere Volume { get; private set; }
        public Cyclone.RigidBody Body { get; private set; }

        public CollisionObject(Cyclone.Math.Vector3 position)
        {
            double radius = 1;
            Volume = new Cyclone.BoundingSphere(position, radius);
            Body = new Cyclone.RigidBody() {Position = position};
        }
    }

    /// <summary>
    /// Tests insertion of elements into the BVH tree.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test1()
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
        if (Cyclone.Core.Equals(root.volume.Radius, 11) && root.volume.Center == new Cyclone.Math.Vector3(0, 0, 0))
        {
            return true;
        }
        return false;
    }

    // TODO: Test to make sure the leaf nodes are the ones we actually added to the tree.
    // TODO: Test parent of two child nodes actually encompass them.
}
*/