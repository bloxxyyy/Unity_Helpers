using System.Collections.Generic;

public class JsonObjectData {
	public string Key { get; set; }

	private JsonValue _Value;

	public TValue GetValue<TValue>() where TValue : JsonValue, new() {
		if (_Value == null) _Value = new TValue();
		return (TValue)_Value;
	}
}

public class JsonValue {
	private object value;
}

public class JsonListValue : JsonValue {
	public List<KeyValuePair<string, string>> Value = new List<KeyValuePair<string, string>>();
}

public class JsonStringValue : JsonValue {
	public string value;
}