/*
 * This file has been moved to StoryRobot/SROptions/SROptions.Test.cs
 * This empty file is left here to ensure it is properly overwritten when importing a new version of the package over an old version.
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
#if !DISABLE_SRDEBUGGER
using SRDebugger;
using SRDebugger.Services;
#endif
using SRF;
using SRF.Service;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public partial class SROptions
{
	private string _testString;
	[Category("Test")]
	public string TestString
	{
		get { return _testString; }
		set
		{
			_testString = value;
		}
	}

}