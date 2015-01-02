using UnityEngine;

public class BoundingSphere : MonoBehaviour
{
    public Vector3 center;
    public float radius;

    private Cyclone.BoundingSphere boundingSphere;
    private Cyclone.Math.Vector3 bsPosition;

    private void Start()
    {
        Vector3 position = transform.position + center;
        bsPosition = new Cyclone.Math.Vector3(position.x, position.y, position.z);
        boundingSphere = new Cyclone.BoundingSphere(new Cyclone.Math.Vector3(bsPosition.x, bsPosition.y, bsPosition.z), radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 gizmo = new Vector3((float)boundingSphere.Center.x, (float)boundingSphere.Center.y, (float)boundingSphere.Center.z);
        Gizmos.DrawWireSphere(gizmo, radius);
    }

    private void Update()
    {
        
    }
}
