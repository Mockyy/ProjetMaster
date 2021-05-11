using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform location;

    public void Restart()
    {
        target.position = location.position;
    }
}
