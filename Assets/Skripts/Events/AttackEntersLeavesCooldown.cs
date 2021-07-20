using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Event which is executed, when the Attacker enters or leave a cooldown
/// </summary>
public class AttackEntersLeavesCooldown : EventManager.Event<AttackEntersLeavesCooldown>
{
    public bool isInCooldown;
    public GameObject attackButton;

    public override void Execute()
    {
      if (attackButton != null) {
        attackButton.GetComponent<Image>().color = isInCooldown ? Color.black : Color.white;
      }
    }
}
