using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;
    public int enemyMinDamage;
    public int enemyMaxDamage;
    public int maxHP;
    public int currentHP;
    public int xpDrop;
    public int goldDrop;

    ParticleSystem particles;

    private void Start()
    {
        if (transform.childCount > 0 && transform.GetChild(0).TryGetComponent(out ParticleSystem p))
            particles = p;
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0){
            currentHP = 0;
            return true;
        }
        else
            return false;
    }

    public void EnemyDie()
    {
        var main = particles.main;
        var emi = particles.emission;
        main.maxParticles = xpDrop;
        emi.rateOverTime = xpDrop / 3;
        main.duration = xpDrop / emi.rateOverTime.constant;
        particles.Play();

        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + goldDrop);
    }
}
