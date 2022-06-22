using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static LocalizationSystem;

public class TranslationEditorWindow : EditorWindow {

	private List<LanguageData> _Dictionary = new List<LanguageData>();

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
			foreach (Language lang in (Language[])Language.GetValues(typeof(Language))) {
				var value = GetLocalizedValue(_Dictionary[i].Key, lang);
				EditorGUILayout.LabelField(value, GUILayout.ExpandWidth(true));
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
