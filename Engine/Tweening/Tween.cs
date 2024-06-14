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

	public override Vector2 Result()
	{
		return Vector2.Lerp(Start, Target, Easing(ElapsedPercentage));
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

	public override float Result()
	{
		return MathHelper.Lerp(Start, Target, Easing(ElapsedPercentage));
	}
}
