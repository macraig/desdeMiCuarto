using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class Common {
	public static string GAME_NAME = "Click";
	public static string INDEX_NAME = "index.gd";
	public static string INDEX_PLUS_NAME = "index_plus.gd";
	public static string QUESTIONS_FILE = "questions.gd";
	public static string INDEX_FILE = GetBaseDir() + "/" + INDEX_NAME;
	public static string INDEX_PLUS_FILE = GetBaseDir() + "/" + INDEX_PLUS_NAME;
	public static string EDIT_SUFFIX = "__edit";

	public static int TITLE_MAX_CHARS = 30;

	public static Color CORRECT_COLOR = new Color32(185, 219, 62, 255);
	public static Color WRONG_COLOR = new Color32(255, 68, 59, 255);

	private Common() { }

	public static string GetBaseDir(){
		string s = Common.DocumentsPath() + "/" + GAME_NAME;
		if(!Directory.Exists(s)) Directory.CreateDirectory(s);
		return s;
	}

	public static void Save(string path, object obj){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(path);
		bf.Serialize(file, obj);
		file.Close();
	}

	public static object Load(string path) {
		if(File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			object o = bf.Deserialize(file);
			file.Close();
			return o;
		}
		return null;
	}

	private static string DesktopPath() {
		return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
	}

	public static string DocumentsPath() {
		return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	}

	public static Sprite LoadImage(string path){
		string pathPrefix = @"file://";

		WWW www = new WWW(pathPrefix + path);
		Texture2D texTmp = new Texture2D(1024, 1024, path.EndsWith("png") ? TextureFormat.DXT5 : TextureFormat.DXT1, false);
		//LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5
		www.LoadImageIntoTexture(texTmp);
		return Sprite.Create(texTmp, new Rect(0, 0, texTmp.width, texTmp.height), new Vector2(0.5f, 0.5f));
	}

	public static AudioClip LoadSound(string path){
		string pathPrefix = @"file://";

		WWW www = new WWW(pathPrefix + path);
		AudioClip a;

		if(path.EndsWith("wav")){
			a = www.audioClip;

			while(a.loadState != AudioDataLoadState.Loaded){}
		} else {
			/*#if UNITY_STANDALONE

			while(!www.isDone){ }
			a = NAudioPlayer.FromMp3Data(www.bytes);

			#else*/

			a = www.audioClip;

			while(a.loadState != AudioDataLoadState.Loaded){}
		}

		return a;
	}
}