[System.Serializable]
public struct LocalizedString {
	public string key;
	public Language language;

	public LocalizedString(string key, Language language) {
		this.key = key;
		this.language = language;
	}

	public string Value => LocalizationSystem.GetLocalizedValue(key, Language.English);
}
