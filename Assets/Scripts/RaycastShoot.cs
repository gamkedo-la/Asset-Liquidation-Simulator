﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastShoot : MonoBehaviour
{

    public InputActionReference FireButton;
    public ParticleSystem particlesOnShoot;
    public Transform endOfGun;
    // Start is called before the first frame update
    void Start()
    {
      FireButton.action.performed += HandleFireButtonPress;
    }

    void HandleFireButtonPress(InputAction.CallbackContext obj)
    {
      ParticleSystem muzzleBlast;
      RaycastHit hit;
      if(Physics.Raycast(transform.position, transform.forward, out hit))
      {
        Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(100.0f);
            }
      }
      muzzleBlast = Instantiate(particlesOnShoot, endOfGun.position, endOfGun.rotation);
      Destroy(muzzleBlast, 2f);
    }
}
