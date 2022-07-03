using System.Collections.Generic;
using UnityEngine;

namespace Koko {
	public class LocalizationSystem {

		private static List<JsonObjectData> Data;

		public static bool isInit;
		public static IFileLoader loader = new JSONLoader();

		public static void Init() {
			loader.FileName = "localization";
			loader.Load();
			UpdateDictionaries();
			isInit = true;
		}

		public static void UpdateDictionaries() {
			Data = loader.GetLanguageData();
		}

		public static string GetLocalizedValue(string key, string langKey) {
			if (!isInit)
				Init();

			for (int i = 0; i < Data.Count; i++) {
				if (Data[i].Key == key)
					return LanguageSystem.GetValLanguageByKey(Data[i], langKey);
			}

			Debug.LogError("Missing a key value!");
			return "";
		}

		public static void Replace(string key, string value, string languageKey) {
			if (loader == null)
				loader = new JSONLoader();
			loader.Load();

			var dataObj = new JsonObjectData();

			var index = -1;
			for (int i = 0; i < Data.Count; i++) {

				if (key == Data[i].Key) {
					dataObj.Key = Data[i].Key;
					index = i;

					var TranslatedObjectesInKey = Data[i].GetValue<JsonListValue>().Value;

					for (int j = 0; j < TranslatedObjectesInKey.Count; j++) {
						if (TranslatedObjectesInKey[j].Key != languageKey) {
							dataObj.GetValue<JsonListValue>().Value.Add(TranslatedObjectesInKey[j]);
						}
					}

					dataObj.GetValue<JsonListValue>().Value.Add(new KeyValuePair<string, string>(languageKey, value));

					Data[i] = dataObj;
					loader.Add(index, dataObj);
					loader.Load();
					UpdateDictionaries();
					break;
				}
			}

		}

		public static void AddOrReplace(string key, string value, string languageKey) {
			var obj = LanguageSystem.GetObjectByKey(languageKey);
			Add(key, value, obj);
		}

		public static void Add(string key, string value, JsonObjectData language) {
			if (loader == null)
				loader = new JSONLoader();
			loader.Load();

			var data = new JsonObjectData();
			data.Key = key;

			var index = -1;
			for (int i = 0; i < Data.Count; i++) {

				if (key == Data[i].Key) {
					data = Data[i];
					index = i;
					break;
				}
			}

			data.GetValue<JsonListValue>().Value.Add(new KeyValuePair<string, string>(LanguageSystem.Give().ToUpper(), value));

			loader.Add(index, data);
			loader.Load();
			UpdateDictionaries();
		}

		public static List<JsonObjectData> GetDictionaryForEditor() {
			if (!isInit)
				Init();
			return Data;
		}

		public static void Remove(string key) {
			if (loader == null)
				loader = new JSONLoader();
			loader.Load();
			loader.Remove(key);
			loader.Load();
			UpdateDictionaries();
		}
	}
}
