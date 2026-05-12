namespace BlurShadersPro.URP
{
    using UnityEngine;
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;

#if UNITY_2022_2_OR_NEWER
    [CustomEditor(typeof(BlurSettings))]
#else
    [VolumeComponentEditor(typeof(BlurSettings))]
#endif
    public class BlurEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;
        SerializedDataParameter blurStepSize;
        SerializedDataParameter blurType;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<BlurSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
            blurStepSize = Unpack(o.Find(x => x.blurStepSize));
            blurType = Unpack(o.Find(x => x.blurType));
        }

        public override void OnInspectorGUI()
        {
            if (!BlurProUtility.CheckEffectEnabled<Blur>())
            {
                EditorGUILayout.HelpBox("The Blur effect must be added to your renderer's Renderer Features list.", MessageType.Error);
                if (GUILayout.Button("Add Blur Renderer Feature"))
                {
                    BlurProUtility.AddEffectToPipelineAsset<Blur>();
                }
            }

            PropertyField(strength);
            PropertyField(blurStepSize);
            PropertyField(blurType);
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Blur");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Blur";
    }
#endif
    }
}
