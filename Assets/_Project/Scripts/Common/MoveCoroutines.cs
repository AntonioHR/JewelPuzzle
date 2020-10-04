using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntonioHR.Common
{
    public static class MoveCoroutines
    {

        public static IEnumerator PerformMove(this Transform me, Vector3 endPosition, float duration)
        {
            Vector3 worldStart = me.transform.position;

            float startTime = Time.time;
            float elapsed = 0;

            while (true)
            {
                elapsed = Time.time - startTime;

                me.transform.position = Vector3.Lerp(worldStart, endPosition, elapsed/duration);
                if(elapsed >= duration)
                    break;
                yield return null;
            }
            me.transform.position = endPosition;
        }

    }
}