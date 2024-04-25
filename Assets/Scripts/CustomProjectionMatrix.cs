using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CustomProjectionMatrix : MonoBehaviour
{
    public Transform targetQuad;
    private Camera cam;
    private Matrix4x4 originalProjectionMatrix;

    private void Awake()
    {
        cam = GetComponent<Camera>();

    }

    void Update()
    {
        if (targetQuad == null) return;

        if (cam == null) return;

        // Calculate the corners of the quad in world space
        Vector3[] worldCorners = new Vector3[4];
        worldCorners[0] = targetQuad.TransformPoint(new Vector3(-0.5f, -0.5f, 0)); // Bottom left
        worldCorners[1] = targetQuad.TransformPoint(new Vector3(0.5f, -0.5f, 0));  // Bottom right
        worldCorners[2] = targetQuad.TransformPoint(new Vector3(-0.5f, 0.5f, 0));  // Top left
        worldCorners[3] = targetQuad.TransformPoint(new Vector3(0.5f, 0.5f, 0));   // Top right

        // Transform the quad's corners to the camera's view space
        Vector3[] viewSpaceCorners = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            viewSpaceCorners[i] = cam.worldToCameraMatrix.MultiplyPoint(worldCorners[i]);
        }

        // Calculate the parameters for the projection matrix based on the view space corners
        float left = viewSpaceCorners[0].x; // Assuming the bottom left corner is the most left
        float right = viewSpaceCorners[1].x; // Assuming the bottom right corner is the most right
        float bottom = viewSpaceCorners[0].y; // Assuming the bottom left corner is the lowest
        float top = viewSpaceCorners[2].y; // Assuming the top left corner is the highest
        float near = Vector3.Distance(cam.transform.position, targetQuad.transform.position);
        float far = cam.farClipPlane;

        // Construct the custom projection matrix
        Matrix4x4 projectionMatrix = Matrix4x4.Frustum(left, right, bottom, top, near, far);

        // Set the custom projection matrix
        cam.projectionMatrix = projectionMatrix;
    }

    //void OnDrawGizmos()
    //{
    //    if (targetQuad == null) return;

    //    Camera cam = GetComponent<Camera>();
    //    if (cam == null) return;

    //    // Draw original frustum
    //    DrawFrustum(cam, originalProjectionMatrix);

    //    // Set temporary projection matrix to draw modified frustum
    //    cam.projectionMatrix = originalProjectionMatrix;
    //    Matrix4x4 tempMatrix = cam.projectionMatrix;
    //    cam.ResetProjectionMatrix(); // Reset to make sure we are using the modified one for drawing
    //    DrawFrustum(cam, tempMatrix); // This draws the frustum with the custom projection matrix applied
    //}

    //void DrawFrustum(Camera cam, Matrix4x4 projectionMatrix)
    //{
    //    Matrix4x4 temp = cam.projectionMatrix;
    //    cam.projectionMatrix = projectionMatrix;
    //    Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
    //    if (cam.orthographic)
    //    {
    //        float spread = cam.farClipPlane - cam.nearClipPlane;
    //        float center = (cam.farClipPlane + cam.nearClipPlane) * 0.5f;
    //        Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2, spread));
    //    }
    //    else
    //    {
    //        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
    //    }
    //    cam.projectionMatrix = temp;
    //}
}

