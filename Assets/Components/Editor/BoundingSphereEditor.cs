using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoundingSphere))]
public class BoundingSphereEditor : Editor
{
    /// <summary>
    /// The target object to apply the values to.
    /// </summary>
    private BoundingSphere targetObject;

    /// <summary>
    /// Get a reference to the target object.
    /// </summary>
    void OnEnable()
    {
        targetObject = (BoundingSphere)target;
    }

    /// <summary>
    /// Layout the custom inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        targetObject.center = EditorGUILayout.Vector3Field("Center", targetObject.center);
        targetObject.radius = EditorGUILayout.FloatField("Radius", targetObject.radius);
        GUILayout.EndVertical();

        // If GUI changed, apply the values to the script.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetObject);
        }
    }
}
