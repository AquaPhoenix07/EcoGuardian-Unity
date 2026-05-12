namespace BlurShadersPro.URP
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable, VolumeComponentMenu("Blur Shaders Pro/Blur")]
    public sealed class BlurSettings : VolumeComponent, IPostProcessComponent
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

        [Tooltip("Blur Strength")]
        public ClampedIntParameter strength = new ClampedIntParameter(1, 1, 500);

        [Tooltip("Higher values will skip pixels during blur passes. Increase for better performance.")]
        public ClampedIntParameter blurStepSize = new ClampedIntParameter(1, 1, 16);

        [Tooltip("Type of blur. Gaussian blur is slightly more expensive, but higher fidelity.")]
        public BlurTypeParameter blurType = new BlurTypeParameter(BlurType.Gaussian);

        public bool IsActive()
        {
            return strength.value > 1 && active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }

    [Serializable]
    public enum BlurType
    {
        Gaussian, Box
    }

    [Serializable]
    public sealed class BlurTypeParameter : VolumeParameter<BlurType>
    {
        public BlurTypeParameter(BlurType value, bool overrideState = false) : base(value, overrideState) { }
    }
}
