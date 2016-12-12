using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

[System.Serializable]
public class ClickGamesIndex {
	private List<ClickGameInfo> games;

	private ClickGamesIndex() {
		games = new List<ClickGameInfo>();
	}

	public static ClickGamesIndex LoadIndex(){
		string path = Common.INDEX_FILE;

		if(!File.Exists(path)) {
			Debug.Log("Index doesn't exist in path " + path + ". It shall be created.");
			return new ClickGamesIndex();
		}

		return (ClickGamesIndex) Common.Load(path);
	}

	public List<ClickGameInfo> GetGames() {
		return games;
	}

	public void SaveYourself(){
		Common.Save(Common.INDEX_FILE, this);
	}

	public void AddGame(ClickGameInfo gameInfo) {
		games.Add(gameInfo);
		SaveYourself();
	}

	public void EditGame(ClickGameInfo oldGame, ClickGameInfo newGame){
		int index = games.IndexOf(oldGame);
		games[index] = newGame;
		SaveYourself();
	}

	public void DeleteGame(ClickGameInfo game) {
		games.Remove(game);
		SaveYourself();
	}

	public ClickGameInfo GetGame(int index){
		return games[index];
	}
}