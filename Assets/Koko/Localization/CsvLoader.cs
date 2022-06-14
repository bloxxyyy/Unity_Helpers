using System;
using System.Collections.Generic;
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
}
