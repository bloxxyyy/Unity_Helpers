using System.Collections.Generic;

public class JsonObjectData{
	public string Key { get; set; }
	public List<KeyValuePair<string, string>> Value { get; set; } = new List<KeyValuePair<string, string>>();
}
