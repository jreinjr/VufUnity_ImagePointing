using UnityEngine;

// This script visualizes the GameObject's local axes in the Scene view.
public class TransformVisualizer : MonoBehaviour
{
    // The length of the axis lines.
    public float axisLength = 1.0f;

    // Called by Unity to draw gizmos in the editor.
    void OnDrawGizmos()
    {
        // Draw the local X axis in red.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * axisLength);

        // Draw the local Y axis in green.
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * axisLength);

        // Draw the local Z axis in blue.
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * axisLength);
    }
}
