using System;
using Mib;
using UnityEngine;

[Serializable]
public class FrameCounter
{
	[SerializeField]
	private readonly float _secondPerFrame;
	
	[SerializeField]
	private float _timer;

	public FrameCounter(int logicFrameRate)
	{
		_secondPerFrame = 1.0f / logicFrameRate;
		Logg.Info(_secondPerFrame.ToString());
	}

	public int Count(float deltaTime)
	{
		_timer += deltaTime;
		int remainFrame = 0;
		while (_timer >= _secondPerFrame)
		{
			_timer -= _secondPerFrame;
			++remainFrame;
		}

		return remainFrame;
	}
}