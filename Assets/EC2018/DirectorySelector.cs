using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace EC2018
{
	public class DirectorySelector : MonoBehaviour {

		public GameObject ContentListParent;
		public GameObject DirectoryButton;

        string applicationPath;

        void Start () {
			applicationPath = Constants.Paths.ApplicationPath;
			var replays = GetSubFolders (GetReplaysPath());

			for (int i = 0; i < replays.Length; i++) {
				var fullPath = replays [i];
                var folderName = GetFolderName (fullPath);
				var b = Instantiate (DirectoryButton);
				b.transform.SetParent (ContentListParent.transform);
				b.GetComponentInChildren<DirectoryButton> ().Setup (folderName, fullPath, this);
			}
		}

		public void OnDirectoryButtonClicked (string title) {
			PlayerPrefs.SetString (Constants.PlayerPrefKeys.SelectedReplay, title);
			SceneManager.LoadScene (1, LoadSceneMode.Single);
		}

        string GetFolderName(string path) {
            return new DirectoryInfo(path).Name;
        }

        string[] GetSubFolders(string path) {
            return Directory.GetDirectories(path);
        }

        string GetReplaysPath() {
            var replaysPath = Constants.Paths.DeployedReplays;

            if (Application.isEditor) {
                replaysPath = Constants.Paths.ExampleReplays;
            }

            return applicationPath + replaysPath;
        }
    }
}
