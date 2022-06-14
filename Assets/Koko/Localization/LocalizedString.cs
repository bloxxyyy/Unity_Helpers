[System.Serializable]
public struct LocalizedString {
	public string key;

	public LocalizedString(string key) => this.key = key;
	public string Value => LocalizationSystem.GetLocalizedValue(key);
	public static implicit operator LocalizedString(string key) => new LocalizedString(key);
}
