
using UnityEngine;
using System.Collections;
using System.IO;

public class OBJExportManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string sDirPath;
		string fileName = "test.obj";
		string filePath;

		sDirPath = Application.persistentDataPath + "/OBJs";
		DirectoryInfo di = new DirectoryInfo(sDirPath);
		if (di.Exists == false)
		{
			di.Create();
		}
		filePath = sDirPath + "/" + fileName;
		System.IO.File.Create (filePath);

		OBJExporter exporter = new OBJExporter();
		exporter.Export (filePath);
	}

	// Update is called once per frame
	void Update () {

	}
}
