using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.PoolingSystem
{
	public partial class PoolingManager
	{
		private class Pool
		{
			private readonly Component _prefab;
			private readonly Transform _parent;
			private readonly Queue<Component> _objects;

			public Pool(Component prefab, Transform parent)
			{
				_prefab = prefab;
				_parent = parent;
				_objects = new Queue<Component>();
			}

			public Pool(Component prefab, int initialCount)
			{
				_prefab = prefab;
				_objects = new Queue<Component>(initialCount);
				for (int i = 0; i < initialCount; i++)
				{
					Add(CreateInstance());
				}
			}

			public Component Get() => _objects.Count > 0 ? _objects.Dequeue() : CreateInstance();

			public void Add(Component borrowed)
			{
				var gameObject = borrowed.gameObject;
				gameObject.SetActive(false);
				gameObject.transform.SetParent(_parent);
				
				_objects.Enqueue(borrowed);
			}

			private Component CreateInstance() => Object.Instantiate(_prefab);
		}
	}
}