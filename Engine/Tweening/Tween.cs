using Microsoft.Xna.Framework;

namespace Tweening;

public class Tween : TweenBase<Vector2>
{
	public Tween() : base()
	{
	}

	public Tween(float duration) : base(duration)
	{
	}

	public Tween(Vector2 start, Vector2 target, float duration) : base(start, target, duration)
	{
	}

	protected override Vector2 Lerp(Vector2 start, Vector2 target, float tPercent)
	{
		return Vector2.Lerp(start, target, tPercent);
	}
}

public class FloatTween : TweenBase<float>
{
	public FloatTween() : base()
	{
	}

	public FloatTween(float duration) : base(duration)
	{
	}

	public FloatTween(float start, float target, float duration) : base(start, target, duration)
	{
	}

	protected override float Lerp(float start, float target, float tPercent)
	{
		return MathHelper.Lerp(start, target, tPercent);
	}
}

public class ColorTween : TweenBase<Color>
{
	public ColorTween()
	{
	}

	public ColorTween(float duration) : base(duration)
	{
	}

	public ColorTween(Color start, Color target, float duration) : base(start, target, duration)
	{
	}

	protected override Color Lerp(Color start, Color target, float tPercent)
	{
		return Color.Lerp(start, target, tPercent);
	}
}
