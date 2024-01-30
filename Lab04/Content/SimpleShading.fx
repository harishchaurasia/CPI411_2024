
//Matrix

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

//************Color Data
float4 AmbientColor;
float AmbientIntensity;
float3 DiffuseLightDirection;
float4 DiffuseColor;
float DiffuseIntensity;


struct VertexInput{
	float4 Position: POSITION;
	float4 Normal: NORMAL;
};

struct VertexOutput{
	float4 Position: POSITION;
	float4 Color: COLOR;
};



VertexOutput VrtexShaderFunction(VertexInput input)
{

	VertexOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float4 screenPosition = mul(viewPosition, Projection);
	output.Position = mul(viewPosition, Projection);

	// ****** Pre-vertex Lighting 90s *************
	float4 N = mul(input.Normal, WorldInverseTranspose);
	float3 L = normalize(DiffuseLightDirection);

	float lightIntensity = max(0, dot(normalize(N.xyz), normalize(DiffuseLightDirection))); // we use .xyz bcause computer doesnt like float4 with float3

	if(lightIntensity < 0) lightIntensity = 0; // to avoid negative color
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
	return output;
}

float4 PixelShaderFunction(VertexOutput input) : COLOR
{
	return saturate(input.Color + AmbientColor + AmbientIntensity) ;
}

technique MyTechnique {
	pass Pass1 {
		VertexShader = compile vs_4_0 VrtexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}