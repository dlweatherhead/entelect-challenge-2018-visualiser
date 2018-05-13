using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EC2018
{
	public class DirectorySelector : MonoBehaviour {
		private const string ExampleReplaysPath = "/Resources/tower-defence-matches";
		private const string DeployedReplaysPath = "/tower-defence-matches";

		public GameObject ContentListParent;
		public GameObject DirectoryButton;

		private string applicationPath;

		void Start () {
			applicationPath = GetApplicationPath ();
			var replays = GetSubFolders (GetReplaysPath());

			for (int i = 0; i < replays.Length; i++) {
				string fullPath = replays [i];
				string name = GetFolderName (fullPath);
				var b = Instantiate (DirectoryButton);
				b.transform.SetParent (ContentListParent.transform);
				b.GetComponentInChildren<DirectoryButton> ().Setup (name, fullPath, this);
			}
		}

		public void OnDirectoryButtonClicked (string title) {
			PlayerPrefs.SetString ("SelectedReplay", title);
			SceneManager.LoadScene (1, LoadSceneMode.Single);
		}

		private string GetFolderName(string path) {
			return new DirectoryInfo (path).Name;
		}

		private string[] GetSubFolders(string path) {
			return Directory.GetDirectories (path);
		}

		private string GetReplaysPath() {
			var replaysPath = DeployedReplaysPath;

			if (Application.isEditor) {
				replaysPath = ExampleReplaysPath;
			}

			return applicationPath + replaysPath;
		}
			
		private string GetApplicationPath() {
			var path = Application.dataPath;

			if (Application.platform == RuntimePlatform.WindowsPlayer) {
				path += "/../";
			} else if (Application.platform == RuntimePlatform.OSXPlayer) {
				path += "/../../";
			}

			return path;
		}
	}
}
