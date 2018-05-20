
/// <summary>
/// Static store for Constants.
/// Tags, etc.
/// </summary>
using UnityEngine;


namespace EC2018 {
	public static class Constants {

		public static class Tags {
			public const string Missile = "Missile";
			public const string Attack = "Attack";
			public const string Defense = "Defense";
			public const string Energy = "Energy";
			public const string GroundTile = "GroundTile";
			public const string PlayerA = "Player A";
			public const string PlayerB = "Player B";
			public const string UIHolder = "UI Holder";
			public const string GameManager = "GameManager";
		}

		public static class ParentNames {
			public const string Buildings = "Buildings";
			public const string GroundTiles = "GroundTiles";
			public const string Missiles = "Missiles";
		}

		public static class PlayerPrefKeys {
			public const string SelectedReplay = "SelectedReplay";
		}

		public static class UI {
			public const string Play = "Play";
			public const string Pause = "Pause";
		}

		public static class Paths {
			public const string ExampleReplays = "/Resources/tower-defence-matches";
			public const string DeployedReplays = "/tower-defence-matches";
			public const string WindowsPathBackNavigation = "/../";
			public const string OSXPathBackNavigation = "/../../";
			public const string LinuxPathBackNavigation = "/../../";
			public const string MapName = "/JsonMap.json";
			public const string RoundFolderNamePrefix = "Round ";
			public const string EndGameStateFileName = "endGameState.txt";
			public static string ApplicationPath {
				get {
					if (Application.platform == RuntimePlatform.WindowsPlayer) {
						return Application.dataPath + Constants.Paths.WindowsPathBackNavigation;
					} else if (Application.platform == RuntimePlatform.OSXPlayer) {
						return Application.dataPath + Constants.Paths.OSXPathBackNavigation;
					} else if (Application.platform == RuntimePlatform.LinuxPlayer) {
						return Application.dataPath + Constants.Paths.LinuxPathBackNavigation;
					} else {
						return Application.dataPath;
					}
				}
			}
		}
	}
}
