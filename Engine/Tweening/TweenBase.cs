using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Tweening;

public abstract class TweenBase<T>
{
	protected float StartTime { get; set; } = 0;

	public bool UseUnscaledTime = false;
	protected float Time => UseUnscaledTime ? Main.UnscaledTotalTime : Main.TotalTime;

	public float Duration { get; protected set; }
	public float ElapsedTime => Time - StartTime;
	public float ElapsedPercentage => Duration <= 0 ? 1 : Math.Clamp(ElapsedTime / Duration, 0, 1f);
	public float EasedElapsedPercentage => Easing(ElapsedPercentage);
	public bool IsRunning => ElapsedTime <= Duration;

	public Func<float, float> Easing { get; protected set; } = EasingFunctions.Linear;

	public T Start { get; protected set; }
	public T Target { get; protected set; }

	public TweenBase()
	{
		StartTime = Time;
	}

	public TweenBase(float duration) : this()
	{
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



	protected abstract T Lerp(T start, T target, float tPercent);

	public virtual T Result()
	{
		return Lerp(Start, Target, EasedElapsedPercentage);
	}

	public virtual T ResultAt(float tPercent)
	{
		if (tPercent < 0 || tPercent > 1)
			throw new ArgumentException(nameof(tPercent) + " needs to be between 0 and 1", nameof(tPercent));
		return Lerp(Start, Target, Easing(tPercent));
	}



	public void Restart()
	{
		StartTime = Time;
	}

	public void RestartAt(float tPercent)
	{
		if (tPercent < 0 || tPercent > 1)
			throw new ArgumentException(nameof(tPercent) + " needs to be between 0 and 1", nameof(tPercent));
		float diff = Duration * tPercent;
		StartTime = Time - diff;
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