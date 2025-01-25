using UnityEngine;

public class MoveableBase : MonoBehaviour
{
    public Vector3 targetPosition;

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }
}