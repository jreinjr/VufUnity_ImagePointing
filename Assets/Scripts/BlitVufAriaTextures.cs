using UnityEngine;

public class BlitVufAriaTextures : MonoBehaviour
{
    public RenderTexture texA; // Source texture
    public RenderTexture texB; // Destination texture 
    public Shader blitWithTransform;

    // Enum for fit options
    public enum FitMode
    {
        FitToWidth,
        FitToHeight,
        StretchToFit,
        CropToFit
    }
    public FitMode fitMode = FitMode.StretchToFit;

    // Enum for rotation options
    public enum Rotation
    {
        None,
        CW90,
        CCW90,
        CW180
    }
    public Rotation rotation = Rotation.None;

    // Enum for flip options
    public enum FlipMode
    {
        None,
        Horizontal,
        Vertical,
        Both // Horizontal + Vertical
    }
    public FlipMode flipMode = FlipMode.None;

    private Material blitMaterial; // Shader material for transformations

    void Start()
    {
        // Create a material with a simple shader that handles transformations
        blitMaterial = new Material(blitWithTransform);
    }

    void FixedUpdate()
    {
        if (texA == null || texB == null || blitMaterial == null)
            return;

        // Set material parameters for rotation and flip
        SetBlitMaterialParameters();

        // Choose the appropriate scale and offset based on fit mode
        Vector2 scale = Vector2.one;
        Vector2 offset = Vector2.zero;

        switch (fitMode)
        {
            case FitMode.FitToWidth:
                scale.y = (float)texB.width / texA.width * (float)texA.height / texB.height;
                break;
            case FitMode.FitToHeight:
                scale.x = (float)texB.height / texA.height * (float)texA.width / texB.width;
                break;
            case FitMode.StretchToFit:
                // Scale remains (1, 1)
                break;
            case FitMode.CropToFit:
                if ((float)texB.width / texA.width < (float)texB.height / texA.height)
                {
                    scale.y = (float)texA.width / texB.width * (float)texB.height / texA.height;
                    offset.y = (1 - scale.y) / 2;
                }
                else
                {
                    scale.x = (float)texA.height / texB.height * (float)texB.width / texA.width;
                    offset.x = (1 - scale.x) / 2;
                }
                break;
        }

        blitMaterial.SetVector("_Scale", scale);
        blitMaterial.SetVector("_Offset", offset);

        // Blit with the given material (which handles transformations)
        Graphics.Blit(texA, texB, blitMaterial);
    }

    void SetBlitMaterialParameters()
    {
        // Set rotation
        switch (rotation)
        {
            case Rotation.CW90:
                blitMaterial.SetInt("_Rotation", 1);
                break;
            case Rotation.CCW90:
                blitMaterial.SetInt("_Rotation", -1);
                break;
            case Rotation.CW180:
                blitMaterial.SetInt("_Rotation", 2);
                break;
            default:
                blitMaterial.SetInt("_Rotation", 0);
                break;
        }

        // Set flip
        bool flipH = (flipMode == FlipMode.Horizontal || flipMode == FlipMode.Both);
        bool flipV = (flipMode == FlipMode.Vertical || flipMode == FlipMode.Both);
        blitMaterial.SetInt("_FlipH", flipH ? 1 : 0);
        blitMaterial.SetInt("_FlipV", flipV ? 1 : 0);
    }
}
