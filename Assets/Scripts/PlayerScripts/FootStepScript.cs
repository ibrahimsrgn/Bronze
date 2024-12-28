using UnityEngine;

public class FootStepScript : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public AudioClip[] Clips;
    public PlayerData PlayerData;
    void Start()
    {
    }

    // Update is called once per frame
    private void Step()
    {
        if (PlayerData.MoveInput.x == 0 && PlayerData.MoveInput.y == 0)
        {
            return;
        }
        else
        {
            int Randomizer = Random.Range(0, Clips.Length);
            m_AudioSource.PlayOneShot(Clips[Randomizer]);
        }
    }
}
