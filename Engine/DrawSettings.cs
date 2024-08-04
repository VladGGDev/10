using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public struct DrawSettings
{
	public SpriteSortMode SortMode { get; set; } = SpriteSortMode.FrontToBack;
	public BlendState BlendState { get; set; } = BlendState.NonPremultiplied;
	public SamplerState SamplerState { get; set; } = SamplerState.LinearClamp;
	public DepthStencilState DepthStencilState { get; set; } = null;
	public RasterizerState RasterizerState { get; set; } = null;
	public Effect Effect { get; set; } = null;
	public Matrix? TransformMatrix { get; set; } = null;

	public DrawSettings()
	{

	}
}
