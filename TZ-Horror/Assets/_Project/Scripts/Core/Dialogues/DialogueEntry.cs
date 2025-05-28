using UnityEngine;

namespace NewDialogueSystem
{
    public class DialogueEntry : MonoBehaviour
    {
        [SerializeField] private TextAsset _dialogues;
        [SerializeField] private DialogueBox _dialogueBox;

        private void Start()
        {
            DialogueSystem.Construct(_dialogues, _dialogueBox);
            //DialogueSystem.StartDialogue("greeting");
        }
    }
}