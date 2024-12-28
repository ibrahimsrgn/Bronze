using UnityEngine;

public class FootStepScript : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public AudioClip[] Clips;
    void Start()
    {
    }

    // Update is called once per frame
    private void Step()
    {
        int Randomizer = Random.Range(0, Clips.Length);
        m_AudioSource.PlayOneShot(Clips[Randomizer]);
    }
}
