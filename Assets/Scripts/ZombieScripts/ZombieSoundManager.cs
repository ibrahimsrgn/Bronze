using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource zombieSoundSourceMouth;
    [SerializeField] private AudioSource zombieSoundSourceFoot;
    [SerializeField] private AudioSource zombieSoundSourceHand;
    [SerializeField] private AudioClip[] step;
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip[] die;
    [SerializeField] private AudioClip[] idle;
    [SerializeField] private AudioClip[] chase;
    [SerializeField] private AudioClip[] hit;
    [SerializeField] private AudioClip spawn;

    //Player equip sound -- GearAll_7

    public void PlayStep()
    {
        if(zombieSoundSourceFoot.isPlaying) return;
        int random = Random.Range(0, step.Length);
        zombieSoundSourceFoot.PlayOneShot(step[random]);
        Debug.Log(step[random].name);
    } //Forest_ground_step1
    public void PlayAttack()
    {
        if(zombieSoundSourceHand.isPlaying) return;
        int random = Random.Range(0, attack.Length);
        zombieSoundSourceHand.PlayOneShot(attack[random]);
    }
    public void PlayDie()
    {
        zombieSoundSourceMouth.Stop();
        zombieSoundSourceHand.Stop();
        zombieSoundSourceFoot.Stop();
        int random = Random.Range(0, die.Length);
        zombieSoundSourceMouth.PlayOneShot(die[random]);
    }
    public void PlayIdle()
    {
        if(zombieSoundSourceMouth.isPlaying) return;
        int random = Random.Range(0, idle.Length);
        zombieSoundSourceMouth.PlayOneShot(idle[random]);
    }
    public void PlayChase()
    {
        if(zombieSoundSourceMouth.isPlaying) return;
        int random = Random.Range(0, chase.Length);
        zombieSoundSourceMouth.PlayOneShot(chase[random]);
    }
    public void PlayHit()
    {
        zombieSoundSourceMouth.Stop();
        if(zombieSoundSourceMouth.isPlaying) return;
        int random = Random.Range(0, hit.Length);
        zombieSoundSourceMouth.PlayOneShot(hit[random]);
    }
    public void PlaySpawn()
    {
 
        if(zombieSoundSourceMouth.isPlaying) return;
        zombieSoundSourceMouth.time = 4.8f;
        zombieSoundSourceMouth.PlayOneShot(spawn);
    } //GhostChild_Pro_1 start 4.8s

}
