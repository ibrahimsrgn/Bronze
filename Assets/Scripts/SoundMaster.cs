using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;


public enum SoundType
{
    PlayerHurt,
    PlayerDeath,
    PlayerJump,
    PlayerLand,
    PlayerFootstep,
    ZombieHurt,
    ZombieDeath,
    ZombieAttack,
    ZombieIdle,
    ZombieAlert,
    ZombieChase,
    ZombieRoar,
    ZombieSpawn,
    GunShot,
    GunReload,
    GunEmpty,
    GunPickup,
    GunDrop,
    GunEquip,
    GunUnequip,
    GunSilencer,
    GunUnsilencer,
    GunScope,
    GunUnscope,
    GunFireMode,
};

[RequireComponent(typeof(AudioSource))]
public class SoundMaster : MonoBehaviour
{
    [SerializeField] private SoundList[] soundLists;
    private static SoundMaster instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
    {
        SoundList soundList = instance.soundLists[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundLists, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            soundLists[i].name = names[i];
        }
#endif
    }


    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioClip[] sounds;
    }
}
