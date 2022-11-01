using UnityEngine;

public class GizmosSphere : MonoBehaviour {
#if UNITY_EDITOR
    public float radius = 1;
    
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
