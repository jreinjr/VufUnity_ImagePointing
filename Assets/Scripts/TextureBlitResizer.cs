using UnityEngine;

public class TextureBlitResizer : MonoBehaviour
{


    public bool flipVertical = false;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {

        Vector2 scale = new Vector2(1, 1);
        Vector2 offset = new Vector2(0, 0);

        if (flipVertical)
        {
            scale.y = -1;
            offset.y = 1;
        }


        // Perform the blit with the computed scale and offset
        Graphics.Blit(src, dest, scale, offset);
    }
}
