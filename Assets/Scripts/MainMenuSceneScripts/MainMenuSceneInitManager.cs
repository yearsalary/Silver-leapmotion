using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuSceneInitManager : MonoBehaviour {
	public Dropdown selectGameDropdown;

	void Start () {
		List<string> options = new List<string> ();

		//컨텐츠 정보 요청??

		ContentModel content1= new ContentModel ();
		content1.id = "01";
		content1.name = "DropBoxGame";
		content1.description = "01 DropBoxGame 01 DropBoxGame 01 DropBoxGame 01 DropBoxGame";

		ContentModel content2= new ContentModel ();
		content2.id = "02";
		content2.name = "StackBoxGame";
		content2.description = "02 StackBoxGame 02 StackBoxGame 02 StackBoxGame 02 StackBoxGame";


		//컨텐츠 정보 요청??
		options.Add ("DropBoxGame");
		options.Add ("StackBoxGame");
		selectGameDropdown.ClearOptions ();
		selectGameDropdown.AddOptions (options);

	}
}
