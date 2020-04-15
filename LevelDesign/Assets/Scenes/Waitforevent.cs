using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Waitforevent : MonoBehaviour
{
   public UnityEvent Event;
   public float waittime;

   public void Call()
   {
      StartCoroutine(Wait());
   }

   private IEnumerator Wait()
   {
      yield return new WaitForSeconds(waittime);
      Event.Invoke();
   }
}
