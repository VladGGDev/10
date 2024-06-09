using System;
using System.Timers;


public class Timeout
{
	Timer _timer;
	Action _action;

	public Timeout(float milliseconds, Action action)
	{
		_action = action;
		_timer = new Timer
		{
			AutoReset = false,
			Enabled = true,
			Interval = milliseconds
		};
		_timer.Elapsed += TimerElapsed;
	}

	void TimerElapsed(object sender, ElapsedEventArgs e)
	{
		_action();
		(sender as Timer).Dispose();
	}
}