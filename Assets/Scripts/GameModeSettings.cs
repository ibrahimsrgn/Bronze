using UnityEngine;

public class GameModeSettings : MonoBehaviour
{
    [SerializeField] private ZombieSpawner zombieSpawner;
    [SerializeField] private GameObject[] gameObjsThatNeedsToBeClosedWhenEndlessMode;
    [SerializeField] private GameObject[] gameObjsThatNeedsToBeClosedWhenStoryMode;

    private void Awake()
    {
        int gameMode = PlayerPrefs.GetInt("GameMode", 0);
        if (gameMode == 0)
        {
            StoryMode();
        }
        else if (gameMode == 1)
        {
            EndlessMode();
        }
    }
    private void EndlessMode()
    {
        foreach (var obj in gameObjsThatNeedsToBeClosedWhenEndlessMode)
        {
            obj.SetActive(false);
        }
        zombieSpawner.canSpawn=true;
    }
    private void StoryMode()
    {
        foreach (var obj in gameObjsThatNeedsToBeClosedWhenStoryMode)
        {
            obj.SetActive(false);
        }
        zombieSpawner.canSpawn=false;
    }
}
