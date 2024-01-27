using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInteractableObject : MonoBehaviour
{
    abstract public IEnumerator Task(Orc orc);

    abstract public void OnStopTask(Orc orc);
}
