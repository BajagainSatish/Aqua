using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationPointClassifier : MonoBehaviour
{
    [SerializeField] private bool initializationPointBelongsToP1;
    public bool InitializationPointBelongsToP1
    {
        get
        {
            return initializationPointBelongsToP1;
        }
    }
}
