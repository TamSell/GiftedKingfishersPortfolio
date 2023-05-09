using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PlayerMomentum1 : MonoBehaviour
{
    [SerializeField] private PlayerController2 energizer;

    private float energy;
    public bool inMomentum;

    public void Momentum()
    {
        energy = energizer.currentEnergy;
        if(inMomentum)
        {
            EnergyBuildUp();
        }
        else
        {
            if (energizer.currentEnergy > 0)
            {
                energizer.currentEnergy -= 1 * Time.deltaTime;
            }
        }
    }
    private float EnergyBuildUp()
    {

        if (energy < energizer.energyMax)
        {
            if (energizer.speed > 12)
            {
                energizer.currentEnergy += 3 * Time.deltaTime;
            }
            else if (energizer.speed > 20)
            {
                energizer.currentEnergy += 5 * Time.deltaTime;

            }
            else if (energizer.speed > 40)
            {
                energizer.currentEnergy += 10 * Time.deltaTime;
            }
            else if (energizer.currentEnergy > 0)
            {

                energizer.currentEnergy -= 1 * Time.deltaTime;
            }
        }
        return energy;
    }

    private void MomentumState()
    {
        inMomentum =!inMomentum;
    }
}
