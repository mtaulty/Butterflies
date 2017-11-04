using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {

  public GameObject butterflyPrefab;

  public void Create()
  {
    if (!this.lastCall.HasValue ||
      ((DateTime.Now - this.lastCall.Value).TotalSeconds > 5))
    {
      var newObject = (GameObject)GameObject.Instantiate(this.butterflyPrefab);
      var forward = Camera.main.transform.forward;
      forward.Normalize();

      newObject.transform.position = Camera.main.transform.position + forward;
      newObject.transform.forward = forward;
      this.lastCall = DateTime.Now;
    }
  }
  public void Rest()
  {
    var butterflies = GameObject.FindGameObjectsWithTag("Butterfly");

    foreach (var butterfly in butterflies)
    {
      var controller = butterfly.gameObject.GetComponent<ButterflyController>();
      controller.Rest();
    }
    Physics.gravity = new Vector3(0, -3.0f, 0);
  }
  DateTime? lastCall;
}
