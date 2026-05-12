namespace BlurShadersPro.URP
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering.Universal;

    public static class BlurProUtility
    {
        // Get a reference to the forward renderer so we can check if an effect has been added.
        public static ScriptableRendererData GetForwardRenderer()
        {
            ScriptableRendererData[] rendererDataList =
                ((ScriptableRendererData[])typeof(UniversalRenderPipelineAsset)
                .GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(UniversalRenderPipeline.asset));
            int index = (int)typeof(UniversalRenderPipelineAsset)
                .GetField("m_DefaultRendererIndex", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(UniversalRenderPipeline.asset);

            return rendererDataList[index];
        }

        // Check the Forward Renderer and make sure the specified effect is attached.
        public static bool CheckEffectEnabled<T>() where T : ScriptableRendererFeature
        {
            if (UniversalRenderPipeline.asset == null)
            {
                return false;
            }

            ScriptableRendererData forwardRenderer =
                ((ScriptableRendererData[])typeof(UniversalRenderPipelineAsset)
                .GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(UniversalRenderPipeline.asset))[0];

            foreach (ScriptableRendererFeature item in forwardRenderer.rendererFeatures)
            {
                if (item?.GetType() == typeof(T))
                {
                    return true;
                }
            }

            return false;
        }

        // Add a missing RendererFeature to the Forward Renderer.
        public static void AddEffectToPipelineAsset<T>() where T : ScriptableRendererFeature
        {
#if UNITY_EDITOR
            if (UniversalRenderPipeline.asset == null)
            {
                Debug.LogError("No URP asset detected. Please make sure your project is using URP.");
                return;
            }

            var forwardRenderer = GetForwardRenderer();
            var effect = ScriptableRendererFeature.CreateInstance<T>();

            AssetDatabase.AddObjectToAsset(effect, forwardRenderer);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(effect, out var guid, out long localID);

            forwardRenderer.rendererFeatures.Add(effect);
            forwardRenderer.SetDirty();

            Debug.Log("Added " + typeof(T).ToString() + " effect to the active Forward Renderer (" + forwardRenderer.name + ").", forwardRenderer);
#endif
        }
    }
}
