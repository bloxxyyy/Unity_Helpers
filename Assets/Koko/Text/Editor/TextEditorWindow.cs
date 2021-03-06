using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static LocalizationSystem;

public class TextEditorWindow : EditorWindow {

	private string _Search = "";
	private Vector2 _Scroll;
	private List<JsonObjectData> _Dictionary = new List<JsonObjectData>();
	private string _Key = "";
	private string _Value = "";

	// Language variables
	private string oldLanguage = "";
	private int oldIndex = -1;
	private string[] curLanguages;
	private int langIndex = -1;
	// -------------------------

	[MenuItem("Window/Koko/Text/TextEditor")]
    public static void Init() {
        var window = (TextEditorWindow)GetWindow(typeof(TextEditorWindow));
        window.Show();
    }

	/// <summary>
	/// Because the translation window changes the languages.
	/// </summary>
	void OnFocus() => curLanguages = LanguageSystem.GetLanguages();

	private void OnEnable() {
		_Dictionary = GetDictionaryForEditor();
	}

	public void OnGUI() {
		GUILayout.Label("Text Editor", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("Box");

		SetLanguageTab();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Search: ", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));
        _Search = EditorGUILayout.TextField(_Search, EditorStyles.textField, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();

		GetInsertItem();

		if (_Search != null) GetSearchResults();
	}

	private void GetInsertItem() {

		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Key: ", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));
		_Key = EditorGUILayout.TextField(_Key, EditorStyles.textField, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Value: ", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));
		EditorStyles.textArea.wordWrap = true;
		_Value = EditorGUILayout.TextArea(_Value, EditorStyles.textArea, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Add Or Update")) {

			if (_Key.Length > 26) {
				EditorUtility.DisplayDialog("Error!", "Key Length to high!", "OK!");
			} else {
				AddOrReplace(_Key, _Value, LanguageSystem.CurrentLanguageKey);
				AssetDatabase.Refresh();
				LocalizationSystem.Init();
				_Dictionary = GetDictionaryForEditor();
			}
		}

		EditorGUILayout.EndVertical();
	}

	private void GetSearchResults() {
		EditorGUILayout.BeginVertical();
		_Scroll = EditorGUILayout.BeginScrollView(_Scroll);

		for (int i = 0; i < _Dictionary.Count; i++) {
			if (_Dictionary[i].Key.ToLower().Contains(_Search.ToLower())) {
				EditorGUILayout.BeginHorizontal("Box");

				DeleteButton(_Dictionary[i].Key);

				var key = _Dictionary[i].Key;

				EditorStyles.label.wordWrap = true;
				GUILayout.Label(key, EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(160));


				var value = GetLocalizedValue(_Dictionary[i].Key, LanguageSystem.CurrentLanguageKey);

				EditorGUILayout.LabelField(value, GUILayout.ExpandWidth(true));

				EditorGUILayout.EndHorizontal();
			}
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	private void DeleteButton(string key) {
		var closeIcon = (Texture)Resources.Load("Koko/close");
		var content = new GUIContent(closeIcon);
		if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20))) {
			if (EditorUtility.DisplayDialog("Remove key " + key + "?", "This will remove the element from the localization, are you sure?", "Do it")) {
				Remove(key);
				AssetDatabase.Refresh();
				LocalizationSystem.Init();
				_Dictionary = GetDictionaryForEditor();
			}
		}
	}

	/// <summary>
	/// Set the language information and update but only if the language changed.
	/// </summary>
	private void SetLanguageTab() {
		if (oldIndex == -1) {
			langIndex = LanguageSystem.GetIndexOfCurrentLanguage();
			curLanguages = LanguageSystem.GetLanguages();
		}

		var i = EditorGUILayout.Popup("Language", langIndex, curLanguages);
		if (i != oldIndex) {
			oldIndex = i;
			LanguageSystem.SetLanguageBasedOnIndex(i);
			langIndex = LanguageSystem.GetIndexOfCurrentLanguage();
			curLanguages = LanguageSystem.GetLanguages();
		}

		if (LanguageSystem.CurrentLanguageKey != oldLanguage) {
			oldLanguage = LanguageSystem.CurrentLanguageKey;
			AssetDatabase.Refresh();
			LocalizationSystem.Init();
		}
	}
}
