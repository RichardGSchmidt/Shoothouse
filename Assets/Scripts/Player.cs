//This is from a sebastian lague video, doing it in parallel with a friend
//(https://github.com/Pascal-Bustamante) to help teach him some Unity!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : Actors {

    public float moveSpeed = 5;
    PlayerController controller;
    GunController gunController;

    Camera viewCamera;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        #region Movement
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);
        #endregion

        #region Look Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }
        #endregion

        #region weapon Input
        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
        #endregion

    }
}
