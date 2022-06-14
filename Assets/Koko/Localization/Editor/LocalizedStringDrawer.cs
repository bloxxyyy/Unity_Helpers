using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer {
    private bool _Dropdown;
    private float _Height;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		if (_Dropdown) return _Height + 25;
		return 20;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		position.width -= 34;
		position.height = 18;

		var valuerect = new Rect(position);
		valuerect.x += 15;
		valuerect.width -= 15;

		var foldButtonRect = new Rect(position);
		valuerect.width = 15;

		_Dropdown = EditorGUI.Foldout(foldButtonRect, _Dropdown, "");
		position.x += 15;
		position.width -= 15;

		var key = property.FindPropertyRelative("key");
		key.stringValue = EditorGUI.TextField(position, key.stringValue);

		position.x += position.width + 2;
		position.width = 17;
		position.height = 17;

		var searchIcon = (Texture)Resources.Load("Koko/search");
		var searchContent = new GUIContent(searchIcon);

		if (GUI.Button(position, searchContent)) {
			TextLocalizerSearchWindow.Open();
		}

		position.x += position.width + 2;
		var storeIcon = (Texture)Resources.Load("Koko/save");
		var storeContent = new GUIContent(storeIcon);

		if (GUI.Button(position, storeContent)) {
			TextLocalizerEditorWindow.Open(key.stringValue);
		}

		if (_Dropdown) {
			var value = LocalizationSystem.GetLocalizedValue(key.stringValue);
			var style = GUI.skin.box;
			_Height = style.CalcHeight(new GUIContent(value), valuerect.width);

			valuerect.height = _Height;
			valuerect.y += 21;

			EditorGUI.LabelField(valuerect, value, EditorStyles.wordWrappedLabel);
		}

		EditorGUI.EndProperty();
	}
}
