using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using interfaces;
using UnityEngine;

namespace managers
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// The time in seconds it takes before a Tick takesPlace.
        /// </summary>
        public int tickRate = 1;

        private bool isTicking = true;

        public GameObject[] ObjectsToCheck;
        private ITickable[] Tickables;

        private void Awake()
        {
            var list = new List<ITickable>();
            foreach (var o in ObjectsToCheck)
            {
                list.AddRange(o.GetComponents<ITickable>());
            }

            Tickables = list.ToArray();

            StartCoroutine(TickClock());
        }


        IEnumerator TickClock()
        {
            //Debug.Log("TICK");
            foreach (var tickable in Tickables)
            {
                tickable.Tick();
            }

            yield return new WaitForSeconds(tickRate);
            if (isTicking) StartCoroutine(TickClock());
        }
    }
}