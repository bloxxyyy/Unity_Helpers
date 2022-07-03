using System.Collections.Generic;
using UnityEngine;

namespace Koko {
	public interface IFileLoader {
		List<JsonObjectData> GetLanguageData();
		TextAsset File { get; set; }

		string FileName { get; set; }

		void Load();
		void Remove(string key);

		// TODO List<KeyValuePair<string, string>> isnt correct, GetValue<JsonListValue> should determain this
		void Edit(string key, List<KeyValuePair<string, string>> newValue);
		void Add(int index, JsonObjectData languageData);
	}
}