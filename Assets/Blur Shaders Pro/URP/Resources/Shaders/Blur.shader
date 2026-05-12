Shader "BlurShadersProURP/Blur"
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
		uint _BlurStepSize;

		float gaussian(int x) 
		{
			float sigmaSqu = _Spread * _Spread;
			return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2 * sigmaSqu));
		}

		ENDHLSL

        Pass
        {
			Name "HorizontalGaussian"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag_horizontal

            float4 frag_horizontal (Varyings i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int x = lower; x <= upper; x += _BlurStepSize)
				{
					float gauss = gaussian(x);
					kernelSum += gauss;
					uv = i.texcoord + float2(_BlitTexture_TexelSize.x * x, 0.0f);
					col += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).xyz;
				}

				col /= kernelSum;

				return float4(col, 1.0f);
			}
            ENDHLSL
        }

		Pass
        {
			Name "VerticalGaussian"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag_vertical

            float4 frag_vertical (Varyings i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int y = lower; y <= upper; y += _BlurStepSize)
				{
					float gauss = gaussian(y);
					kernelSum += gauss;
					uv = i.texcoord + float2(0.0f, _BlitTexture_TexelSize.y * y);
					col += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).xyz;
				}

				col /= kernelSum;
				return float4(col, 1.0f);
			}
            ENDHLSL
        }

		Pass
        {
			Name "HorizontalBox"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag_horizontal

            float4 frag_horizontal (Varyings i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int x = lower; x <= upper; ++x)
				{
					kernelSum++;
					uv = i.texcoord + float2(_BlitTexture_TexelSize.x * x, 0.0f);
					col += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).xyz;
				}

				col /= kernelSum;

				return float4(col, 1.0f);
			}
            ENDHLSL
        }

		Pass
        {
			Name "VerticalBox"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag_vertical

            float4 frag_vertical (Varyings i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int y = lower; y <= upper; ++y)
				{
					kernelSum++;
					uv = i.texcoord + float2(0.0f, _BlitTexture_TexelSize.y * y);
					col += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).xyz;
				}

				col /= kernelSum;
				return float4(col, 1.0f);
			}
            ENDHLSL
        }
    }
}
