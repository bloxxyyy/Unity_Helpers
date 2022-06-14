using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour {

    private TextMeshProUGUI textfield;
    public string key;

    void Start() {
        textfield = GetComponent<TextMeshProUGUI>();
        string value = LocalizationSystem.GetLocalizedValue(key);
        textfield.text = value;
    }

}
