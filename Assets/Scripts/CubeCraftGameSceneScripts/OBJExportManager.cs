
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic; 
using UnityEngine.UI;
using System.Threading;
using LitJson;

public class OBJExportManager : MonoBehaviour {
	public bool onlySelectedObjects = true;
	public bool applyPosition = true;
	public bool applyRotation = true;
	public bool applyScale = true;
	public bool generateMaterials = true;
	public bool exportTextures = true;
	public bool splitObjects = true;
	public bool autoMarkTexReadable = false;
	public bool objNameAddIdNum = false;
	public string uploadURL = "http://117.17.158.66:8080/vrain/client/uploadObj";
	private string lastExportFolder;
	private string versionString = "v2.0";
	public GameObject targetParent;
	public String originFileName;

	public float progress;
	public Text progressText;
	public Button gameStopBtn;
	public Canvas titleCanvas;
	public InputField titleInputField;
	public Button checkBtn;


	public GameObject[] GetChilds(GameObject parent) {
		Transform[] childTransforms = parent.GetComponentsInChildren<Transform>();
		List<GameObject> childObjList = new List<GameObject> ();
		foreach (Transform child in childTransforms)
			childObjList.Add (child.gameObject);

		return childObjList.ToArray();
	}

	public void OnSaveBtn() {
		titleCanvas.enabled = true;
		checkBtn.interactable = false;
	}

	public void CheckTitleInput() {
		if (titleInputField.text.Length > 0)
			checkBtn.interactable = true;
		else
			checkBtn.interactable = false;
	}
		
	public void OnExport () {
		gameStopBtn.interactable = false;
		originFileName = titleInputField.text;
		progressText.text = "추출중...";
		StartCoroutine (Export ());
	}

	public IEnumerator Export()
	{
		string sDirPath;
		string fileName = originFileName + ".obj";
		string filePath;
		string exportPath;

		yield return new WaitForSeconds (0.3f);

		sDirPath = Application.persistentDataPath + "/OBJs";
		DirectoryInfo di = new DirectoryInfo(sDirPath);
		if (di.Exists == false)
			di.Create();
		filePath = sDirPath + "/" + fileName;
		System.IO.File.Create (filePath).Close ();
		exportPath = filePath;

		//init stuff
		Dictionary<string, bool> materialCache = new Dictionary<string, bool>();
		var exportFileInfo = new System.IO.FileInfo(exportPath);
		lastExportFolder = exportFileInfo.Directory.FullName;
		string baseFileName = System.IO.Path.GetFileNameWithoutExtension(exportPath);

		//get list of required export things
		MeshFilter[] sceneMeshes;
		if (onlySelectedObjects)
		{
			List<MeshFilter> tempMFList = new List<MeshFilter>();
			foreach (GameObject g in GetChilds(targetParent))
			{

				MeshFilter f = g.GetComponent<MeshFilter>();
				if (f != null)
				{
					tempMFList.Add(f);
				}

			}
			sceneMeshes = tempMFList.ToArray();
		}
		else
		{
			sceneMeshes = FindObjectsOfType(typeof(MeshFilter)) as MeshFilter[];

		}
			
		//work on export
		StringBuilder sb = new StringBuilder();
		StringBuilder sbMaterials = new StringBuilder();
		sb.AppendLine("# Export of " + Application.loadedLevelName);
		sb.AppendLine("# from Aaro4130 OBJ Exporter " + versionString);
		if (generateMaterials)
		{
			sb.AppendLine("mtllib " + baseFileName + ".mtl");
		}
		float maxExportProgress = (float)(sceneMeshes.Length + 1);
		int lastIndex = 0;
		for(int i = 0; i < sceneMeshes.Length; i++)
		{
			string meshName = sceneMeshes[i].gameObject.name;
			progress = (float)(i + 1) / maxExportProgress;

			MeshFilter mf = sceneMeshes[i];
			MeshRenderer mr = sceneMeshes[i].gameObject.GetComponent<MeshRenderer>();

			if (splitObjects)
			{
				string exportName = meshName;
				if (objNameAddIdNum)
				{
					exportName += "_" + i;
				}
				sb.AppendLine("g " + exportName);
			}
			if(mr != null && generateMaterials)
			{
				Material[] mats = mr.sharedMaterials;
				for(int j=0; j < mats.Length; j++)
				{
					Material m = mats[j];
					if (!materialCache.ContainsKey(m.name))
					{
						materialCache[m.name] = true;
						sbMaterials.Append(MaterialToString(m));
						sbMaterials.AppendLine();
					}
				}
			}

			//export the meshhh :3
			Mesh msh = mf.sharedMesh;
			int faceOrder = (int)Mathf.Clamp((mf.gameObject.transform.lossyScale.x * mf.gameObject.transform.lossyScale.z), -1, 1);

			//export vector data (FUN :D)!
			foreach (Vector3 vx in msh.vertices)
			{
				Vector3 v = vx;
				if (applyScale)
				{
					v = MultiplyVec3s(v, mf.gameObject.transform.lossyScale);
				}

				if (applyRotation)
				{

					v = RotateAroundPoint(v, Vector3.zero, mf.gameObject.transform.rotation);
				}

				if (applyPosition)
				{
					v += mf.gameObject.transform.position;
				}
				v.x *= -1;
				sb.AppendLine("v " + v.x + " " + v.y + " " + v.z);
			}
			foreach (Vector3 vx in msh.normals)
			{
				Vector3 v = vx;

				if (applyScale)
				{
					v = MultiplyVec3s(v, mf.gameObject.transform.lossyScale.normalized);
				}
				if (applyRotation)
				{
					v = RotateAroundPoint(v, Vector3.zero, mf.gameObject.transform.rotation);
				}
				v.x *= -1;
				sb.AppendLine("vn " + v.x + " " + v.y + " " + v.z);

			}
			foreach (Vector2 v in msh.uv)
			{
				sb.AppendLine("vt " + v.x + " " + v.y);
			}

			for (int j=0; j < msh.subMeshCount; j++)
			{
				if(mr != null && j < mr.sharedMaterials.Length)
				{
					string matName = mr.sharedMaterials[j].name;
					sb.AppendLine("usemtl " + matName);
				}
				else
				{
					sb.AppendLine("usemtl " + meshName + "_sm" + j);
				}

				int[] tris = msh.GetTriangles(j);
				for(int t = 0; t < tris.Length; t+= 3)
				{
					int idx2 = tris[t] + 1 + lastIndex;
					int idx1 = tris[t + 1] + 1 + lastIndex;
					int idx0 = tris[t + 2] + 1 + lastIndex;
					if(faceOrder < 0)
					{
						sb.AppendLine("f " + ConstructOBJString(idx2) + " " + ConstructOBJString(idx1) + " " + ConstructOBJString(idx0));
					}
					else
					{
						sb.AppendLine("f " + ConstructOBJString(idx0) + " " + ConstructOBJString(idx1) + " " + ConstructOBJString(idx2));
					}

				}
			}

			lastIndex += msh.vertices.Length;
		}

		//write to disk
		StreamWriter streamWriter = System.IO.File.CreateText(exportPath);

		foreach (char ch in sb.ToString()) {
			streamWriter.Write (ch);
		}
		streamWriter.Close ();

		//System.IO.File.WriteAllText(exportPath, sb.ToString());
		if (generateMaterials)
		{
			//streamWriter = System.IO.File.WriteAllText(exportFileInfo.Directory.FullName + "\\" + baseFileName + ".mtl", sbMaterials.ToString());
			streamWriter = System.IO.File.CreateText(exportFileInfo.Directory.FullName + "\\" + baseFileName + ".mtl");
			foreach (char ch in sbMaterials.ToString()) {
				streamWriter.Write (ch);
			}
			streamWriter.Close();
		}


		progressText.text = "파일생성 완료됨...서버 전송중";
		StartCoroutine(Upload_OBJ_and_MLTFile (uploadURL, "file:///"+exportPath, "file:///"+exportFileInfo.Directory.FullName + "\\" + baseFileName + ".mtl"));
		//export complete, close progress dialog
		//EditorUtility.ClearProgressBar();
	}

	string MaterialToString(Material m)
	{
		StringBuilder sb = new StringBuilder();

		sb.AppendLine("newmtl " + m.name);


		//add properties
		if (m.HasProperty("_Color"))
		{
			sb.AppendLine("Kd " + m.color.r.ToString() + " " + m.color.g.ToString() + " " + m.color.b.ToString());
			if (m.color.a < 1.0f)
			{
				//use both implementations of OBJ transparency
				sb.AppendLine("Tr " + (1f - m.color.a).ToString());
				sb.AppendLine("d " + m.color.a.ToString());
			}
		}
		if (m.HasProperty("_SpecColor"))
		{
			Color sc = m.GetColor("_SpecColor");
			sb.AppendLine("Ks " + sc.r.ToString() + " " + sc.g.ToString() + " " + sc.b.ToString());
		}
		if (exportTextures) {
			//diffuse
			string exResult = TryExportTexture("_MainTex", m);
			if (exResult != "false")
			{
				sb.AppendLine("map_Kd " + exResult);
			}
			//spec map
			exResult = TryExportTexture("_SpecMap", m);
			if (exResult != "false")
			{
				sb.AppendLine("map_Ks " + exResult);
			}
			//bump map
			exResult = TryExportTexture("_BumpMap", m);
			if (exResult != "false")
			{
				sb.AppendLine("map_Bump " + exResult);
			}

		}

		sb.AppendLine("illum 2");
		return sb.ToString();
	}

	string TryExportTexture(string propertyName,Material m)
	{
		if (m.HasProperty(propertyName))
		{
			Texture t = m.GetTexture(propertyName);
			if(t != null)
			{
				return ExportTexture((Texture2D)t);
			}
		}
		return "false";
	}


	private string ConstructOBJString(int index)
	{
		string idxString = index.ToString();
		return idxString + "/" + idxString + "/" + idxString;
	}

	Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
	{
		return angle * (point - pivot) + pivot;
	}

	Vector3 MultiplyVec3s(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	string ExportTexture(Texture2D t)
	{
		try
		{
			string exportName = lastExportFolder + "\\" + t.name + ".png";
			Texture2D exTexture = new Texture2D(t.width, t.height, TextureFormat.ARGB32, false);
			exTexture.SetPixels(t.GetPixels());
			System.IO.File.WriteAllBytes(exportName, exTexture.EncodeToPNG());
			return exportName;
		}
		catch (System.Exception ex)
		{
			Debug.Log("Could not export texture : " + t.name + ". is it readable?");
			return "null";
		}

	}

	IEnumerator Upload_OBJ_and_MLTFile(string uploadURL, string localOBJFilePath, string localMTLFilePath) {
		//TODO: OBJfile upload.
		WWW objFile = new WWW (localOBJFilePath);
		yield return objFile;
		WWW mtlFile = new WWW (localMTLFilePath);
		yield return mtlFile;

		WWWForm postForm = new WWWForm ();

		PlayRecordData palyData = new PlayRecordData ("aaa","bbb","큐브크래프트","0","0","1","1");
		postForm.AddField("result", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(palyData, true))));
		postForm.AddBinaryData("obj",objFile.bytes, originFileName + ".obj");
		postForm.AddBinaryData("mtl",mtlFile.bytes, originFileName + ".mtl");
		WWW upload = new WWW (uploadURL, postForm);
		yield return upload;

		if (upload.error == null) {
			Debug.Log (upload.text);
		} else {
			Debug.Log (upload.error);
		}



		progressText.text = "완료됨";
		gameStopBtn.interactable = true;
	}
		

}
