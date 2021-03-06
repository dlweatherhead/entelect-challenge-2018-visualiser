﻿using System;

namespace EC2018
{
	public static class CommandLineUtil
	{
		const string RoundStep = "-round-step";

		const float defaultRoundStep = 1f;

		public static float GetRoundStep() {
			var args = System.Environment.GetCommandLineArgs ();

			for(int i=0; i < args.Length; i++) {
				if(args[i] == RoundStep) {
					return float.Parse (args [i + 1]);
				}
			}

			return defaultRoundStep;
		}
	}
}

