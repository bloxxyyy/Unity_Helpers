using UnityEditor;
using UnityEngine;

namespace Koko._2D {

	public class AddActorPopup : EditorWindow {
		public static void Init() {
			AddActorPopup window = CreateInstance<AddActorPopup>();
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
			window.ShowPopup();
		}

		private GUIStyle _ErrorLabelStyle;
		private string value = "";
		private string error = "";

		private void OnFocus() {
			if (_ErrorLabelStyle == null) {
				_ErrorLabelStyle = new GUIStyle(GUI.skin.label);
				_ErrorLabelStyle.normal.textColor = Color.red;
			}
		}

			void OnGUI() {
			EditorGUILayout.LabelField("Please insert Actor Key name...", EditorStyles.wordWrappedLabel);

			EditorStyles.textArea.wordWrap = true;
			value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.ExpandWidth(true));
			EditorGUILayout.LabelField(error, _ErrorLabelStyle);
			
			GUILayout.Space(60);
			using (new GUILayout.HorizontalScope()) {
				if (GUILayout.Button("Create!")) {
					if (value.Length > 3) {
						//if (ActorSystem.AddActor(value)) {
							Close();
						//} else {
						//	error = "Actor already exists!";
						//}
					} else {
						error = "Please insert a name with a higher value than 3!";
					}
				}
				GUILayout.Space(10);
				if (GUILayout.Button("Delete!")) {
					this.Close();
				}
			}
		}
	}	

	public class ActorWindow : EditorWindow {

		private Texture2D _Addtexture, _AddHoverTexture, _Removetexture, _RemoveHoverTexture;
		private GUIStyle _AddButtonStyle, _RemoveButtonStyle;

		[MenuItem("Koko/2D/ActorEditor")]
		public static void Init() {
			var window = (ActorWindow)GetWindow(typeof(ActorWindow));
			window.Show();
		}

		public void OnGUI() {
			NullChecks();
			
			GUILayout.Label("Actor Editor", EditorStyles.boldLabel);
			GUILayout.Space(10);

			using (new GUILayout.VerticalScope()) {
				AddCharacterGui();
				GUILayout.Space(10);
				CharacterSelector();
			}
		}

		private void AddCharacterGui() {
			using (new GUILayout.HorizontalScope("box")) {
				if (GUILayout.Button("", _AddButtonStyle, GUILayout.MaxWidth(30), GUILayout.MaxHeight(30), GUILayout.ExpandWidth(false))) {
					AddActorPopup.Init();
				}
				GUILayout.Label("Add Character", EditorStyles.largeLabel);
			}
		}

		private int _ActorSelectorIndex = 0;
		private string dialog = "Are you sure you want to delete this actor?";
		private void CharacterSelector() {
			using (new GUILayout.VerticalScope("box")) {
				using (new GUILayout.HorizontalScope()) {
					GUILayout.Label("Actor Selector", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
					var tempData = new string[2] { "Test", "Test2" };
					_ActorSelectorIndex = EditorGUILayout.Popup(_ActorSelectorIndex, tempData, GUILayout.ExpandWidth(true));

					if (GUILayout.Button("", _RemoveButtonStyle, GUILayout.MaxWidth(22), GUILayout.MaxHeight(22))) {
						if (EditorUtility.DisplayDialog("Delete Actor", dialog, "Delete", "Don't delete")) {
						}
					}
				}
			}
		}

		private void NullChecks() {
			if (_AddButtonStyle == null)
				_AddButtonStyle = new GUIStyle(GUI.skin.button);
			if (_Addtexture == null || _AddHoverTexture == null) {
				_Addtexture = (Texture2D)Resources.Load("Koko/add");
				_AddHoverTexture = (Texture2D)Resources.Load("Koko/add_Hover_Or_Click");
				_AddButtonStyle.normal.background = _Addtexture;
				_AddButtonStyle.hover.background = _AddHoverTexture;
			}
			if (_RemoveButtonStyle == null)
				_RemoveButtonStyle = new GUIStyle(GUI.skin.button);
			if (_Removetexture == null || _RemoveHoverTexture == null) {
				_Removetexture = (Texture2D)Resources.Load("Koko/remove");
				_RemoveHoverTexture = (Texture2D)Resources.Load("Koko/remove_hover");
				_RemoveButtonStyle.normal.background = _Removetexture;
				_RemoveButtonStyle.hover.background = _RemoveHoverTexture;
			}
		}

	}
}
