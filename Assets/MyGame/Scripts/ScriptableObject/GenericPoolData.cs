using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPoolData<E> : ScriptableObject where E : Enum
{
    [SerializeField] public E type;
    [SerializeField] public bool isAddressables = true;
    [SerializeField] public string addPath;
    [SerializeField] public int defaultCapacity = 10;
    [SerializeField] public int maxSize = 10;
}
