using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewDialogueSystem
{
    [RequireComponent(typeof(UIFadeAnimator))]
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private float _typeDelay = 0.05f;

        private UIFadeAnimator _uiFadeAnimator;
        private string _dialogueKey;
        private string _currentSentence;
        private Queue<string> _sentences = new();
        private bool _isTyping;
        private bool _isAnimating;
        private bool _isFirstSentence;

        public event Action<string> OnDialogueEnd;

        private void Awake()
        {
            _uiFadeAnimator = GetComponent<UIFadeAnimator>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _sentences.Count >= 0)
            {
                TypeNextSentence();
            }
        }

        public void TypeDialogue(string dialogueKey, List<string> sentences)
        {
            Debug.Log(dialogueKey);

            _dialogueKey = dialogueKey;
            _isFirstSentence = true;

            foreach (string sentence in sentences)
            {
                _sentences.Enqueue(sentence);
            }

            TypeNextSentence();
        }

        private void TypeNextSentence()
        {
            if (_isAnimating || _dialogueKey == "")
            {
                return;
            }

            if (_isTyping)
            {
                StopAllCoroutines();
                DisplayText(_currentSentence);
                return;
            }

            if (_sentences.Count == 0)
            {
                StartCoroutine(DialogueEnd());
                return;
            }

            _currentSentence = _sentences.Dequeue();
            StartCoroutine(DisplayTextWithDelay(_currentSentence));
        }

        private void DisplayText(string text)
        {
            _textField.text = text;
            _isTyping = false;
        }

        private IEnumerator DisplayTextWithDelay(string text)
        {
            _isTyping = true;

            if (!_isFirstSentence)
            {
                yield return ToggleAnimator(false);
            }
            else
            {
                _isFirstSentence = false;
            }

            _textField.text = "";
            yield return ToggleAnimator(true);

            foreach (char letter in text)
            {
                _textField.text += letter;
                yield return new WaitForSeconds(_typeDelay);
            }

            _isTyping = false;
        }

        private IEnumerator ToggleAnimator(bool state)
        {
            _uiFadeAnimator.ToggleMenu(state);
            _isAnimating = true;

            while (_uiFadeAnimator.IsActive != state)
            {
                yield return null;
            }

            _isAnimating = false;
        }

        private IEnumerator DialogueEnd()
        {
            yield return ToggleAnimator(false);

            _textField.text = "";

            string dialogueKey = _dialogueKey;
            _dialogueKey = "";
            OnDialogueEnd?.Invoke(dialogueKey);
        }
    }
}