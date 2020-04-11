using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HalftoneRendererFeature : ScriptableRendererFeature
{

    public Material halftoneMaterial;

    private HalftoneRenderPass _halftoneRenderPass;
    public override void Create()
    {
        _halftoneRenderPass = new HalftoneRenderPass(halftoneMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_halftoneRenderPass);
    }


}
