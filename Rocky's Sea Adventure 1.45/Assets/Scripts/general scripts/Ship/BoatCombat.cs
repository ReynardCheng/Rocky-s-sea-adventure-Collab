using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCombat : MonoBehaviour {

    public int shipHealth = 100;

    //public GameObject[] damageSectionHolder;
    public GameObject[] cannonHolder;

    public GameObject shipLeftSide;
    public GameObject shipRightSide;
    

    public void TakeDamage(int damageToTake, GameObject damageLocation) //Called from script attached to enemy's bullet. Damagelocation being the bullet gameobject.
    {
        {
         //float closestCannonDistance = Mathf.Infinity;
         //GameObject sectionToDamage = null;

            //foreach (GameObject damageSection in damageSectionHolder) //Finding the closest cannon slot to the damage point
            //{
            //    var difference = (damageLocation.transform.position - damageSection.transform.position);
            //    var curDistance = difference.sqrMagnitude;
            //    if (curDistance < closestCannonDistance)
            //    {
            //        closestCannonDistance = curDistance;
            //        sectionToDamage = damageSection;
            //    }
            //}

            //if (sectionToDamage.GetComponent<BuildCannon>() != null)    //Damaged Section has BuildCannon script attached (Potentially has a cannon slot)
            //{
            //    if (sectionToDamage.GetComponent<BuildCannon>().slotTaken)  //A cannon is currently built in this section, thus damage the cannons
            //        sectionToDamage.GetComponent<BuildCannon>().linkedCannonFire.CannonHealth -= damageToTake;
            //    else
            //    {
            //        shipHealth -= damageToTake;
            //        print("ShipHealth:" + shipHealth);
            //    }
            //}

            //if (sectionToDamage.GetComponent<BuildCannon>() == null)    //Damaged section has no BuildCannon script (no cannon in section), damage the ship
            //{
            //    shipHealth -= damageToTake;
            //    print("ShipHealth:" + shipHealth);
            //}
        }   // Old Code


        //This shit new like my new favorit meme SOMEbody toucha my spagetta hahaha smash like button!! Fresh af!!!

        if (IsSideEmpty(IsRightCloserSide(damageLocation))) //Is the side where bullet hits ship empty of cannons? Damage ship directly.
        {
            shipHealth -= damageToTake;
            print(shipHealth);
        }
        else 
        {
            //print(FindClosestSideCannon(IsRightCloserSide(damageLocation), damageLocation).name);

            //Find closest cannon on the side that was hit, and damage it
            FindClosestSideCannon(IsRightCloserSide(damageLocation), damageLocation).GetComponent<CannonHealth>().ChangeHealth(damageToTake, 0);
        }
        
        
        

    }

    bool IsRightCloserSide(GameObject damageLocation)
    {
        //Finding distance to left side of ship and right side of ship
        var leftDiff = damageLocation.transform.position - shipLeftSide.transform.position;
        var leftDiffSqr = leftDiff.sqrMagnitude;

        var rightDiff = damageLocation.transform.position - shipRightSide.transform.position;
        var rightDiffSqr = rightDiff.sqrMagnitude;

        if (leftDiffSqr > rightDiffSqr)
            return true;
        else
            return false;
    }

    bool IsSideEmpty(bool findRightSide)
    {
        //Iterates through all side cannons, and find if slotTaken(cannon present) is true
        bool sideIsEmpty = true;

        if (findRightSide)
        {
            for (int i = 0; i < 3; i++)
            {
                if (cannonHolder[i].GetComponent<BuildCannon>().slotTaken)
                {
                    sideIsEmpty = false;
                    break;
                }
            }
        }
        else
        {
            for (int i = 3; i < 6; i++)
            {
                if (cannonHolder[i].GetComponent<BuildCannon>().slotTaken)
                {
                    sideIsEmpty = false;
                    break;
                }
            }
        }
        return sideIsEmpty;
    }

    GameObject FindClosestSideCannon(bool findRightSide, GameObject damageLocation)
    {
        GameObject closestCannon = null;

        if (findRightSide)
        {
            float closestCannonDistance = Mathf.Infinity;
            for (int i = 0; i < 3; i++)
            {
                var difference = (damageLocation.transform.position - cannonHolder[i].transform.position);
                var curDistance = difference.sqrMagnitude;
                if (cannonHolder[i].GetComponent<BuildCannon>().slotTaken)
                {
                    if (curDistance < closestCannonDistance)
                    {
                        closestCannonDistance = curDistance;
                        closestCannon = cannonHolder[i];
                    }
                }
            }
        }
        else
        {
            float closestCannonDistance = Mathf.Infinity;
            for (int i = 3; i < 6; i++)
            {
                var difference = (damageLocation.transform.position - cannonHolder[i].transform.position);
                var curDistance = difference.sqrMagnitude;
                if (cannonHolder[i].GetComponent<BuildCannon>().slotTaken)
                {
                    if (curDistance < closestCannonDistance)
                    {
                        closestCannonDistance = curDistance;
                        closestCannon = cannonHolder[i];
                    }
                }
            }
        }

        return closestCannon;
    }
}
