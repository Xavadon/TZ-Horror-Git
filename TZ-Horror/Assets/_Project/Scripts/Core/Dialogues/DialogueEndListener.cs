using UnityEngine;

namespace NewDialogueSystem
{
    public class DialogueEndListener : MonoBehaviour
    {
        private void OnEnable()
        {
            DialogueSystem.OnDialogueFinished += StartNextDialogue;
        }

        private void OnDisable()
        {
            DialogueSystem.OnDialogueFinished -= StartNextDialogue;
        }

        private void StartNextDialogue(string key)
        {
            if (key == "greeting")
            {
                DialogueSystem.StartDialogue("fairy_tale");
            }
        }
    }
}