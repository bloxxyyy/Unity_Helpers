using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Koko {
	public class JSONLoader : IFileLoader {

		public TextAsset File { get; set; }
		public string FileName { get; set; }

		public List<JsonObjectData> Data = new List<JsonObjectData>();

		public void Load() {
			File = Resources.Load<TextAsset>("Koko/" + FileName);
		}

		public List<JsonObjectData> GetLanguageData() {
			Data.Clear();
			var JsonObject = JObject.Parse(File.text);

			foreach (var obj in JsonObject.Properties()) {
				var data = new JsonObjectData();
				data.Key = obj.Name;

				int i = 0;
				foreach (JProperty val in obj.Value.Children()) {
					data.GetValue<JsonListValue>().Value.Add(new KeyValuePair<string, string>(val.Name, val.Value.ToString()));
					i++;
				}

				Data.Add(data);
			}

			return Data;
		}

#if UNITY_EDITOR
		public void Add(int index, JsonObjectData languageData) {
			if (FileName == "languages") {
				if (index != -1)
					Data[0].GetValue<JsonListValue>().Value.RemoveAt(index);

				var key = languageData.GetValue<JsonListValue>().Value[0].Key;
				var value = languageData.GetValue<JsonListValue>().Value[0].Value;

				Data[0].GetValue<JsonListValue>().Value.Add(new KeyValuePair<string, string>(key, value));
				SaveToJson();
				Load();
				return;
			}

			if (index != -1)
				Data.RemoveAt(index);
			Data.Add(languageData);
			SaveToJson();
			Load();
		}

		public void Edit(string key, List<KeyValuePair<string, string>> newValue) {
			for (int i = 0; i < Data.Count; i++) {
				if (Data[i].Key == key)
					Data[i].GetValue<JsonListValue>().Value = newValue;
			}
			SaveToJson();
			Load();
		}

		public void Remove(string key) {
			for (int i = 0; i < Data.Count; i++) {
				if (Data[i].Key == key)
					Data.RemoveAt(i);
			}
			SaveToJson();
			Load();
		}

		public void SaveToJson() {
			var newData = "{ ";

			int i = 0;
			int last = Data.Count - 1;
			foreach (var obj in Data) {
				newData += "\"" + obj.Key + "\": { ";

				int j = 0;
				int last2 = obj.GetValue<JsonListValue>().Value.Count - 1;
				foreach (var val in obj.GetValue<JsonListValue>().Value) {
					newData += "\"" + val.Key + "\":";
					newData += "\"" + val.Value + "\"";

					if (j != last2)
						newData += ",";
					j++;
				}

				newData += " }";
				if (i != last)
					newData += ",";
				i++;
			}
			newData += " }";

			System.IO.File.WriteAllText("Assets/Resources/Koko/" + FileName + ".json", newData);
		}
#endif
	}
}