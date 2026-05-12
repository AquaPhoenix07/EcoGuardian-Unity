using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_6000_0_OR_NEWER
    using UnityEngine.Rendering.RenderGraphModule;
#endif

namespace BlurShadersPro.URP
{
    public class Blur : ScriptableRendererFeature
    {
        BlurRenderPass pass;

        public override void Create()
        {
            pass = new BlurRenderPass();
            name = "Blur";
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var settings = VolumeManager.instance.stack.GetComponent<BlurSettings>();

            if (settings != null && settings.IsActive())
            {
                renderer.EnqueuePass(pass);
            }
        }

        protected override void Dispose(bool disposing)
        {
            pass.Dispose();
            base.Dispose(disposing);
        }

        class BlurRenderPass : ScriptableRenderPass
        {
            private Material material;
            private RTHandle blurTexHandle;

            public BlurRenderPass()
            {
                profilingSampler = new ProfilingSampler("Blur");
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

#if UNITY_6000_0_OR_NEWER
                requiresIntermediateTexture = true;
#endif
            }

            private void CreateMaterial()
            {
                var shader = Shader.Find("BlurShadersProURP/Blur");

                if (shader == null)
                {
                    Debug.LogError("Cannot find shader: \"BlurShadersProURP/Blur\".");
                    return;
                }

                material = new Material(shader);
            }

            private static RenderTextureDescriptor GetCopyPassDescriptor(RenderTextureDescriptor descriptor)
            {
                descriptor.msaaSamples = 1;
                descriptor.depthBufferBits = (int)DepthBits.None;

                return descriptor;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                ResetTarget();

                var descriptor = GetCopyPassDescriptor(cameraTextureDescriptor);
                RenderingUtils.ReAllocateIfNeeded(ref blurTexHandle, descriptor);

                base.Configure(cmd, cameraTextureDescriptor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (renderingData.cameraData.isPreviewCamera)
                {
                    return;
                }

                if (material == null)
                {
                    CreateMaterial();
                }

                CommandBuffer cmd = CommandBufferPool.Get();

                // Set Blur effect properties.
                var settings = VolumeManager.instance.stack.GetComponent<BlurSettings>();
                material.SetInt("_KernelSize", settings.strength.value);
                material.SetFloat("_Spread", settings.strength.value / 7.5f);
                material.SetInt("_BlurStepSize", settings.blurStepSize.value);

                RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

                if (settings.strength.value > settings.blurStepSize.value * 2)
                {
                    // Perform the Blit operations for the Blur effect.
                    using (new ProfilingScope(cmd, profilingSampler))
                    {
                        if (settings.blurType.value == BlurType.Gaussian)
                        {
                            Blit(cmd, cameraTargetHandle, blurTexHandle, material, 0);
                            Blit(cmd, blurTexHandle, cameraTargetHandle, material, 1);
                        }
                        else if (settings.blurType.value == BlurType.Box)
                        {
                            Blit(cmd, cameraTargetHandle, blurTexHandle, material, 2);
                            Blit(cmd, blurTexHandle, cameraTargetHandle, material, 3);
                        }
                    }
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            public void Dispose()
            {
                blurTexHandle?.Release();
            }

#if UNITY_6000_0_OR_NEWER

            private class CopyPassData
            {
                public Material material;
                public TextureHandle inputTexture;
            }

            private class MainPassData
            {
                public Material material;
                public TextureHandle inputTexture;
            }

            private static void ExecuteCopyPass(RasterCommandBuffer cmd, RTHandle source, Material material)
            {
                var settings = VolumeManager.instance.stack.GetComponent<BlurSettings>();

                if (settings.strength.value > settings.blurStepSize.value * 2)
                {
                    if (settings.blurType.value == BlurType.Gaussian)
                    {
                        Blitter.BlitTexture(cmd, source, new Vector4(1, 1, 0, 0), material, 0);
                    }
                    else if (settings.blurType.value == BlurType.Box)
                    {
                        Blitter.BlitTexture(cmd, source, new Vector4(1, 1, 0, 0), material, 2);
                    }
                }
            }

            private static void ExecuteMainPass(RasterCommandBuffer cmd, RTHandle source, Material material)
            {
                var settings = VolumeManager.instance.stack.GetComponent<BlurSettings>();

                if (settings.strength.value > settings.blurStepSize.value * 2)
                {
                    // Set Blur effect properties.
                    material.SetInt("_KernelSize", settings.strength.value);
                    material.SetFloat("_Spread", settings.strength.value / 7.5f);
                    material.SetInt("_BlurStepSize", settings.blurStepSize.value);

                    if(settings.blurType.value == BlurType.Gaussian)
                    {
                        Blitter.BlitTexture(cmd, source, new Vector4(1, 1, 0, 0), material, 1);
                    }
                    else if (settings.blurType.value == BlurType.Box)
                    {
                        Blitter.BlitTexture(cmd, source, new Vector4(1, 1, 0, 0), material, 3);
                    }
                }
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                if(material == null)
                {
                    CreateMaterial();
                }

                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                UniversalRenderer renderer = (UniversalRenderer)cameraData.renderer;
                var colorCopyDescriptor = GetCopyPassDescriptor(cameraData.cameraTargetDescriptor);
                TextureHandle copiedColor = TextureHandle.nullHandle;

                // Perform the intermediate copy pass (source -> temp).
                copiedColor = UniversalRenderer.CreateRenderGraphTexture(renderGraph, colorCopyDescriptor, "_BlurColorCopy", false);

                using (var builder = renderGraph.AddRasterRenderPass<CopyPassData>("Blur_CopyColor", out var passData, profilingSampler))
                {
                    passData.material = material;
                    passData.inputTexture = resourceData.activeColorTexture;

                    builder.UseTexture(resourceData.activeColorTexture, AccessFlags.Read);
                    builder.SetRenderAttachment(copiedColor, 0, AccessFlags.Write);
                    builder.SetRenderFunc((CopyPassData data, RasterGraphContext context) => ExecuteCopyPass(context.cmd, data.inputTexture, data.material));
                }

                // Perform main pass (temp -> source).
                using (var builder = renderGraph.AddRasterRenderPass<MainPassData>("Blur_MainPass", out var passData, profilingSampler))
                {
                    passData.material = material;
                    passData.inputTexture = copiedColor;

                    builder.UseTexture(copiedColor, AccessFlags.Read);
                    builder.SetRenderAttachment(resourceData.activeColorTexture, 0, AccessFlags.Write);
                    builder.SetRenderFunc((MainPassData data, RasterGraphContext context) => ExecuteMainPass(context.cmd, data.inputTexture, data.material));
                }
            }

#endif
        }
    }
}
