using System;

namespace Tweening;

// All easing functions that are properties were taken from https://easings.net/
public struct EasingFunctions
{
	const float c1 = 1.70158f;
	const float c2 = c1 * 1.525f;
	const float c3 = c1 + 1;
	const float c4 = 2f * MathF.PI / 3f;
	const float c5 = 2 * MathF.PI / 4.5f;

	const float n1 = 7.5625f;
	const float d1 = 2.75f;


	public static readonly Func<float, float> Linear = x => x;

	public static readonly Func<float, float> EaseInSine = x => 1 - MathF.Cos(x * MathF.PI / 2f);
	public static readonly Func<float, float> EaseOutSine = x => MathF.Sin(x * MathF.PI / 2f);
	public static readonly Func<float, float> EaseInOutSine = x => -(MathF.Cos(MathF.PI * x) - 1) / 2f;

	public static readonly Func<float, float> EaseInQuad = x => x * x;
	public static readonly Func<float, float> EaseOutQuad = x => 1 - (1 - x) * (1 - x);
	public static readonly Func<float, float> EaseInOutQuad = x => x < 0.5f ? 2 * x * x : 1 - MathF.Pow(-2 * x + 2, 2) / 2;

	public static readonly Func<float, float> EaseInCubic = x => x * x * x;
	public static readonly Func<float, float> EaseOutCubic = x => 1 - MathF.Pow(1 - x, 3);
	public static readonly Func<float, float> EaseInOutCubic = x => x < 0.5f ? 4 * x * x * x : 1 - MathF.Pow(-2 * x + 2, 3) / 2;

	public static readonly Func<float, float> EaseInQuart = x => x * x * x * x;
	public static readonly Func<float, float> EaseOutQuart = x => 1 - MathF.Pow(1 - x, 4);
	public static readonly Func<float, float> EaseInOutQuart = x => x < 0.5f ? 8 * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 4) / 2;

	public static readonly Func<float, float> EaseInQuint = x => x * x * x * x * x;
	public static readonly Func<float, float> EaseOutQuint = x => 1 - MathF.Pow(1 - x, 5);
	public static readonly Func<float, float> EaseInOutQuint = x => x < 0.5f ? 16 * x * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 5) / 2;

	public static readonly Func<float, float> EaseInExpo = x => x == 0 ? 0 : MathF.Pow(2, 10 * x - 10);
	public static readonly Func<float, float> EaseOutExpo = x => x == 1 ? 1 : 1 - MathF.Pow(2, -10 * x);
	public static readonly Func<float, float> EaseInOutExpo =
		x => x == 0
			? 0
			: x == 1
				? 1
				: x < 0.5f ? MathF.Pow(2, 20 * x - 10) / 2 : (2 - MathF.Pow(2, -20 * x + 10)) / 2;

	public static readonly Func<float, float> EaseInCirc = x => 1 - MathF.Sqrt(1 - MathF.Pow(x, 2));
	public static readonly Func<float, float> EaseOutCirc = x => MathF.Sqrt(1 - MathF.Pow(x - 1, 2));
	public static readonly Func<float, float> EaseInOutCirc =
		x => x < 0.5
			? (1 - MathF.Sqrt(1 - MathF.Pow(2 * x, 2))) / 2
			: (MathF.Sqrt(1 - MathF.Pow(-2 * x + 2, 2)) + 1) / 2;

	public static readonly Func<float, float> EaseInBack = x => c3 * x * x * x - c1 * x * x;
	public static Func<float, float> EaseInBackCustom(float amount)
		=> x => (amount + 1) * x * x * x - amount * x * x;
	public static readonly Func<float, float> EaseOutBack = x => 1 + c3 * MathF.Pow(x - 1, 3) + c1 * MathF.Pow(x - 1, 2);
	public static Func<float, float> EaseOutBackCustom(float amount)
		=> x => 1 + (amount + 1) * MathF.Pow(x - 1, 3) + amount * MathF.Pow(x - 1, 2);
	public static readonly Func<float, float> EaseInOutBack =
		x => x < 0.5
			? MathF.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2) / 2
			: (MathF.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
	public static Func<float, float> EaseInOutBackCustom(float amount) => EaseInOutBackCustom(amount, amount);
	public static Func<float, float> EaseInOutBackCustom(float amount1, float amount2)
		=> x => x < 0.5
			? MathF.Pow(2 * x, 2) * ((amount1 + 1) * 2 * x - amount1) / 2
			: (MathF.Pow(2 * x - 2, 2) * ((amount2 + 1) * (x * 2 - 2) + amount2) + 2) / 2;

	public static readonly Func<float, float> EaseInElastic =
		x => x == 0
			? 0
			: x == 1
				? 1
				: -MathF.Pow(2, 10 * x - 10) * MathF.Sin((x * 10 - 10.75f) * c4);
	public static Func<float, float> EaseInElasticCustom(int revolutions)
		=> x => x == 0
			? 0
			: x == 1
				? 1
				: -MathF.Pow(2, 10 * x - 10) * MathF.Sin((x * (revolutions * 3 + 1) - 10.75f) * c4);
	public static readonly Func<float, float> EaseOutElastic =
		x => x == 0
			? 0
			: x == 1
				? 1
				: MathF.Pow(2, -10 * x) * MathF.Sin((x * 10 - 0.75f) * c4) + 1;
	public static Func<float, float> EaseOutElasticCustom(int revolutions)
		=> x => x == 0
			? 0
			: x == 1
				? 1
				: MathF.Pow(2, -10 * x) * MathF.Sin((x * (revolutions * 3 + 1) - 0.75f) * c4) + 1;
	public static readonly Func<float, float> EaseInOutElastic =
		x => x == 0
			? 0
			: x == 1
				? 1
				: x < 0.5f
					? -(MathF.Pow(2, 20 * x - 10) * MathF.Sin((20 * x - 11.125f) * c5)) / 2
					: MathF.Pow(2, -20 * x + 10) * MathF.Sin((20 * x - 11.125f) * c5) / 2 + 1;
	//public static Func<float, float> EaseInOutElasticCustom(int revolutions1, int revolutions2)
	//	=> x => x == 0
	//		? 0
	//		: x == 1
	//			? 1
	//			: x < 0.5f
	//				? -(MathF.Pow(2, 20 * x - 10) * MathF.Sin((20 * (revolutions1 * 3 + 1) - 11.125f) * c5)) / 2
	//				: MathF.Pow(2, -20 * x + 10) * MathF.Sin((20 * (revolutions2 * 3 + 1) - 11.125f) * c5) / 2 + 1;

	public static readonly Func<float, float> EaseInBounce = x => 1 - EaseOutBounce(1 - x);
	public static readonly Func<float, float> EaseOutBounce = x =>
	{
		if (x < 1 / d1)
		{
			return n1 * x * x;
		}
		else if (x < 2 / d1)
		{
			return n1 * (x -= 1.5f / d1) * x + 0.75f;
		}
		else if (x < 2.5 / d1)
		{
			return n1 * (x -= 2.25f / d1) * x + 0.9375f;
		}
		else
		{
			return n1 * (x -= 2.625f / d1) * x + 0.984375f;
		}
	};
	public static readonly Func<float, float> EaseInOutBounce =
		x => x < 0.5f
			? (1 - EaseOutBounce(1 - 2 * x)) / 2
			: (1 + EaseOutBounce(2 * x - 1)) / 2;

	public static Func<float, float> LinearYoYo(float fadeInP = 0.5f, float fadeOutP = 0.5f)
		=> x =>
			{
				float stayP = 1 - fadeInP - fadeOutP;
				if (x <= fadeInP)
					return x / fadeInP;
				else if (x >= fadeInP + stayP)
					return 1 - (x - fadeInP - stayP) / fadeOutP;
				else
					return 1;
			};
}
