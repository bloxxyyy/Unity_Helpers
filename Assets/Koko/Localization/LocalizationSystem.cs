using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem {

	private static List<LanguageData> Data;

	public static bool isInit;
	public static IFileLoader loader = new JSONLoader();

	public static void Init() {
		loader.Load();
		UpdateDictionaries();
		isInit = true;
	}

	public static void UpdateDictionaries() {
		Data = loader.GetLanguageData();
	}

	public static string GetLocalizedValue(string key, Language language) {
		if (!isInit) Init();

		for (int i = 0; i < Data.Count; i++) {
			if (Data[i].Key == key) {

				foreach(var val in Data[i].Value) {
					if (language == Language.English && val.Key == "ENG") return val.Value;
					if (language == Language.Japanese && val.Key == "JP") return val.Value;
				}
			}
		}

		return "";
	}

	public static void AddOrReplace(string key, string value, Language language) {
		for (int i = 0; i < Data.Count; i++) {
			if (key == Data[i].Key) {

				var Found = false;
				for (int j = 0; j < Data[i].Value.Count; j++) {

					if (language == Language.English && Data[i].Value[j].Key == "ENG")
						Found = true;
					if (language == Language.Japanese && Data[i].Value[j].Key == "JP")
						Found = true;
				}

				if (Found) {
					Replace(key, value, language, i);
					return;
				}
			}
		}

		Add(key, value, language);
	}

	public static void Add(string key, string value, Language language) {
		if (loader == null) loader = new JSONLoader();
		loader.Load();

		LanguageData data = new LanguageData();
		data.Key = key;
		data.Value = new List<KeyValuePair<string, string>>();
		var index = -1;
		for (int i = 0; i < Data.Count; i++) {

			if (key == Data[i].Key) {
				data = Data[i];
				index = i;
			}
		}

		if (language == Language.English) {
			data.Value.Add(new KeyValuePair<string, string>("ENG", value));
		}

		if (language == Language.Japanese) {
			data.Value.Add(new KeyValuePair<string, string>("JP", value));
		}

		if (data.Value.Count < 1) {
			Debug.LogError("Language not found!");
			return;
		}

		loader.Add(index, data);
		loader.Load();
		UpdateDictionaries();
	}

	public static void Replace(string key, string value, Language language, int dataIndex) {
		if (loader == null) loader = new JSONLoader();
		loader.Load();

		var data = Data[dataIndex];

		KeyValuePair<string, string> keyVal = new KeyValuePair<string, string>();
		bool found = false;

		for (int i = 0; i < data.Value.Count; i++) {
			if (language == Language.English && data.Value[i].Key == "ENG") {
				keyVal = new KeyValuePair<string, string>("ENG", value);
				data.Value[i] = keyVal;
				found = true;
			}

			if (language == Language.Japanese && data.Value[i].Key == "JP") {
				keyVal = new KeyValuePair<string, string>("JP", value);
				data.Value[i] = keyVal;
				found = true;
			}
		}

		if (!found) {
			Debug.LogError("Couldn't Edit value!");
			return;
		}

		loader.Edit(key, data.Value);
		loader.Load();
		UpdateDictionaries();
	}

	public static List<LanguageData> GetDictionaryForEditor(Language language) {
		if (!isInit) Init();
		return Data;
	}

	public static void Remove(string key) {
		if (loader == null) loader = new JSONLoader();
		loader.Load();
		loader.Remove(key);
		loader.Load();
		UpdateDictionaries();
	}
}
