using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018;
using UnityEngine.UI;

namespace EC2018
{
	public class DirectoryButton : MonoBehaviour {

		public Button ButtonComponent;
		public Text Title;
        string fullPath;

        DirectorySelector directorySelector;

        void Start() {
			ButtonComponent.onClick.AddListener (HandleClick);
		}

		public void Setup(string title, string fullPath, DirectorySelector directorySelector) {
			Title.text = title;
			this.fullPath = fullPath;
			this.directorySelector = directorySelector;
		}

		public void HandleClick() {
			directorySelector.OnDirectoryButtonClicked (fullPath);
		}
	}
}
