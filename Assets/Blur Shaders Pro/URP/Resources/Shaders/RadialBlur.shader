Shader "BlurShadersProURP/RadialBlur"
{
    SubShader
    {
		Tags
		{
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
		}

		HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

		#define E 2.71828f

#if UNITY_VERSION < 600000
		float4 _BlitTexture_TexelSize;
#endif
		uint _KernelSize;
		float _Spread;
		float _StepSize;

		float gaussian(int x)
		{
			float sigmaSqu = (_Spread * _Spread);
			return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2 * sigmaSqu));
		}

		ENDHLSL

        Pass
        {
			HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag_horizontal

            float4 frag_horizontal (Varyings i) : SV_Target
			{
				float3 col = 0.0f;
				float kernelSum = 0.0f;

				float2 offset = i.texcoord - 0.5f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				for (int x = lower; x <= upper; ++x)
				{
					float2 uv = i.texcoord + offset * x * _StepSize;

					float gauss = gaussian(x);
					kernelSum += gauss;

					col += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).rgb;
				}

				col /= kernelSum;

				return float4(col, 1.0f);
			}
            ENDHLSL
        }
    }
}
