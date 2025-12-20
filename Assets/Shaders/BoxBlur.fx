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

float DirectionPattern[] = { 1, 1, 1, 0, -1, 1};

float4 PixelShaderFunction(float4 SampleColor: COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 pixel = 1 / uScreenResolution * uIntensity * 20;
    float3 blur = tex2D(uImage0, coords);
    float3 colorAround = blur.rgb;
    
    for (int i = 0; i < 4; i++)
    {
        colorAround += tex2D(uImage0, coords + float2(pixel.x * DirectionPattern[i], pixel.y * DirectionPattern[i + 2])).rgb;
        colorAround += tex2D(uImage0, coords - float2(pixel.x * DirectionPattern[i], pixel.y * DirectionPattern[i + 2])).rgb;
    }
    
    colorAround /= 9;
    
    blur = colorAround + (uColor.x * uIntensity);
    
    return float4(blur, 1);
}

technique Technique1
{
    pass BlurPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}