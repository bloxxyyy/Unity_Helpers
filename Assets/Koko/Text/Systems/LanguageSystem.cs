using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koko {

	/// <summary>
	/// To add more languages. This has nothing to do with translations. just adding a language to the system.
	/// </summary>
	public class LanguageSystem {

		private static List<JsonObjectData> Languages;

		public static string CurrentLanguageKey;

		public static bool isInit;
		public static IFileLoader loader = new JSONLoader();

		public static void Init() {
			CurrentLanguageKey = "Eng"; // todo Nyo!
			loader.FileName = "languages";
			loader.Load();
			UpdateDictionaries();
			isInit = true;
		}

		public static void UpdateDictionaries() {
			Languages = loader.GetLanguageData();
		}

		public static string GetValLanguageByKey(JsonObjectData obj, string langKey) {
			for (int i = 0; i < obj.GetValue<JsonListValue>().Value.Count; i++) {
				if (obj.GetValue<JsonListValue>().Value[i].Key == langKey)
					return obj.GetValue<JsonListValue>().Value[i].Value;
			}

			return "";
		}

		public static bool Get(string checkedLanguage) {
			if (!isInit)
				Init();
			// check if current language if not BOO!!!
			return false;
		}

		// return the current language key
		internal static string Give() {
			if (!isInit)
				Init();
			return CurrentLanguageKey;
		}

		public static int GetIndexOfCurrentLanguage() {
			if (!isInit)
				Init();
			var langData = Languages[0];

			for (int i = 0; i < langData.GetValue<JsonListValue>().Value.Count; i++) {
				if (langData.GetValue<JsonListValue>().Value[i].Key == CurrentLanguageKey)
					return i;
			}

			return 0;
		}

		public static void SetLanguageBasedOnIndex(int index) {
			if (!isInit)
				Init();
			var langData = Languages[0];

			var languages = new string[langData.GetValue<JsonListValue>().Value.Count];
			for (int i = 0; i < langData.GetValue<JsonListValue>().Value.Count; i++) {
				if (index == i)
					CurrentLanguageKey = langData.GetValue<JsonListValue>().Value[i].Key;
			}
		}

		public static string[] GetLanguages() {
			if (!isInit)
				Init();
			if (Languages == null)
				throw new Exception("No languages found! Big error! Abort!");

			UpdateDictionaries();

			var langData = Languages[0];

			var languages = new string[langData.GetValue<JsonListValue>().Value.Count];
			for (int i = 0; i < langData.GetValue<JsonListValue>().Value.Count; i++) {
				languages[i] = langData.GetValue<JsonListValue>().Value[i].Value;
			}
			return languages;
		}

		public static JsonObjectData GetObjectByKey(string languageKey) {
			if (!isInit)
				Init();
			foreach (var language in Languages[0].GetValue<JsonListValue>().Value) {
				if (language.Key == languageKey)
					return Languages[0];
			}

			return null;
		}

		public static string GetLanguageKeyByIndex(int j) {
			if (!isInit)
				Init();

			var langData = Languages[0].GetValue<JsonListValue>().Value;

			for (int i = 0; i < langData.Count; i++) {
				if (i == j)
					return langData[i].Key;
			}

			Debug.LogError("Couldn't find key by index!");
			return "";
		}

		public static void AddLanguage(string key, string value) {
			if (loader == null)
				loader = new JSONLoader();
			loader.Load();

			var data = new JsonObjectData();
			data.Key = key;
			data.GetValue<JsonListValue>().Value.Add(new KeyValuePair<string, string>(key, value));

			var index = -1;
			var Data = Languages[0].GetValue<JsonListValue>().Value;
			for (int i = 0; i < Data.Count; i++) {
				if (key == Data[i].Key)
					index = i;
			}

			loader.Add(index, data);
			loader.Load();
			UpdateDictionaries();
		}
	}
}
