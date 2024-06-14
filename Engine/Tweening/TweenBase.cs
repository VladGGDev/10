using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Tweening;

public abstract class TweenBase<T>
{
	float _startTime = 0;

	public bool UseUnscaledTime = false;
	float Time => UseUnscaledTime ? Main.UnscaledTotalTime : Main.TotalTime;

	public float Duration { get; private set; }
	public float ElapsedTime => Time - _startTime;
	public float ElapsedPercentage => Duration == 0 ? 1 : Math.Clamp(ElapsedTime / Duration, 0, 1f);
	public float EasedElapsedPercentage => Easing(ElapsedPercentage);
	public bool IsRunning => ElapsedTime <= Duration;

	public Func<float, float> Easing { get; private set; } = EasingFunctions.Linear;

	public T Start { get; private set; }
	public T Target { get; private set; }

	public TweenBase()
	{
		_startTime = Time;
	}

	public TweenBase(float duration) : this()
	{
		if (duration <= 0)
			throw new ArgumentException(nameof(duration) + " cannot be less than or equal to 0", nameof(duration));
		Duration = duration;
	}

	public TweenBase(T start, T target, float duration) : this(duration)
	{
		Start = start;
		Target = target;
	}


	public TweenBase<T> Clone()
	{
		return (TweenBase<T>)MemberwiseClone();
	}




	public abstract T Result();



	public void Restart()
	{
		_startTime = Time;
	}

	public void RestartAt(float tPercent)
	{
		if (tPercent < 0 || tPercent > 1)
			throw new ArgumentException(nameof(tPercent) + " needs to be between 0 and 1", nameof(tPercent));
		float diff = Duration * tPercent;
		_startTime = Time - diff;
	}


	// Factory pattern
	public TweenBase<T> SetDuration(float duration)
	{
		Duration = duration;
		return this;
	}

	public TweenBase<T> SetDurationWithOffset(float duration)
	{
		float prevElapsedPercentage = ElapsedPercentage;
		Duration = duration;
		RestartAt(prevElapsedPercentage);
		return this;
	}

	public TweenBase<T> SetEasing(Func<float, float> easing)
	{
		Easing = easing;
		return this;
	}

	public TweenBase<T> SetStart(T start)
	{
		Start = start;
		return this;
	}

	public TweenBase<T> SetTarget(T target)
	{
		Target = target;
		return this;
	}
}