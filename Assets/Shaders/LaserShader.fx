sampler uImage0 : register(s0);
sampler uImage1 : register(s1); // Automatically Images/Misc/Perlin via Force Shader testing option
sampler uImage2 : register(s2); // Automatically Images/Misc/noise via Force Shader testing option
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
//    uSourceRect = (0.1, 0.1, 0.1, 0.1);
    float4 color = tex2D(uImage0, coords);
    float4 FXcolor = tex2D(uImage1, float2(coords.x, coords.y));
      
    if(coords.x > 0.5)
    {
        FXcolor = tex2D(uImage1, float2(coords.x, coords.y * 0.01f));
        
        float Lum = (FXcolor.r + FXcolor.g + FXcolor.b) / 3;
        
        FXcolor.rgb = lerp(float3(37 / 255, 70 / 255, 1), float3(243 / 255, 1, 1), Lum * 0.75); 
        
        FXcolor *= 2;
        
        color = lerp(color, FXcolor, uOpacity);
    }
    

    
    return color;
}

technique Technique1
{
    pass LaserPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}