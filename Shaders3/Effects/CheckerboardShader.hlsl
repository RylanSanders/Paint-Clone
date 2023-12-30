sampler2D input : register(s0);
float pixDiv : register(c0);
float4 color1 : register(c1);
float4 color2 : register(c2);

float4 main(float2 uv:TEXCOORD) : COLOR
{
    if (tex2D(input, uv).a == 0)
    {
        return tex2D(input, uv);
    }
    float xPos = fmod(trunc(uv.x * 100 / pixDiv), 2);
    float yPos = fmod(trunc(uv.y * 100 / pixDiv), 2);
    float total = trunc(fmod(xPos + yPos, 2));
    if (total == 0)
    {
        return color1;
    }
    else
    {
        return color2;

    }
        
}