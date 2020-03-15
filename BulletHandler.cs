using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public ParticleSystem ps;
    List<ParticleSystem.Particle> live = new List<ParticleSystem.Particle>();


    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    private void OnParticleTrigger()
    {
        int liveBullets = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, live);


        for (int i = 0; i < liveBullets; i++)
        {
            Debug.Log("Particles have entered");
            ParticleSystem.Particle p = live[i];
            p.startColor = new Color(255, 0, 0, 255);
            live[i] = p;
            //currAmmo--;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, live);

    }

}
