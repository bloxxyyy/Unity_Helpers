using System;
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
	public static CsvLoader csvLoader = new CsvLoader();

	public static void Init() {
		csvLoader.LoadCsv();

		UpdateDictionaries();

		isInit = true;
	}

	public static void UpdateDictionaries() {
		localisedEN = csvLoader.GetDictionaryValues("en");
		localisedJP = csvLoader.GetDictionaryValues("jp");
		localisedNL = csvLoader.GetDictionaryValues("nl");
	}

	public static string GetLocalizedValue(string key) {
		if (!isInit) Init();

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

	public static void Add(string key, string value) {
		if(csvLoader == null) csvLoader = new CsvLoader();
		if (value.Contains("\"")) value.Replace('"', '\"');
		csvLoader.LoadCsv();
		csvLoader.Add(key, value);
		csvLoader.LoadCsv();
		UpdateDictionaries();
	}

	public static void Replace(string key, string value) {
		if(csvLoader == null) csvLoader = new CsvLoader();
		if (value.Contains("\"")) value.Replace('"', '\"');
		csvLoader.LoadCsv();
		csvLoader.Edit(key, value);
		csvLoader.LoadCsv();
		UpdateDictionaries();
	}

	public static Dictionary<string, string> GetDictionaryForEditor() {
		if (!isInit) Init();
		return localisedEN;
	}

	public static void Remove(string key) {
		if (csvLoader == null) csvLoader = new CsvLoader();
		csvLoader.LoadCsv();
		csvLoader.Remove(key);
		csvLoader.LoadCsv();
		UpdateDictionaries();
	}
}
