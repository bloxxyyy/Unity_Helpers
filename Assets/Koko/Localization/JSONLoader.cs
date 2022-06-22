using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class JSONLoader : IFileLoader {

	public TextAsset File { get; set; }
	public List<LanguageData> Data = new List<LanguageData>();

	public void Load() {
		File = Resources.Load<TextAsset>("Koko/localization");
	}

	public List<LanguageData> GetLanguageData() {
		Data.Clear();
		var JsonObject = JObject.Parse(File.text);

		foreach (var obj in JsonObject.Properties()) {
			var data = new LanguageData();
			data.Key = obj.Name;

			int i = 0;
			foreach (JProperty val in obj.Value.Children()) {
				data.Value.Add(new KeyValuePair<string, string>(val.Name, val.Value.ToString()));
				i++;
			}
			
			Data.Add(data);
		}

		return Data;
	}

#if UNITY_EDITOR
	public void Add(LanguageData languageData) {
		Data.Add(languageData);
		SaveToJson();
		Load();
	}

	public void Edit(string key, List<KeyValuePair<string, string>> newValue) {
		for (int i = 0; i < Data.Count; i++) {
			if (Data[i].Key == key) Data[i].Value = newValue;
		}
		SaveToJson();
		Load();
	}

	public void Remove(string key) {
		for (int i = 0; i < Data.Count; i++) {
			if (Data[i].Key == key) Data.RemoveAt(i);
		}
		SaveToJson();
		Load();
	}

	public void SaveToJson() {
		var newData = "[ \"";

		int i = 0;
		int last = Data.Count - 1;
		foreach (var obj in Data) {
			newData += obj.Key + "\": { ";

			int j = 0;
			int last2 = obj.Value.Count - 1;
			foreach (var val in obj.Value) {
				newData += "\"" + val.Key + "\":";
				newData += "\"" + val.Value + "\"";

				if (j != last2) newData += ",";
				j++;
			}

			newData += " }";
			if (i != last) newData += ",";
			i++;
		}
		newData += " }";

		System.IO.File.WriteAllText("Assets/Resources/Koko/localization.csv", newData);
	}
#endif
}