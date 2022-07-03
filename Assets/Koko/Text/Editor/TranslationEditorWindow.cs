using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Koko.LocalizationSystem;

namespace Koko {

	public class TranslationEditorWindow : EditorWindow {

		private List<JsonObjectData> _Dictionary = new List<JsonObjectData>();

		private Vector2 _Scroll;

		private string _Key = "";
		private string _Value = "";

		[MenuItem("Koko/Text/TranslationEditor")]
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

			GetInsertItem();

			using (new GUILayout.VerticalScope()) {
				using var scope = new GUILayout.ScrollViewScope(_Scroll);
				LoadItems();
				_Scroll = scope.scrollPosition;
			}
		}

		private void LoadItems() {
			for (int i = 0; i < _Dictionary.Count; i++) {

				using (new GUILayout.HorizontalScope("Box")) {

					EditorStyles.label.wordWrap = true;
					var key = _Dictionary[i].Key;
					GUILayout.Label(key, EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));

					using (new GUILayout.VerticalScope()) {
						for (int j = 0; j < LanguageSystem.GetLanguages().Length; j++) {

							var index = LanguageSystem.GetLanguageKeyByIndex(j);

							var value = GetLocalizedValue(_Dictionary[i].Key, index);

							using (new GUILayout.HorizontalScope()) {
								GUILayout.Label(index, EditorStyles.label, GUILayout.ExpandWidth(false), GUILayout.Width(30));
								var newValue = EditorGUILayout.TextField(value, GUILayout.ExpandWidth(true));
								if (newValue != value) {
									Replace(_Dictionary[i].Key, newValue, index);
									AssetDatabase.Refresh();
									LocalizationSystem.Init();
								}
							}
						}
					}

				}

			}
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

			if (GUILayout.Button("Add Or Update Language")) {

				if (_Key.Length > 5) {
					EditorUtility.DisplayDialog("Error!", "Key Length to high!", "OK!");
				} else {
					LanguageSystem.AddLanguage(_Key, _Value);
					AssetDatabase.Refresh();
					LocalizationSystem.Init();
				}
			}

			EditorGUILayout.EndVertical();
		}
	}
}