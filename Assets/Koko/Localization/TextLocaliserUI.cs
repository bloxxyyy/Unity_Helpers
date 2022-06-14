using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour {

    private TextMeshProUGUI textfield;
    public LocalizedString localizedString;

    void Start() {
        textfield = GetComponent<TextMeshProUGUI>();
        textfield.text = localizedString.Value;
    }

}
