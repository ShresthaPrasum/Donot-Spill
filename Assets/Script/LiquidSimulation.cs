using System;
using System.Collections;
using UnityEngine;

public class LiquidSimulation : MonoBehaviour
{
	[SerializeField] private GameObject waterDropPrefap;
	[SerializeField] private Transform waterSpawnPoint;
	[SerializeField] private Transform spawnedWaterParent;
	
	[SerializeField] private string waterTagName = "Water";

	[SerializeField] private int totalDropsToSpawn = 120;
	[SerializeField] private float secondsBetweenDrops = 0.03f;
	[SerializeField] private float waitBeforeStarting = 0.1f;

	[SerializeField] private bool addRandomXOffset = true;

	[SerializeField] private float maxRandomXOffset = 0.05f;

	private Coroutine spawnRoutine;
	private int spawnedDropCount;

	public bool IsSpawningWater => spawnRoutine !=null;

	public int SpawnedDropCount => spawnedDropCount;

	public event Action FinishedSpawningWater;

	public void StartSpawningWater()
	{
		if(IsSpawningWater)
		{
			return;
		}

		
	}

 }
