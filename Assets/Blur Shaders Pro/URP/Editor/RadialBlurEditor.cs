namespace BlurShadersPro.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

#if UNITY_2022_2_OR_NEWER
    [CustomEditor(typeof(RadialBlurSettings))]
#else
    [VolumeComponentEditor(typeof(RadialBlurSettings))]
#endif
    public class RadialBlurEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;
        SerializedDataParameter stepSize; 

        public override void OnEnable()
        {
            var o = new PropertyFetcher<RadialBlurSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
            stepSize = Unpack(o.Find(x => x.stepSize));
        }

        public override void OnInspectorGUI()
        {
            if (!BlurProUtility.CheckEffectEnabled<RadialBlur>())
            {
                EditorGUILayout.HelpBox("The Radial Blur effect must be added to your renderer's Renderer Features list.", MessageType.Error);
                if (GUILayout.Button("Add Radial Blur Renderer Feature"))
                {
                    BlurProUtility.AddEffectToPipelineAsset<RadialBlur>();
                }
            }

            PropertyField(strength);
            PropertyField(stepSize);
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Radial Blur");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Radial Blur";
    }
#endif
    }
}
