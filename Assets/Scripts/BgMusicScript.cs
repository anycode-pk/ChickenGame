using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusicScript : MonoBehaviour
{
    public BgMusicScript BgMusicInstance;

    private void Awake()
    {
        if(BgMusicInstance != null && BgMusicInstance != this) 
        {
            Destroy(this.gameObject);
            return;
        }

        BgMusicInstance = this;
        DontDestroyOnLoad(this);
    }
}
