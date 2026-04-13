using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GlassFillDetector : MonoBehaviour
{
    [Header("What Counts As Water")]
    [SerializeField] private string waterTagName = "Water";

    [Header("Win Condition")]
    [SerializeField] private int dropsNeededInsideGlass = 40;

    private readonly HashSet<int> waterDropIdsInside = new HashSet<int>();
    private bool alreadySentTargetReachedEvent;

    public int CurrentDropCount => waterDropIdsInside.Count;
    public int TargetDropCount => dropsNeededInsideGlass;
    public bool HasReachedTarget => CurrentDropCount >= TargetDropCount;

    public float FillPercent01
    {
        get
        {
            if (TargetDropCount <= 0)
            {
                return 1f;
            }

            return Mathf.Clamp01((float)CurrentDropCount / TargetDropCount);
        }
    }

    public event Action<int, int> FillCountChanged;
    public event Action TargetReached;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsWaterDrop(other))
        {
            return;
        }

        if (waterDropIdsInside.Add(other.gameObject.GetInstanceID()))
        {
            NotifyFillChanged();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsWaterDrop(other))
        {
            return;
        }

        if (waterDropIdsInside.Remove(other.gameObject.GetInstanceID()))
        {
            NotifyFillChanged();
        }
    }

    public void ResetCounter()
    {
        waterDropIdsInside.Clear();
        alreadySentTargetReachedEvent = false;
        NotifyFillChanged();
    }

    private bool IsWaterDrop(Collider2D colliderToCheck)
    {
        if (string.IsNullOrWhiteSpace(waterTagName))
        {
            return false;
        }

        return colliderToCheck.CompareTag(waterTagName);
    }

    private void NotifyFillChanged()
    {
        FillCountChanged?.Invoke(CurrentDropCount, TargetDropCount);

        if (HasReachedTarget)
        {
            if (!alreadySentTargetReachedEvent)
            {
                alreadySentTargetReachedEvent = true;
                TargetReached?.Invoke();
            }
            return;
        }

        alreadySentTargetReachedEvent = false;
    }

    private void Reset()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }
}