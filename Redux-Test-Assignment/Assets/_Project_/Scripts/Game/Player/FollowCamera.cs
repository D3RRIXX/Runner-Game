using Cinemachine;
using UnityEngine;

namespace Game.Player
{
	public class FollowCamera : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		
		public void Construct(Transform player)
		{
			_virtualCamera.Follow = player;
			_virtualCamera.LookAt = player;
		}
	}
}