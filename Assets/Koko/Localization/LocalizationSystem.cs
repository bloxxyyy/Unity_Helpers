using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem {
	public enum Language {
		English, Japanese, Dutch
	}

	public static Language language = Language.English;

	private static Dictionary<string, string> localisedEN;
	private static Dictionary<string, string> localisedJP;
	private static Dictionary<string, string> localisedNL;

	public static bool isInit;

	public static void init() {
		var csvLoader = new CsvLoader();
		csvLoader.LoadCsv();

		localisedEN = csvLoader.GetDictionaryValues("en");
		localisedJP = csvLoader.GetDictionaryValues("jp");
		localisedNL = csvLoader.GetDictionaryValues("nl");

		isInit = true;
	}

	public static string GetLocalizedValue(string key) {
		if (!isInit) init();

		string value = key;

		switch (language) {
			case Language.English:
				localisedEN.TryGetValue(key, out value);
				break;
			case Language.Japanese:
				localisedJP.TryGetValue(key, out value);
				break;
			case Language.Dutch:
				localisedNL.TryGetValue(key, out value);
				break;
			default:
				break;
		}

		return value;
	}
}
