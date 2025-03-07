﻿using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;

namespace Game.BoostSystem
{
	[RequireComponent(typeof(Collider))]
	public abstract class BoostBase : MonoBehaviour
	{
		private void Awake()
		{
			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(Constants.PLAYER_TAG))
			    .Subscribe(x =>
			    {
				    ApplyBoost(x.gameObject);
				    gameObject.SetActive(false);
			    })
			    .AddTo(this);
		}

		protected abstract void ApplyBoost(GameObject player);
	}
}