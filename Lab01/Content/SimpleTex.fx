struct VertexPositionTexture 
{
	float4 Position: POSITION;
	float4 TextureCoordinate : TEXCOORD;
};

texture MyTexture;
sampler MySampler = sampler_state{
Texture = <MyTexture> ;

};

VertexPositionTexture MyVertexShader(VertexPositionTexture input) 
{
	return input;
}

float4 MyPixelShader(VertexPositionTexture input) : COLOR
{
	return tex2D(MySampler, input.TextureCoordinate);
}
technique MyTechnique {
	pass Pass1 {
		VertexShader = compile vs_4_0 MyVertexShader();
		PixelShader = compile ps_4_0 MyPixelShader();
	}
}