using UnityEngine;
[RequireComponent(typeof(Camera))]
public class TextureBlitter : MonoBehaviour
{
    public RenderTexture source;
    public RenderTexture target;
    public Material _mat;
    private void OnPostRender()
    {
        // Ensure we have source and target defined
        if (source == null || target == null)
        {
            Debug.LogError("Please assign both source and target RenderTextures!");
            return;
        }

        // Check if source has been rendered into
        if (source.IsCreated())
        {
            // Downsample and blit from source to target
            Graphics.Blit(source, target, _mat);
        }
    }
}
