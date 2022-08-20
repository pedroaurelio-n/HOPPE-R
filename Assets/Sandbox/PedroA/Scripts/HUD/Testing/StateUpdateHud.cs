using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tortoise.HOPPER
{
    public class StateUpdateHud : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> textFields;

        private static List<TextMeshProUGUI> _textFields;
        private static List<string> states;

        private void Awake()
        {
            _textFields = textFields;
            states = new List<string>();
        }

        public static void UpdateStateList(string currentState)
        {
            states.Insert(0, currentState);

            if (states.Count > _textFields.Count)
            {
                states.RemoveAt(_textFields.Count);
            }

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i] != null)
                    _textFields[i].text = states[i].Remove(0, 6);
            }
        }
    }
}
