using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// in this script :
/// handle how the upgrades are selected for each the player can upgrade 
/// apply the upgrade by invoking an event that transfer data to a script 
/// that hold all the players data that can be upgraded(coming soon)
/// listens to an event to trigger the selection process of the upgrades
/// after the play chooses the upgrade the manager should remove the upgrade he chose 
/// or decrease the chance to appear again
/// there are 2 type of upgrade:
/// the ones that appear during the level (normal)
/// and the ones that appear after deafeatinng the boss of the level(higher or rare)
/// </summary>

public enum UpgradeType
{
    health,
    movementspeed,
    damageReduction
}
public class UpgradeManager : MonoBehaviour
{
 
    public void select_upgrade(Upgrade upgrade)
    {
        UpggradeValues[] selected = (UpggradeValues[])upgrade.getupgrade();
        foreach (UpggradeValues v in selected)
        {
            Logging.Log($"your{v.GetUpgradeType()}is incresed by{v.GetValue()/100}%");
        }
        
    }
}
