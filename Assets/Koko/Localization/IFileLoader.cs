using System.Collections.Generic;
using UnityEngine;

public interface IFileLoader {
	List<LanguageData> GetLanguageData();
	TextAsset File { get; set; }

	void Load();
	void Remove(string key);
	void Edit(string key, List<KeyValuePair<string, string>> newValue);
	void Add(LanguageData languageData);
}