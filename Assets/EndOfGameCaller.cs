using UnityEngine;

public class EndOfGameCaller : MonoBehaviour
{
    public void CloseEyes()
    {
        EndGameScript endGameScript = FindAnyObjectByType<EndGameScript>();
        endGameScript.ClosingEyesAndWaking();
    }

    public void LastEvents()
    {
        EndGameScript endGameScript = FindAnyObjectByType<EndGameScript>();
        endGameScript.LastStand();
    }

    public void FinishTheGame()
    {
        EndGameScript endGameScript = FindAnyObjectByType<EndGameScript>();
        endGameScript.FinishTheGame();
    }
}
