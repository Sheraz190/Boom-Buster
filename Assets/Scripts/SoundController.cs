using UnityEngine;

public class SoundController : MonoBehaviour
{
    #region Variables
    public static SoundController Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shruikenClip;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip attackSound;
    #endregion

    public void TurnShruikenSound()
    {
        audioSource.PlayOneShot(shruikenClip);
    }

    public void TurnAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

}
