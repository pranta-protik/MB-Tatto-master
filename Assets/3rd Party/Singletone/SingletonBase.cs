using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        //Making sure no Object can override Awake (Only implementation inside Singleton)
        protected virtual void Awake()
        {

        }
    }

