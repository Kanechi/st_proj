//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2020 //
/// Shader generate with Shadero 1.9.9                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Customs/GlayScale"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_SuperGrayScale_Fade_1("_SuperGrayScale_Fade_1", Range(0, 1)) = 1
_ShinyFX_Pos_1("_ShinyFX_Pos_1", Range(-1, 1)) = 0.124
_ShinyFX_Size_1("_ShinyFX_Size_1", Range(-1, 1)) = -0.118
_ShinyFX_Smooth_1("_ShinyFX_Smooth_1", Range(0, 1)) = 0.271
_ShinyFX_Intensity_1("_ShinyFX_Intensity_1", Range(0, 4)) = 1
_ShinyFX_Speed_1("_ShinyFX_Speed_1", Range(0, 8)) = 0
_SpriteFade("SpriteFade", Range(0, 1)) = 1.0

// required for UI.Mask
[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
[HideInInspector]_Stencil("Stencil ID", Float) = 0
[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
[HideInInspector]_ColorMask("Color Mask", Float) = 15

}

SubShader
{

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off 

// required for UI.Mask
Stencil
{
Ref [_Stencil]
Comp [_StencilComp]
Pass [_StencilOp]
ReadMask [_StencilReadMask]
WriteMask [_StencilWriteMask]
}

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};

sampler2D _MainTex;
float _SpriteFade;
float _SuperGrayScale_Fade_1;
float _ShinyFX_Pos_1;
float _ShinyFX_Size_1;
float _ShinyFX_Smooth_1;
float _ShinyFX_Intensity_1;
float _ShinyFX_Speed_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float DSFXr (float2 c, float seed)
{
return frac(43.*sin(c.x+7.*c.y)*seed);
}

float DSFXn (float2 p, float seed)
{
float2 i = floor(p), w = p-i, j = float2 (1.,0.);
w = w*w*(3.-w-w);
return lerp(lerp(DSFXr(i, seed), DSFXr(i+j, seed), w.x), lerp(DSFXr(i+j.yx, seed), DSFXr(i+1., seed), w.x), w.y);
}

float DSFXa (float2 p, float seed)
{
float m = 0., f = 2.;
for ( int i=0; i<9; i++ ){ m += DSFXn(f*p, seed)/f; f+=f; }
return m;
}

float4 DestroyerFX(float4 txt, float2 uv, float value, float seed, float HDR)
{
float t = frac(value*0.9999);
float4 c = smoothstep(t / 1.2, t + .1, DSFXa(3.5*uv, seed));
c = txt*c;
c.r = lerp(c.r, c.r*120.0*(1 - c.a), value);
c.g = lerp(c.g, c.g*40.0*(1 - c.a), value);
c.b = lerp(c.b, c.b*5.0*(1 - c.a) , value);
c.rgb = lerp(saturate(c.rgb),c.rgb,HDR);
return c;
}
float4 SuperGrayScale(float4 rgba, float4 red, float4 green, float4 blue, float fade)
{
float3 c_r = float3(red.r, red.g, red.b);
float3 c_g = float3(green.r, green.g, green.b);
float3 c_b = float3(blue.r, blue.g, blue.b);
float4 r = float4(dot(rgba.rgb, c_r) + red.a, dot(rgba.rgb, c_g) + green.a, dot(rgba.rgb, c_b) + blue.a, rgba.a);
return lerp(rgba, r, fade);

}

float4 AlphaIntensity(float4 txt,float fade)
{
if (txt.a < 1) txt.a = txt.a*fade;
return txt;
}

float4 ShinyFX(float4 txt, float2 uv, float pos, float size, float smooth, float intensity, float speed)
{
pos = pos + 0.5+sin(_Time*20*speed)*0.5;
uv = uv - float2(pos, 0.5);
float a = atan2(uv.x, uv.y) + 1.4, r = 3.1415;
float d = cos(floor(0.5 + a / r) * r - a) * length(uv);
float dist = 1.0 - smoothstep(size, size + smooth, d);
txt.rgb += dist*intensity;
return txt;
}
float4 frag (v2f i) : COLOR
{
float4 _MainTex_1 = tex2D(_MainTex, i.texcoord);
float4 _SuperGrayScale_1 = SuperGrayScale(_MainTex_1,float4(0,0,1,0),float4(0,0,1,0),float4(0,0,1,0),_SuperGrayScale_Fade_1);
float4 _ShinyFX_1 = ShinyFX(_SuperGrayScale_1,i.texcoord,_ShinyFX_Pos_1,_ShinyFX_Size_1,_ShinyFX_Smooth_1,_ShinyFX_Intensity_1,_ShinyFX_Speed_1);
float4 FinalResult = _ShinyFX_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}
