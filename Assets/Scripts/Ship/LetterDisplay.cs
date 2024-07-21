using System;
using TMPro;
using UnityEngine;

public class LetterDisplay : MonoBehaviour {
    TextMeshProUGUI letterText;
    char letter;

    void Awake() {
        letterText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update() {
        DisplayLetter();
    }

    void DisplayLetter() {
        Quaternion uprightRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        transform.rotation *= uprightRotation;
    }

    public void SetLetter(char l) {
        letter = l;
        letterText.text = l.ToString();
    }
}
