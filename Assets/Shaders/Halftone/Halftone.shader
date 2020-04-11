Shader "Hidden/Halftone"
{
    Properties
    {
        _MainTex ("Background", 2D) = "white" {}
        _SceneTex ("Scene Texture", 2D) = "_CameraColorTexture" {}
        _CircleTex ("Circle Texture", 2D) = "_CameraColorTexture" {}
        _gridSizeX ("Grid Size X", Range(0,0.1)) = 0.01
        _gridSizeY ("Grid Size Y", Range(0,0.1)) = 0.01
        _dotSize ("Dot Size", Range(0,0.1)) = 0.01
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _gridSizeX;
            float _gridSizeY;

            float _dotSize;
            sampler2D _MainTex;
            sampler2D _CircleTex;
            sampler2D _SceneTex;

            float getDistSquared(float2 a, float2 b)
            {
                const float diffVec = a - b;
                return dot(diffVec, diffVec);
            }

            float2 GetClosesUVGridCenter(float2 uv, float2 offset)
            {
                const float2 gridSize = float2(_gridSizeX, _gridSizeY);
                const float2 invGridSize = 1 / gridSize;

                // round to closest point
                float2 closestGridPoint = floor((uv * invGridSize + 0.5)+offset);
                return closestGridPoint * gridSize;
            }

            float getLuma(float3 color)
            {
                const float3 lumaConversion = float3(0.2126, 0.7152, 0.0722);
                return dot(lumaConversion, color);
            }
            

            struct POINT_SAMPLE {
                float luma;
                float2 uv;
                fixed4 col;
                float pointSize;
                bool fragmentIsInside;
            };

            struct SURROUNDING_POINTS {
                POINT_SAMPLE taps[9];
            };

            bool UvIsInSquare(float2 uv, float2 center, float radius)
            {
                float2 distVec = abs(uv-center);
                return distVec < float2(radius,radius);
            }

            void SamplePoinAtUV(float2 uv, out POINT_SAMPLE outSample, float2 fragUV)
            {
                outSample.col = tex2D(_SceneTex, uv);
                outSample.luma = getLuma(outSample.col);
                outSample.uv = uv;
                outSample.pointSize = _dotSize * outSample.luma;
                outSample.fragmentIsInside = UvIsInSquare(fragUV,uv,outSample.pointSize);
            };

            SURROUNDING_POINTS GetSurroundingPoints(float2 center, float2 fragUV)
            {
                SURROUNDING_POINTS sampledPoints;
                for (int i = -1; i <= 1; ++i)
                {
                    float sampleX = _gridSizeX/2 * i;
                    for (int j = -1; j <= 1; ++j)
                    {
                        float sampleY = _gridSizeY/2 * j;
                        float2 uvSampleOffset = center + float2(sampleX, sampleY);
                        int tapIndex = (i + 1) * 3 + j + 1;

                        SamplePoinAtUV(uvSampleOffset, sampledPoints.taps[tapIndex], fragUV);
                    }
                }
                return sampledPoints;
            };

            float GetAlphaForPoint(in POINT_SAMPLE sampleToUse, in float2 fragUV)
            {
                float2 localUV = ((sampleToUse.uv - fragUV)/sampleToUse.pointSize)+float2(0.5,0.5);
                const fixed4 alphaColor = tex2D(_CircleTex, localUV);
                const float alpha = sampleToUse.fragmentIsInside?alphaColor.r:0;
                return sampleToUse.fragmentIsInside?alpha:0;
            }

            fixed4 frag(v2f frag) : SV_Target
            {
                float2 closestUVcenter = GetClosesUVGridCenter(frag.uv,float2(0,0));
                SURROUNDING_POINTS surrounding = GetSurroundingPoints(closestUVcenter, frag.uv);
                
                POINT_SAMPLE sampleToUse = surrounding.taps[5];
                for (int i = 0; i < 9; ++i)
                {
                    if (surrounding.taps[i].fragmentIsInside && (sampleToUse.pointSize > surrounding.taps[i].pointSize))
                    {
                        sampleToUse = surrounding.taps[i];
                    }
                }

                const fixed4 background = tex2D(_MainTex, frag.uv);
                float alpha = GetAlphaForPoint(sampleToUse, frag.uv);
                return lerp(background, sampleToUse.col, alpha);
            }
            ENDCG
        }
    }
}
