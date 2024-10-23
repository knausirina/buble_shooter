using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Views;

namespace Field
{
    public class PoolBalls
    {
        private IObjectPool<BubbleView> _pool;

        private readonly GameObject _prefab;

        public PoolBalls(GameObject prefab)
        {
            _prefab = prefab;
        }

        public IObjectPool<BubbleView> Pool
        {
            get
            {
                if (_pool == null)
                {
                   _pool = new LinkedPool<BubbleView>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true);
                }
                return _pool;
            }
        }

        private void OnDestroyPoolObject(BubbleView obj)
        {
            //throw new System.NotImplementedException();
        }

        private void OnReturnedToPool(BubbleView obj)
        {
            //throw new System.NotImplementedException();
        }

        private void OnTakeFromPool(BubbleView obj)
        {
            //throw new System.NotImplementedException();
        }

        private BubbleView CreatePooledItem()
        {
            var gameObject = Object.Instantiate(_prefab);
            return gameObject.GetComponent<BubbleView>();
        }
    }
}