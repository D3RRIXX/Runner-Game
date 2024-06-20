using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BoostSystem.Boosts
{
	public class BoostSpawner : MonoBehaviour
	{
		[Serializable]
		private class BoostSpawnConfig
		{
			public BoostBase BoostPrefab;
			[Range(0f, 1f)] public float Chance;
		}

		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private BoostSpawnConfig[] _possibleBoosts;

		private void OnValidate()
		{
			if (_possibleBoosts.Sum(x => x.Chance) > 1f)
				Debug.LogWarning("Total boost spawn chance exceeds 100%", this);
		}

		private void OnEnable()
		{
			float randValue = Random.value;
			TrySpawnBoost(randValue);
		}

		private void TrySpawnBoost(float randValue)
		{
			float accumulativeChance = 0f;

			foreach (BoostSpawnConfig config in _possibleBoosts)
			{
				accumulativeChance += config.Chance;
				if (randValue < accumulativeChance)
				{
					SpawnBoost(config.BoostPrefab);
					break;
				}
			}
		}

		private void SpawnBoost(BoostBase boostPrefab)
		{
			Instantiate(boostPrefab, _spawnPoint.position, Quaternion.identity, transform);
		}
	}
}