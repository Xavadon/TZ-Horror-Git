using UnityEngine;

public class StepSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float stepInterval = 0.5f;

    private CharacterController _characterController;
    private float _nextStepTime;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_characterController == null) return;
        if (!_characterController.isGrounded) return;

        Vector3 velocity = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z);
        if (velocity.magnitude > 0.3f)
        {
            if (Time.time >= _nextStepTime)
            {
                PlayStepSound();
                _nextStepTime = Time.time + stepInterval;
            }
        }
    }

    private void PlayStepSound()
    {
        if (audioSource != null && stepClips != null && stepClips.Length > 0)
        {
            int index = Random.Range(0, stepClips.Length);
            audioSource.PlayOneShot(stepClips[index]);
        }
    }
}
