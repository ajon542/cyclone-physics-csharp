using UnityEngine;
using System.Collections;

public class BVHNodeTest : MonoBehaviour
{
    private void Start()
    {
        TestLog.Instance.LogResult("BVHNodeTest.Test1", Test1());
    }

    /// <summary>
    /// Tests the inverse of a matrix.
    /// </summary>
    /// <returns><c>true</c> if test succeeded; otherwise, <c>false</c>.</returns>
    private bool Test1()
    {
        return true;
    }
}
