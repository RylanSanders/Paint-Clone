sampler2D input : register(s0);
float pixDiv : register(c0);
float4 color1 : register(c1);
float4 color2 : register(c2);

float4 main(float2 uv:TEXCOORD) : COLOR
{
    float4 currentColor = tex2D(input, uv);
    if (currentColor.a == 0)
    {
        return currentColor;
    }
    float xPos = fmod(trunc(uv.x / pixDiv), 2);
    float yPos = fmod(trunc(uv.y / pixDiv), 2);
    float total = xPos + yPos;
    //Adding xPos and yPos gets us either 0,1, or 2 
    if (total == 0 || total==2)
    {
        return color1;
    }
    else
    {
        return color2;
    }
}