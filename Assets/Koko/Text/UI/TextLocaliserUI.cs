using TMPro;
using UnityEngine;

namespace Koko {

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocaliserUI : MonoBehaviour {

        private TextMeshProUGUI textfield;
        public LocalizedString localizedString;

        void Start() {
            textfield = GetComponent<TextMeshProUGUI>();
            textfield.text = localizedString.Value;
        }

    }
}
