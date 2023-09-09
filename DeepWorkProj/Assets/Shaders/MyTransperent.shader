//SSsss
 Shader "MyTransparent" {
 Properties {
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 }
 
 SubShader {
     Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 200
     Cull Off
	//Blend SrcAlpha OneMinusSrcAlpha 
CGPROGRAM
 #pragma surface surf Lambert alpha
 
 sampler2D _MainTex;
 fixed4 _Color;
 float _Cutoff;
 
 struct Input {
     float2 uv_MainTex;
 };
 
 void surf (Input IN, inout SurfaceOutput o) {
     fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
     o.Albedo = c.rgb;
     o.Alpha = c.a;
  //   if (ca > _Cutoff)
 //      o.Alpha = c.a;
 //    else
 //      o.Alpha = 0;
 }
 ENDCG
 }
 
 //Fallback "Transparent/VertexLit"
 }