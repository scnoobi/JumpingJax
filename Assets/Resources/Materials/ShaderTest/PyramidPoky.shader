Shader "Custom/PyramidPoky"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ExtrusionFactor("Extrusion factor", float) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            Cull Off
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma geometry geom
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct v2g
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct g2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _ExtrusionFactor;

                v2g vert(appdata v)
                {
                    v2g o;

                    o.vertex = v.vertex;
                    o.uv = v.uv;
                    o.normal = v.normal;
                    return o;
                }

                float rand(float3 vec) {
                    float random = dot(vec, float3(12.9898, 78.233, 37.719));
                    random = frac(random * 143758.5453);
                    return random;
                }

                [maxvertexcount(12)]
                void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
                {
                    g2f o;

                    float4 barycenter = (IN[0].vertex + IN[1].vertex + IN[2].vertex) / 3;

                    float rando = rand(IN[0].vertex * _SinTime % 1.0);

                    float3 normal = (IN[0].normal + IN[1].normal + IN[2].normal) / 3;

                    for (int i = 0; i < 3; i++) {
                        int next = (i + 1) % 3;

                        o.vertex = UnityObjectToClipPos(IN[i].vertex);
                        UNITY_TRANSFER_FOG(o,o.vertex);
                        o.uv = TRANSFORM_TEX(IN[i].uv, _MainTex);
                        o.color = fixed4(0.0, 0.0, 0.0, 1.0);
                        triStream.Append(o);

                        o.vertex = UnityObjectToClipPos(barycenter + float4(normal, 0.0) * _ExtrusionFactor * (rando + 0.1f));
                        UNITY_TRANSFER_FOG(o,o.vertex);
                        o.uv = TRANSFORM_TEX(IN[i].uv, _MainTex);
                        o.color = fixed4(0.0, 0.0, 1.0, 1.0);
                        triStream.Append(o);

                        o.vertex = UnityObjectToClipPos(IN[next].vertex);
                        UNITY_TRANSFER_FOG(o,o.vertex);
                        o.uv = TRANSFORM_TEX(IN[next].uv, _MainTex);
                        o.color = fixed4(0.0, 0.0, 0.0, 1.0);
                        triStream.Append(o);

                        triStream.RestartStrip();
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        o.vertex = UnityObjectToClipPos(IN[i].vertex);
                        UNITY_TRANSFER_FOG(o,o.vertex);
                        o.uv = TRANSFORM_TEX(IN[i].uv, _MainTex);
                        o.color = fixed4(0.0, 0.0, 0.0, 1.0);
                        triStream.Append(o);
                    }

                    triStream.RestartStrip();
                }

                fixed4 frag(g2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }

            // Shadow Caster Pass
            Pass
            {
                Tags {"LightMode" = "ShadowCaster"}

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_shadowcaster
                #include "UnityCG.cginc"

                struct v2f {
                    V2F_SHADOW_CASTER;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    SHADOW_CASTER_FRAGMENT(i)
                }
                ENDCG
            }
        }
}