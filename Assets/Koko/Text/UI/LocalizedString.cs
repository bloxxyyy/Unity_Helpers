[System.Serializable]
public struct LocalizedString {
	public string key;
	public JsonObjectData language;

	public LocalizedString(string key, JsonObjectData language) {
		this.key = key;
		this.language = language;
	}

	public string Value => LocalizationSystem.GetLocalizedValue(key, language.Key);
}
