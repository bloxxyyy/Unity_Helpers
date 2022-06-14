using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CsvLoader {
	private TextAsset csvFile;
	private char _LineSeperator = '\n';
	private char _Surround = '"';
	private string[] _FieldSeperator = { "\",\"" };

	public void LoadCsv() {
		csvFile = Resources.Load<TextAsset>("Koko/localization");
	}

	public Dictionary<string, string> GetDictionaryValues(string attributeId) {
		var dictionary = new Dictionary<string, string>();
		var lines = csvFile.text.Split(_LineSeperator);

		int attributeIndex = -1;
		var headers = lines[0].Split(_FieldSeperator, StringSplitOptions.None);
		
		for (int i = 0; i < headers.Length; i++) {
			if (headers[i].Contains(attributeId)) {
				attributeIndex = i;
				break;
			}
		}

		var CsvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
		for (int i = 0; i < lines.Length; i++) {
			var line = lines[i];
			var fields = CsvParser.Split(line);
			for (int j = 0; j < fields.Length; j++) {
				fields[j] = fields[j].TrimStart(' ', _Surround);
				fields[j] = fields[j].TrimEnd(_Surround);
			}

			if (fields.Length > attributeIndex) {
				var key = fields[0];
				if (dictionary.ContainsKey(key)) continue; 
				var value = fields[attributeIndex];
				dictionary.Add(key, value);
			}
		}

		return dictionary;
	}

#if UNITY_EDITOR
	public void Add(string key, string value) {
		var appended = string.Format("\n\"{0}\",\"{1}\",\"\"", key, value);
		File.AppendAllText("Assets/Resources/Koko/localization.csv", appended);
		UnityEditor.AssetDatabase.Refresh();
	}

	public void Remove(string key) {
		var lines = csvFile.text.Split(_LineSeperator);
		var keys = new string[lines.Length];
		for (int i = 0; i < lines.Length; i++) {
			var line = lines[i];
			keys[i] = line.Split(_FieldSeperator, StringSplitOptions.None)[0];
		}

		var index = -1;

		for (int i = 0; i < keys.Length; i++) {
			if (keys[i].Contains(key)) {
				index = i;
				break;
			}
		}

		if (index > -1) {
			var newlines = lines.Where(w => w != lines[index]).ToArray();
			string replaces = string.Join(_LineSeperator.ToString(), newlines);
			File.WriteAllText("Assets/Resources/Koko/localization.csv", replaces);
		}
	}

	public void Edit(string key, string value) {
		Remove(key);
		Add(key, value);
	}
#endif
}
