using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DrawPass
{
	internal SpriteBatch _SpriteBatch { get; init; }
	public int Order { get; set; } = 0;
	public DrawSettings Settings;
	public static Dictionary<string, DrawPass> Passes { get; set; } = new();

	public DrawPass(SpriteBatch spriteBatch, int order, DrawSettings settings)
	{
		_SpriteBatch = spriteBatch;
		Order = order;
		Settings = settings;
	}


	#region Draw and DrawString methods from SpriteBatch
	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

	public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
		=> _SpriteBatch.Draw(texture, position, sourceRectangle, color);

	public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
		=> _SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);

	public void Draw(Texture2D texture, Vector2 position, Color color)
		=> _SpriteBatch.Draw(texture, position, color);

	public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
		=> _SpriteBatch.Draw(texture, destinationRectangle, color);

	public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color);

	public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);

	public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);

	public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);

	public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color);

	public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);

	public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);

	public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, bool rtl)
		=> _SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
	#endregion


	#region SpriteBatch extensions
	public void DrawSquareOutline(Vector2 position, Vector2 size, Color color)
		=> _SpriteBatch.DrawSquareOutline(position, size, color);

	public void DrawSquareOutline(Point position, Point size, Color color)
		=> _SpriteBatch.DrawSquareOutline(position, size, color);

	public void DrawCircleOutline(Point position, float radius, Color color)
		=> _SpriteBatch.DrawCircleOutline(position, radius, color);

	public void DrawCircleOutline(Vector2 position, float radius, Color color)
		=> _SpriteBatch.DrawCircleOutline(position, radius, color);

	public void DrawSimlple(Texture2D texture, Vector2 position, float rotation, Vector2 size, Color color, float layerDepth)
		=> _SpriteBatch.DrawSimlple(texture, position, rotation, size, color, layerDepth);
	#endregion
}
