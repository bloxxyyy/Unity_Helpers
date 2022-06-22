using System.Collections.Generic;

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

	public static void Add(string key, string value, Language language) {
		throw new System.Exception("Nyo!");
		if (loader == null) loader = new JSONLoader();
		loader.Load();
		//loader.Add(key, value, language);
		loader.Load();
		UpdateDictionaries();
	}

	public static void Replace(string key, string value, Language language) {
		throw new System.Exception("Nyo!");

		if (loader == null) loader = new JSONLoader();
		loader.Load();
		//loader.Edit(key, value, language);
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
