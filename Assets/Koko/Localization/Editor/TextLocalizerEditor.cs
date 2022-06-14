using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextLocalizerEditorWindow : EditorWindow {

	public string key;
	public string value;

	public static void Open(string key) {
		var window = new TextLocalizerEditorWindow();
		window.titleContent = new GUIContent("Localizer Window");
		window.ShowUtility();
		window.key = key;
	}

	public void OnGUI() {
		key = EditorGUILayout.TextField("Key: ", key);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Value: ", GUILayout.MaxWidth(50));

		EditorStyles.textArea.wordWrap = true;
		value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Add")) {
			if (LocalizationSystem.GetLocalizedValue(key) != string.Empty) LocalizationSystem.Replace(key, value);
			else LocalizationSystem.Add(key, value);
		}

		minSize = new Vector2(460, 250);
		maxSize = minSize;
	}
}

public class TextLocalizerSearchWindow : EditorWindow {

	public string value;
	public Vector2 scroll;
	public Dictionary<string, string> dictionary;

	public static void Open() {
		var window = new TextLocalizerSearchWindow();
		window.titleContent = new GUIContent("Localizer Search");

		var mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
		var rect = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
		window.ShowAsDropDown(rect, new Vector2(500, 300));
	}

	private void OnEnable() {
		dictionary = LocalizationSystem.GetDictionaryForEditor();
	}

	private void OnGUI() {
		EditorGUILayout.BeginHorizontal("Box");
		EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
		value = EditorGUILayout.TextField(value);
		EditorGUILayout.EndHorizontal();

		GetSearchResults();
	}

	private void GetSearchResults() {
		if (value == null) return;
		EditorGUILayout.BeginVertical();
		scroll = EditorGUILayout.BeginScrollView(scroll);
		foreach (var element in dictionary) {
			if (element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower())) {
				EditorGUILayout.BeginHorizontal("Box");
				var closeIcon = (Texture)Resources.Load("Koko/close");
				var content = new GUIContent(closeIcon);
				if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20))) {
					if (EditorUtility.DisplayDialog("Remove key " + element.Key + "?", "This will remove the element from the localization, are you sure?", "Do it")) {
						LocalizationSystem.Remove(element.Key);
						AssetDatabase.Refresh();
						LocalizationSystem.Init();
						dictionary = LocalizationSystem.GetDictionaryForEditor();

					}
				}

				EditorGUILayout.TextField(element.Key);
				EditorGUILayout.LabelField(element.Value);
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
