using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuSceneInitManager : MonoBehaviour {
	public Dropdown selectGameModeDropdwon;
	public Dropdown selectGameDropdown;
	public Text contentDescription;
	private List<ContentModel> gameContentList;
	string gameMode;

	void Start () {
		List<string> options = new List<string> ();
		gameContentList = new List<ContentModel> ();

		//컨텐츠 정보 요청??
		ContentModel content1= new ContentModel ();
		content1.id = "01";
		content1.name = "DropBoxGame";
		content1.description = "01 DropBoxGame 01 DropBoxGame 01 DropBoxGame 01 DropBoxGame";
		content1.modeType = "Single";

		ContentModel content2= new ContentModel ();
		content2.id = "02";
		content2.name = "StackBoxGame";
		content2.description = "02 StackBoxGame 02 StackBoxGame 02 StackBoxGame 02 StackBoxGame";
		content2.modeType = "Single";

		ContentModel content3= new ContentModel ();
		content3.id = "03";
		content3.name = "TableHockeyGame";
		content3.description = "03 TableHockeyGame 03 TableHockeyGame 03 TableHockeyGame 03 TableHockeyGame";
		content3.modeType = "Multi";

		gameContentList.Add (content1);
		gameContentList.Add (content2);
		gameContentList.Add (content3);

		options.Add ("Single");
		options.Add ("Multi");

		selectGameModeDropdwon.ClearOptions ();
		selectGameModeDropdwon.AddOptions (options);
		options.Clear ();
	
		ChangeGameMode ();
		ChangeGame();
	}

	public void ChangeGameMode() {
		List<string> options = new List<string> ();
		gameMode = selectGameModeDropdwon.captionText.text;

		foreach(ContentModel content in gameContentList) {
			Debug.Log (content.name);
			if (content.modeType.Equals (gameMode))
				options.Add (content.name);
		}

		selectGameDropdown.ClearOptions ();
		selectGameDropdown.AddOptions (options);
		ChangeGame();
	}

	public void ChangeGame() {
		string gameName = selectGameDropdown.captionText.text;
		ContentModel selectedGame = gameContentList.Find (item => item.name.Equals (gameName));
		contentDescription.text = selectedGame.description;

	}
}
