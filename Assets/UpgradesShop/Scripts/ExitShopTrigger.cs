using System;
using UnityEngine;

public class ExitShopTrigger : MonoBehaviour
{
   private const string PlayerTag = "Player";

   public Action ExitShopAction;

   private void OnTriggerEnter(Collider other)
   {
      if(other.tag != PlayerTag)
      {
         return;
      }

      ExitShopAction?.Invoke();
   }
}
