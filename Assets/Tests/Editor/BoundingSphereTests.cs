using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BoundingSphereTests
{
    /// <summary>
    /// Test the construction of a bounding sphere object.
    /// </summary>
    [Test]
    public void Test1()
    {
        var center = new Cyclone.Math.Vector3(0, 0, 0);
        double radius = 200;
        var bs = new Cyclone.BoundingSphere(center, radius);

        Assert.AreEqual(center, bs.Center); 
        Assert.AreEqual(radius, bs.Radius);
    }

    /// <summary>
    /// Test the construction of a bounding sphere object.
    /// </summary>
    [Test]
    public void Test2()
    {
        var one = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, -100, 0), 1);
        var two = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, 100, 0), 1);

        var bs = new Cyclone.BoundingSphere(one, two);

        Assert.AreEqual(new Cyclone.Math.Vector3(0, 0, 0), bs.Center);
        Assert.AreEqual(101, bs.Radius);
    }

    /// <summary>
    /// Test bounding sphere overlapping.
    /// </summary>
    [Test]
    public void Test3()
    {
        var one = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, -100, 0), 1);
        var two = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, 100, 0), 1);

        Assert.IsFalse(one.Overlaps(two));
        Assert.IsFalse(two.Overlaps(one));
    }

    /// <summary>
    /// Test bounding sphere overlapping.
    /// </summary>
    [Test]
    public void Test4()
    {
        var one = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, 0, 0), 1);
        var two = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, 0, 0.5), 1);

        Assert.IsTrue(one.Overlaps(two));
        Assert.IsTrue(two.Overlaps(one));
    }

    /// <summary>
    /// Test bounding sphere overlapping.
    /// </summary>
    [Test]
    public void Test5()
    {
        var one = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, 0, 0), 1);
        var two = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(0, 2, 0), 1);

        Assert.IsFalse(one.Overlaps(two));
        Assert.IsFalse(two.Overlaps(one));
    }
}
