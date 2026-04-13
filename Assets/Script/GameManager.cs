using System;
using System.Collections;
using UnityEngine;

public class HappyGlassGameManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private LineDrawingController lineDrawer;
    [SerializeField] private LiquidSimulation waterSpawner;
    [SerializeField] private GlassFillDetector glassFillDetector;

    [Header("Round Flow")]
    [SerializeField] private bool startWaterAfterFirstLine = true;
    [SerializeField] private bool disableDrawingWhenRoundStarts = true;
    [SerializeField] private float secondsBeforeCheckingWin = 8f;

    private bool isRoundRunning;
    private Coroutine resultCheckRoutine;

    public event Action<bool> RoundFinished;

    private void OnEnable()
    {
        if (lineDrawer != null)
        {
            lineDrawer.FirstLineFinished += HandleFirstLineFinished;
        }
    }

    private void OnDisable()
    {
        if (lineDrawer != null)
        {
            lineDrawer.FirstLineFinished -= HandleFirstLineFinished;
        }

        if (resultCheckRoutine != null)
        {
            StopCoroutine(resultCheckRoutine);
            resultCheckRoutine = null;
        }
    }

    public void StartRound()
    {
        if (isRoundRunning)
        {
            return;
        }

        isRoundRunning = true;

        if (disableDrawingWhenRoundStarts && lineDrawer != null)
        {
            lineDrawer.SetDrawingAllowed(false);
        }

        if (waterSpawner != null)
        {
            waterSpawner.StartSpawningWater();
        }

        if (resultCheckRoutine != null)
        {
            StopCoroutine(resultCheckRoutine);
        }

        resultCheckRoutine = StartCoroutine(CheckRoundResultAfterDelay());
    }

    public void ResetRound()
    {
        if (resultCheckRoutine != null)
        {
            StopCoroutine(resultCheckRoutine);
            resultCheckRoutine = null;
        }

        isRoundRunning = false;

        if (waterSpawner != null)
        {
            waterSpawner.ResetAllWaterDrops();
        }

        if (glassFillDetector != null)
        {
            glassFillDetector.ResetCounter();
        }

        if (lineDrawer != null)
        {
            lineDrawer.ClearAllDrawnLines();
            lineDrawer.SetDrawingAllowed(true);
        }
    }

    private void HandleFirstLineFinished()
    {
        if (startWaterAfterFirstLine)
        {
            StartRound();
        }
    }

    private IEnumerator CheckRoundResultAfterDelay()
    {
        yield return new WaitForSeconds(secondsBeforeCheckingWin);

        bool playerWon = glassFillDetector != null && glassFillDetector.HasReachedTarget;

        Debug.Log(playerWon ? "Level Complete" : "Level Failed");
        RoundFinished?.Invoke(playerWon);

        isRoundRunning = false;
        resultCheckRoutine = null;
    }
}