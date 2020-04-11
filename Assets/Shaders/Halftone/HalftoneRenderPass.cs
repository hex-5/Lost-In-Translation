using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;

public class HalftoneRenderPass : ScriptableRenderPass
{
    private string m_ProfilerTag = "Halftone Pass";
    private string _sceneTextureParameterName = "_SceneTex";
    public Material m_HalftoneMaterial;

    public HalftoneRenderPass(Material halftoneMaterial)
    {
        this.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        m_HalftoneMaterial = halftoneMaterial;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.all);

        ref CameraData cameraData = ref renderingData.cameraData;

        int sceneTextureNameId = -1;
        if ((sceneTextureNameId = Shader.PropertyToID(_sceneTextureParameterName)) == -1)
        {
            Debug.LogError($"Could not find shader param '{_sceneTextureParameterName}'");
        }

        //acquire RT to render to
        int tempRTNameId = Shader.PropertyToID("_TempTarget2");
        RenderTargetIdentifier tempRtIdentifier = new RenderTargetIdentifier(tempRTNameId);
        cmd.GetTemporaryRT(tempRTNameId, cameraData.cameraTargetDescriptor);

        // get camera color texture
        //cameraTargetHandle.Init("_AfterPostProcessTexture");
        RenderTargetIdentifier cameraTargetIdentifier  = cameraData.postProcessEnabled? new RenderTargetIdentifier("_AfterPostProcessColor") : new RenderTargetIdentifier("_CameraColorTexture");
        
        // why is this not necessary?!
        //cmd.CopyTexture(BuiltinRenderTextureType.CurrentActive, tempRtIdentifier);

        cmd.SetGlobalTexture(sceneTextureNameId, tempRtIdentifier);

        DrawHalftoneBlit(cameraData.camera, cmd);

        context.ExecuteCommandBuffer(cmd);
        cmd.ReleaseTemporaryRT(tempRTNameId);
        CommandBufferPool.Release(cmd);
    }

    private void DrawHalftoneBlit(Camera camera, CommandBuffer cmd)
    {
        cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
        cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_HalftoneMaterial);
        cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
    }
}
