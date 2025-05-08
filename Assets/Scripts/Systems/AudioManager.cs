using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;

    [SerializeField] AudioSource chipWalkingSound;
    [SerializeField] AudioSource chipJumpingSound;
    [SerializeField] AudioSource runningSound;
    [SerializeField] AudioSource jumpingSound;
    [SerializeField] AudioSource dashSound;
    [SerializeField] AudioSource axeSound;
    [SerializeField] AudioSource swordSound;
    [SerializeField] AudioSource monsterSound;
    [SerializeField] AudioSource clickSound;
    [SerializeField] AudioSource ambienceSound;


    private void Awake()
    {
        instance = this;
    }

    public void ChipWalkingSound()
    {
        chipWalkingSound.Play();
    }

    public void ChipWalkingSoundStop()
    {
        chipWalkingSound.Stop();
    }

    public void ChipJumpingSound()
    {
        chipJumpingSound.Play();
    }

    public void RunningSound()
    {
        runningSound.Play();
    }
    public void RunningSoundStop()
    {
        runningSound.Stop();
    }
    public void JumpSound()
    {
        jumpingSound.Play();
    }
    public void DashSound()
    {
        dashSound.Play();
    }
    public void AxeSound()
    {
        axeSound.Play();
    }
    public void SwordSound()
    {
        swordSound.Play();
    }
    public void MonsterSound()
    {
        monsterSound.Play();

    }
    public void ClickSound()
    {
        clickSound.Play();
    }
    public void AmbienceSound()
    {
        ambienceSound.Play();
    }
}
