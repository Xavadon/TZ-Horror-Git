using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;

    public void Construct()
    {
        DisableText();
    }

    public void UpdateText(string text)
    {
        _textField.text = text;
    }

    public void DisableText()
    {
        _textField.text = "";
    }
}
