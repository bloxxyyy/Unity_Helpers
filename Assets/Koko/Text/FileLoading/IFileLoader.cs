using System.Collections.Generic;
using UnityEngine;

public interface IFileLoader {
	List<JsonObjectData> GetLanguageData();
	TextAsset File { get; set; }

	string FileName { get; set; }

	void Load();
	void Remove(string key);
	void Edit(string key, List<KeyValuePair<string, string>> newValue);
	void Add(int index, JsonObjectData languageData);
}