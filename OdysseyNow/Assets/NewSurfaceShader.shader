Shader "Custom/Screen" {
    Properties{
        _MainTex("贴图", 2D) = "white" {}
        _TimeScale("花屏速度" , Range(0.0001, 2.0)) = 1.0
    }
        SubShader{
        Tags{ "RenderType" = "Opaque" }
        LOD 300
 
        pass {
        Cull Back
            
            CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
 
        #include "UnityCG.cginc"
        #define R frac(43.*sin(p.x*73.+p.y*8.))
 
 
        float _TimeScale;
        float sat(float t) {
            return clamp(t, 0.0, 1.0);
        }
 
        float2 sat(float2 t) {
            return clamp(t, 0.0, 1.0);
        }
 
        float remap(float t, float a, float b) {
            return sat((t - a) / (b - a));
        }
 
        float linterp(float t) {
            return sat(1.0 - abs(2.0*t - 1.0));
        }
 
        float3 spectrum_offset(float t) {
            float3 ret;
            float lo = step(t,0.5);
            float hi = 1.0 - lo;
            float w = linterp(remap(t, 1.0 / 6.0, 5.0 / 6.0));
            float neg_w = 1.0 - w;
            ret = float3(lo,1.0,hi) * float3(neg_w, w, neg_w);
            return pow(ret, float3(1.0 / 2.2, 1.0 / 2.2, 1.0 / 2.2));
        }
 
        float rand(float2 n) {
            return (sin(dot(float2(n.x *_TimeScale, n.y *_TimeScale), float2(12.9898, 78.233))));
        }
 
        float srand(float2 n) {
            return rand(n) * 2.0 - 1.0;
        }
 
        float mytrunc(float x, float num_levels)
        {
            return floor(x*num_levels) / num_levels;
        }
        float2 mytrunc(float2 x, float num_levels)
        {
            return floor(x*num_levels) / num_levels;
        }
            sampler2D _MainTex;
        float4 _MainTex_ST;
        float uvOffset;
 
        struct a2v {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
        };
 
        struct v2f {
            float4 pos : POSITION;
            float2 uv : TEXCOORD0;
            float3 color : TEXCOORD1;
        };
 
        v2f vert(a2v v) {
            v2f o; o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            o.color = ShadeVertexLights(v.vertex, v.normal);
            return o;
        }
 
        float4 frag(v2f i) : COLOR{
        float4 c;
 
 
        float aspect = _ScreenParams.x / _ScreenParams.y;
        aspect = 1;
        float2 uv = _ScreenParams.xy / _ScreenParams.xy;
        uv = float2(1,1);
 
        float time = fmod(_Time.y, 32.0); 
        float GLITCH = 10.1;
 
        float gnm = sat(GLITCH);
        float rnd0 = rand(mytrunc(float2(time, time), 6.0));
        float r0 = sat((1.0 - gnm)*0.7 + rnd0);
        float rnd1 = rand(float2(mytrunc(i.uv.x, 10.0*r0), time));
        float r1 = 0.5 - 0.5 * gnm + rnd1;
        r1 = 1.0 - max(0.0, ((r1<1.0) ? r1 : 0.9999999));
        float rnd2 = rand(float2(mytrunc(i.uv.y, 40.0*r1), time)); 
        float r2 = sat(rnd2);
 
        float rnd3 = rand(float2(mytrunc(i.uv.y, 10.0*r0), time));
        float r3 = (1.0 - sat(rnd3 + 0.8)) - 0.1;
 
        float pxrnd = rand(i.uv + time);
 
        float ofs = 0.05 * r2 * GLITCH * (rnd0 > 0.5 ? 1.0 : -1.0);
        ofs += 0.5 * pxrnd * ofs;
 
        i.uv.y += 0.1 * r3 * GLITCH;
 
        const int NUM_SAMPLES = 10;
        const float RCP_NUM_SAMPLES_F = 1.0 / float(NUM_SAMPLES);
 
        float4 sum = float4(0.0, 0.0, 0.0, 0.0);
        float3 wsum = float3(0.0, 0.0, 0.0);
        for (int j = 0; j<NUM_SAMPLES; ++j)
        {
            float t = float(j) * RCP_NUM_SAMPLES_F;
            i.uv.x = sat(i.uv.x + ofs * t);
            
            float4 samplecol = tex2D(_MainTex, i.uv); 
            float3 s = spectrum_offset(t);
 
            samplecol.rgb = samplecol.rgb * s;
            sum += samplecol;
            wsum += s;
        }
        sum.rgb /= wsum;
        sum.a *= RCP_NUM_SAMPLES_F;
        c.a = sum.a;
        c.rgb = sum.rgb;
        c.rgb = c.rgb;
 
        return c;
        }
            ENDCG
    }
    }FallBack "Diffuse"
}
