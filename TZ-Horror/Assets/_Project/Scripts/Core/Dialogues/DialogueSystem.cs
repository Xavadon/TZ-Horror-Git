using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewDialogueSystem
{
    public static class DialogueSystem
    {
        private static TextAsset _dialogueFile;
        private static DialogueBox _dialogueBox;
        private static string language = "En"; // Выбранный язык (например, "Ru" или "En").

        private static Dictionary<string, List<string>> _translationDictionary = new Dictionary<string, List<string>>();

        public static event Action<string, Transform, float> OnDialogueStart;
        public static event Action<string> OnDialogueFinished;

        public static void Construct(TextAsset dialogues, DialogueBox dialogueBox)
        {
            _dialogueFile = dialogues;
            _dialogueBox = dialogueBox;

            LoadTranslations();
        }

        private static void LoadTranslations()
        {
            TranslationData translationData = JsonUtility.FromJson<TranslationData>(_dialogueFile.text);
            foreach (var translation in translationData.translations)
            {
                List<string> texts = GetTextsForLanguage(translation);
                _translationDictionary[translation.key] = texts;
            }
        }

        private static List<string> GetTextsForLanguage(Translation translation)
        {
            if (language == "Ru" && translation.textsRu != null)
            {
                return translation.textsRu;
            }
            else if (language == "En" && translation.textsEn != null)
            {
                return translation.textsEn;
            }

            return new List<string> { "Translation Missing" };
        }

        public static void StartDialogue(string key, Transform speakerTransform = null, float focusTime = 0.75f)
        {
            if (_translationDictionary.TryGetValue(key, out List<string> sentences))
            {
                _dialogueBox.TypeDialogue(key, sentences);
                _dialogueBox.OnDialogueEnd += DialogueEnd;

                OnDialogueStart?.Invoke(key, speakerTransform, focusTime);
            }
            else
            {
                Debug.Log("Key not found.");
            }
        }

        private static void DialogueEnd(string key)
        {
            Debug.Log($"Dialogue Finished: {key}");
            OnDialogueFinished?.Invoke(key);
            _dialogueBox.OnDialogueEnd -= DialogueEnd;
        }
    }

    [System.Serializable]
    public class Translation
    {
        public string key;
        public List<string> textsRu;
        public List<string> textsEn;
    }

    [System.Serializable]
    public class TranslationData
    {
        public List<Translation> translations = new List<Translation>();
    }
}