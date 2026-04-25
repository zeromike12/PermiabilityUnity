Shader "Custom/RadialTransparentDiffusion"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0, 1, 0.5)
        _TargetColor ("Target Color", Color) = (1, 0, 0, 0.5)
        _DiffusionAmount ("Diffusion Radius", Range(0, 1.2)) = 0.0
        _NoiseScale ("Noise Scale", Float) = 15.0
        _Speed ("Swirl Speed", Float) = 1.0
        _EdgeSoftness ("Edge Softness", Range(0.01, 0.5)) = 0.2
    }
    SubShader
    {
        // 1. Set up the transparent queue and blending
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _BaseColor;
            float4 _TargetColor;
            float _DiffusionAmount;
            float _NoiseScale;
            float _Speed;
            float _EdgeSoftness;

            // Simple pseudo-random procedural noise
            float2 hash(float2 p) 
            {
                p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
            }

            float noise(in float2 p)
            {
                const float K1 = 0.366025404;
                const float K2 = 0.211324865;
                float2 i = floor(p + (p.x + p.y) * K1);
                float2 a = p - i + (i.x + i.y) * K2;
                float2 o = (a.x > a.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
                float2 b = a - o + K2;
                float2 c = a - 1.0 + 2.0 * K2;
                float3 h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
                float3 n = h * h * h * h * float3(dot(a, hash(i + 0.0)), dot(b, hash(i + o)), dot(c, hash(i + 1.0)));
                return dot(n, float3(70.0, 70.0, 70.0));
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 2. Calculate the distance from the center (0.5, 0.5) of the UV map
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // 3. Generate noise for the liquid edge
                float2 animatedUV = i.uv * _NoiseScale + _Time.y * _Speed;
                float n = noise(animatedUV); 

                // 4. Distort the distance by the noise to make the expanding circle wavy and irregular
                float distortedDist = dist + (n * 0.2);

                // 5. Create a soft mask based on the Diffusion Radius
                // If the distorted distance is smaller than the _DiffusionAmount, the mask approaches 1 (Target Color)
                float mask = smoothstep(_DiffusionAmount + _EdgeSoftness, _DiffusionAmount - _EdgeSoftness, distortedDist);

                // 6. Blend both the RGB color and the Alpha channel
                float4 finalColor = lerp(_BaseColor, _TargetColor, mask);
                
                return finalColor;
            }
            ENDCG
        }
    }
}