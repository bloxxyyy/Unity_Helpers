using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static LocalizationSystem;

public class TranslationEditorWindow : EditorWindow {

	private List<JsonObjectData> _Dictionary = new List<JsonObjectData>();

	private Vector2 _Scroll;

	[MenuItem("Window/Koko/Text/TranslationEditor")]
	public static void Init() {
		var window = (TranslationEditorWindow)GetWindow(typeof(TranslationEditorWindow));
		window.Show();
	}

	private void OnEnable() {
		_Dictionary = GetDictionaryForEditor();
		AssetDatabase.Refresh();
		LocalizationSystem.Init();
		_Dictionary = GetDictionaryForEditor();
	}

	public void OnGUI() {
		GUILayout.Label("Translation Editor", EditorStyles.boldLabel);
		GetResults();
	}

	private void GetResults() {
		EditorGUILayout.BeginVertical();
		_Scroll = EditorGUILayout.BeginScrollView(_Scroll);

		for (int i = 0; i < _Dictionary.Count; i++) {
			EditorGUILayout.BeginHorizontal("Box");

			EditorGUILayout.TextField(_Dictionary[i].Key, GUILayout.ExpandWidth(false), GUILayout.Width(160));
			EditorStyles.label.wordWrap = true;

			EditorGUILayout.BeginVertical();
			for (int j = 0; j < LanguageSystem.GetLanguages().Length; j++) {
				var value = GetLocalizedValue(_Dictionary[i].Key, LanguageSystem.GetLanguageKeyByIndex(j));
				EditorGUILayout.LabelField(value, GUILayout.ExpandWidth(true));
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
