using System;
using System.Collections;
using System.Collections.Generic;
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

        private bool isTicking;

         private List<ITickable> Tickables { get; set; }

        private void Awake()
        {
            StartCoroutine(TickClock());
        }


        IEnumerator TickClock()
        {
            foreach (var tickable in Tickables)
            {
                tickable.Tick();
            }

            yield return new WaitForSeconds(tickRate);
            if (isTicking) StartCoroutine(TickClock());
        }
    }
}