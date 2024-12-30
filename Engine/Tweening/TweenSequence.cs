using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Tweening;

public class TweenSequence
{
	List<Tween> _tweens = new List<Tween>();
	float _startTime = 0;
	public bool UseUnscaledTime = false;
	float Time => UseUnscaledTime ? Main.UnscaledTotalTime : Main.TotalTime;
	public float TotalDuration { get; private set; } = 0;

	public float ElapsedTime => Time - _startTime;
	public float ElapsedPercentage => Math.Clamp(ElapsedTime / TotalDuration, 0, 1f);
	public float EasedElapsedPercentage => Easing(Math.Clamp(ElapsedTime / TotalDuration, 0, 1f));
	public bool IsRunning => ElapsedTime <= TotalDuration;

	public Func<float, float> Easing { get; set; } = EasingFunctions.Linear;


	public TweenSequence() { }
	public TweenSequence(Func<float, float> easing) => Easing = easing;



	public Vector2 Result()
	{
		if (!IsRunning)
			return _tweens[^1].Target;
		float t;
		int i;
		for (i = 0, t = EasedElapsedPercentage; i < _tweens.Count && t > _tweens[i].Duration / TotalDuration; t -= _tweens[i].Duration / TotalDuration, i++) ;
		if (i >= _tweens.Count)
			return _tweens[^1].Target; // Catch extremely rare error
		Tween tw = _tweens[i];
		return Vector2.Lerp(tw.Start, tw.Target, tw.Easing(t / tw.Duration * TotalDuration));
	}



	public TweenSequence Add(Tween tween)
	{
		TotalDuration += tween.Duration;
		_tweens.Add((Tween)tween.Clone());
		return this;
	}

	public TweenSequence Add(TweenBase<Vector2> tween)
	{
		TotalDuration += tween.Duration;
		_tweens.Add((Tween)tween.Clone());
		return this;
	}

	public TweenSequence AddDelay(float delay)
	{
		if (_tweens.Count == 0)
			throw new SystemException("Cannot add a delay without a starting position. Use StartWithDelay function instead!");
		TotalDuration += delay;
		_tweens.Add(new Tween(_tweens[^1].Target, _tweens[^1].Target, delay));
		return this;
	}

	public TweenSequence StartWithDelay(Vector2 start, float delay)
	{
		TotalDuration += delay;
		_tweens.Add(new Tween(start, start, delay));
		return this;
	}



	public void Restart()
	{
		_startTime = Time;
	}

	public void RestartAt(float tPercent)
	{
		if (tPercent < 0 || tPercent > 1)
			throw new ArgumentException(nameof(tPercent) + " needs to be between 0 and 1", nameof(tPercent));
		float diff = TotalDuration * tPercent;
		_startTime = Time - diff;
	}
}



//public class TweenSequence<T>
//{
//	List<TweenBase<T>> _tweens = new List<TweenBase<T>>();
//	float _startTime = 0;
//	public bool UseUnscaledTime = false;
//	float Time => UseUnscaledTime ? Main.UnscaledTotalTime : Main.TotalTime;
//	public float TotalDuration { get; private set; } = 0;

//	public float ElapsedTime => Time - _startTime;
//	public float ElapsedPercentage => Math.Clamp(ElapsedTime / TotalDuration, 0, 1f);
//	public float EasedElapsedPercentage => Easing(Math.Clamp(ElapsedTime / TotalDuration, 0, 1f));
//	public bool IsRunning => ElapsedTime <= TotalDuration;

//	public Func<float, float> Easing { get; set; } = EasingFunctions.Linear;


//	public TweenSequence() { }
//	public TweenSequence(Func<float, float> easing) => Easing = easing;



//	public T Result()
//	{
//		if (!IsRunning)
//			return _tweens[^1].Target;
//		float t;
//		int i;
//		for (i = 0, t = EasedElapsedPercentage; i < _tweens.Count && t > _tweens[i].Duration / TotalDuration; t -= _tweens[i].Duration / TotalDuration, i++) ;
//		if (i >= _tweens.Count)
//			return _tweens[^1].Target; // Catch extremely rare error
//		TweenBase<T> tw = _tweens[i].Clone();
//		return tw.ResultAt(tw.Easing(t / tw.Duration * TotalDuration));
//	}



//	public TweenSequence<T> Add(TweenBase<T> tween)
//	{
//		TotalDuration += tween.Duration;
//		_tweens.Add(tween.Clone());
//		return this;
//	}

//	public TweenSequence<T> AddDelay(float delay)
//	{
//		if (_tweens.Count == 0)
//			throw new SystemException("Cannot add a delay without a starting position. Use StartWithDelay function instead!");
//		TotalDuration += delay;
//		TweenBase<T> tween = default;
//		tween.SetStart(_tweens[^1].Target).SetTarget(_tweens[^1].Target).SetDuration(delay);
//		_tweens.Add(tween);
//		return this;
//	}

//	//public TweenSequence<T> StartWithDelay(T start, float delay)
//	//{
//	//	TotalDuration += delay;
//	//	TweenBase<T> tween = new();
//	//	tween.SetStart(start).SetTarget(start).SetDuration(delay);
//	//	_tweens.Add(tween);
//	//	return this;
//	//}



//	public void Restart()
//	{
//		_startTime = Time;
//	}

//	public void RestartAt(float tPercent)
//	{
//		if (tPercent < 0 || tPercent > 1)
//			throw new ArgumentException(nameof(tPercent) + " needs to be between 0 and 1", nameof(tPercent));
//		float diff = TotalDuration * tPercent;
//		_startTime = Time - diff;
//	}
//}