using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyclone.Math;

namespace UnitTests
{
    /// <summary>
    /// Test class for Vector3.
    /// </summary>
    [TestClass]
    public class Vector3Tests
    {
        /// <summary>
        /// Tests the addition of all components of a vector.
        /// </summary>
        [TestMethod]
        public void TestAddition()
        {
            Vector3 v1 = new Vector3(0, 0, 0);
            Vector3 v2 = new Vector3(0, 0, 0);

            v2.x += 10;
            Vector3 result = v1 + v2;
            Assert.AreEqual(result, new Vector3(10, 0, 0));

            v2.y += 10;
            result += v2;
            Assert.AreEqual(result, new Vector3(20, 10, 0));

            v2.z += 10;
            result += v2;
            Assert.AreEqual(result, new Vector3(30, 20, 10));
        }

        /// <summary>
        /// Tests the subtraction of all components of a vector.
        /// </summary>
        [TestMethod]
        public void TestSubtraction()
        {
            Vector3 v1 = new Vector3(0, 0, 0);
            Vector3 v2 = new Vector3(0, 0, 0);

            v2.x -= 10;
            Vector3 result = v1 + v2;
            Assert.AreEqual(result, new Vector3(-10, 0, 0));

            v2.y += -10;
            result += v2;
            Assert.AreEqual(result, new Vector3(-20, -10, 0));

            v2.z += -10;
            result += v2;
            Assert.AreEqual(result, new Vector3(-30, -20, -10));
        }

        /// <summary>
        /// Tests the dot (scalar) product.
        /// </summary>
        [TestMethod]
        public void TestDotProduct1()
        {
            // The vector class has two methods for calculating dot product.
            // Make sure they produce the same results.
            Vector3 v1 = new Vector3(1, 2, 3);
            Vector3 v2 = new Vector3(1, 2, 3);

            Assert.AreEqual(v1 * v2, v1.ScalarProduct(v2));
            Assert.AreEqual(v1 * v2, v2.ScalarProduct(v1));
            Assert.AreEqual(v1 * v2, v2 * v1);
        }
    }
}
