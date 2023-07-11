using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    private void Start()
    {
        if(_particle == null)
        {
            _particle = GetComponent<ParticleSystem>();
        }

        if (_particle!=null)
        {
            ParticleSystem.MainModule mainModule = _particle.main;
            mainModule.loop = false;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }
    }

    public void OnParticleSystemStopped()
    {
        GameFXManager.Ins.RecycleAFX(this.gameObject);
    }


    
}
