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
            Tickables = ObjectsToCheck.Select(objects => objects.GetComponent<ITickable>()).Where(obj => obj != null)
                .ToArray();

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