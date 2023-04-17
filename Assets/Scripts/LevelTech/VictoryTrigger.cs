using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RuntimeEntities.Instance.Player.Rigidbody.isKinematic = true;
        RuntimeEntities.Instance.Player.Rigidbody.velocity = Vector2.zero;
        RuntimeEntities.Instance.Player.Sprite.Renderer.sortingOrder = 100;
        UIController.Instance.StartCoroutine("FadeOut");
        RuntimeEntities.Instance.Player.Active = false;
        RuntimeEntities.Instance.Player.Arm.gameObject.SetActive(false);
        RuntimeEntities.Instance.SetFocusCam();
        UIController.Instance.ShowButtons(1);
        UIController.Instance.SetNormalPointer();
        UIController.Instance.StartCoroutine("SpellVictoryMessage");
    }
}
