using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
   void TakeDamage(float amount);

   void ChangeMeshColor(Color newcolor);
}
