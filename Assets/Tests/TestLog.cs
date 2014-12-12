using UnityEngine;

/// <summary>
/// Helper class for logging test success and failure to the console.
/// </summary>
public class TestLog : MonoBehaviour
{
    private static TestLog instance;

    public static TestLog Instance
    {
        get { return instance ?? (instance = FindObjectOfType<TestLog>()); }
    }

    public void LogResult(string testId, bool result)
    {
        if (result)
        {
            Debug.Log(testId + " SUCCEEDED");
        }
        else
        {
            Debug.LogError(testId + " FAILED");
        }
    }
}