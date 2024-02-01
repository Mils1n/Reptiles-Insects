using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blaster : MonoBehaviour
{
    //bulet
    public GameObject Bullet;
    public GameObject Hand;
    public GameObject оружие;
    

    //bullet force
    public float shootForce;
    public float upwardForce;

    //Gun stats
    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;

    public int magazineSize;
    public int bulletsPerTap;
    public int damage;

    public bool allowButtonHold;
    
    int bulletsLeft;
    int bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting;
    bool readyToShoot;
    bool reloading;

    //Reference
    public Camera FPSCamera;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing
    public bool allowInvoke = true;
    

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display, if it exists
        if(ammunitionDisplay != null)
          ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if(allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);

        //Reloading
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
        
    }

    private void Shoot()
    {
        readyToShoot = false;

    

        //Find the exact hit position using a raycast
        Ray ray = FPSCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0)); //Just a middle of your
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
          targetPoint = hit.point;
          else
          targetPoint = ray.GetPoint(75); //Just a poit far away from the player

          //Calculate direction from attackPoint to targetPoint
          Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

          //Calculate spread
          float x = Random.Range(-spread, spread);
          float y = Random.Range(-spread, spread);

          //Calculate new direction with spread
          Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

          //Instantiate bullet/projectile
          GameObject currentBullet = Instantiate(Bullet, attackPoint.position, Quaternion.identity);
          //Rotate bullet to shoot direction
          currentBullet.transform.forward = directionWithSpread.normalized;

          //Add forces to bullet
          currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
          currentBullet.GetComponent<Rigidbody>().AddForce(FPSCamera.transform.up * upwardForce, ForceMode.Impulse);

          //Add recoil to player
          playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);

          //Instantiate muzzle flash, if you have one
          if(muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletsPerTap make sure torepeat shoot function
        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
          Invoke("Shoot", timeBetweenShots);

          
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
