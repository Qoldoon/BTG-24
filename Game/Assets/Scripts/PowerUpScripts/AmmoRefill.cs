using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRefill : MonoBehaviour
{
    //TODO: redo all this
    private int revolverExtraAmmo = 3;
    private int launcherExtraAmmo = 1;
    private int akExtraAmmo = 10;
    private int sniperExtraAmmo = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RevolverAmmoRefill(collision);
            LauncherAmmoRefill(collision);
            AkAmmoRefill(collision);
            SniperAmmoRefill(collision);
            Destroy(gameObject);
        }
    }

    private void RevolverAmmoRefill(Collider2D player)
    {
        GameObject revolver = GameObject.Find("Revolver(Clone)");
        if (revolver != null)
        {
            revolver.GetComponent<Shoot>().ammoCount += revolverExtraAmmo;
        }
    }

    private void LauncherAmmoRefill(Collider2D player)
    {
        GameObject launcher = GameObject.Find("Missile Launcher(Clone)");
        if (launcher != null)
        {
            launcher.GetComponent<Shoot>().ammoCount += launcherExtraAmmo;
        }
    }
    private void AkAmmoRefill(Collider2D player)
    {
        GameObject ak = GameObject.Find("AK(Clone)");
        if (ak != null)
        {
            ak.GetComponent<Shoot>().ammoCount += akExtraAmmo;
        }
    }
    private void SniperAmmoRefill(Collider2D player)
    {
        GameObject ak = GameObject.Find("Sniper(Clone)");
        if (ak != null)
        {
            ak.GetComponent<Shoot>().ammoCount += sniperExtraAmmo;
        }
    }

}
