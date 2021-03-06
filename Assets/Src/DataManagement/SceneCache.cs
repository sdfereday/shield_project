﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Constants;

namespace Game.DataManagement
{
    public class SceneCache : MonoBehaviour
    {
        /// <summary>
        /// Item Storage Cache
        /// </summary>
        private List<string> itemCache;

        /* After game load, you'd populate this with whatever's available
        /* for that area (storage not yet implemented however) */
        private void Awake() =>
            itemCache = new List<string>();

        public void RegisterItem(string id)
        {
            if (ItemExists(id))
            {
                throw new UnityException(GlobalConsts.SESSION_CACHE_ERROR_DUPLICATE);
            }

            itemCache.Add(id);
        }

        public bool ItemExists(string id) =>
            itemCache.Any(item => item == id);
    }
}