using UnityEngine;

public class SoundController : MonoBehaviour
{
    #region Variables
    public static SoundController Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shruikenClip;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip buttonClick;
    #endregion

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
    }

    public void TurnShruikenSound()
    {
        audioSource.PlayOneShot(shruikenClip);
    }

    public void TurnOnAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    public void TurnOnJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void GeneralButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }
}
