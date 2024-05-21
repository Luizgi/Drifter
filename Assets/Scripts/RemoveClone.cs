using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveClone : MonoBehaviour
{

    public void Awake()
    {
        gameObject.name = gameObject.name.Replace("(Clone)", "");
    }
}
