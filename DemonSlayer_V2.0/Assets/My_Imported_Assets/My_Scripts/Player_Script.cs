using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class Player_Script : MonoBehaviour
{
    public static Player_Script instance;

    public float reloadBarSpeed;
    public int gunDamage = 1;                                           // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player

    private Camera fpsCam;                                              // Holds a reference to the first person camera
    private AudioSource gunAudio;                                       // Reference to the audio source which will play our shooting sound effect                                    // Reference to the LineRenderer component which will display our laserline
    private float nextFire; 

    private int currentAmmo;
    private int magazineCapacity;
    private int maximumAmmo;
    private GameObject projectileGO;

    private float reloadDamage = 1;

    private bool bReloading;

    [SerializeField]
    private int health = 100;
    [SerializeField]
    private int maxHealth = 100;

    public GameObject Shot1;
    public GameObject Wave;

    public Transform gunNozzleTrans;

    public Animator swordAmin;

    private bool bSword = true;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        magazineCapacity = GetMagazineCapacity();
        currentAmmo = magazineCapacity;
        maximumAmmo = GetMaximumAmmo();

        Scene_Controller_Script.instance.UpdateAmmoText(currentAmmo.ToString() + " / " + maximumAmmo.ToString());

        // Get and store a reference to our AudioSource component
        gunAudio = GetComponent<AudioSource>();

        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInChildren<Camera>();
    }
    void Update()
    {
        GameObject Bullet;
        if (health > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAmmo >= 1 && !bReloading)
                {
                    currentAmmo--;
                    Bullet = Shot1;
                    //Fire
                    // Create a vector at the center of our camera's viewport
                    Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                    // Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
                    Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);

                    // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
                    if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
                    {
                        // Update the time when our player can fire next
                        nextFire = Time.time + fireRate;

                        gunAudio.Play();

                        // Create a vector at the center of our camera's viewport
                        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                        // Declare a raycast hit to store information about what our raycast has hit
                        RaycastHit hit;

                        // Check if our raycast has hit anything
                        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
                        {                            
                            // Get a reference to a health script attached to the collider we hit
                            if (hit.transform.GetComponent<TakeDamage_Script>() != null)
                                hit.transform.GetComponent<TakeDamage_Script>().Damage(Weapons_Class.instance.weaponDamage * (int)reloadDamage);

                            if (hit.transform.GetComponent<Door_Script>() != null)
                                hit.transform.GetComponent<Door_Script>().OpenDoor();

                            // Check if the object we hit has a rigidbody attached
                            if (hit.rigidbody != null)
                            {
                                // Add force to the rigidbody we hit, in the direction from which it was hit
                                hit.rigidbody.AddForce(-hit.normal * hitForce);
                            }
                        }
                    }
                    GameObject s1 = (GameObject)Instantiate(Bullet, gunNozzleTrans.position, gunNozzleTrans.rotation);
                    s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());
                    GameObject wav = (GameObject)Instantiate(Wave, gunNozzleTrans.position, gunNozzleTrans.rotation);
                    wav.transform.localScale *= 0.25f;
                    wav.transform.Rotate(Vector3.left, 90.0f);
                    wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
                    Scene_Controller_Script.instance.UpdateAmmoText(currentAmmo.ToString() + " / " + maximumAmmo.ToString());
                }                    
                if (currentAmmo < 1 && maximumAmmo > 1 && !bReloading)
                {
                    reloadDamage = 4;
                    Scene_Controller_Script.instance.UpdateAmmoText("'R' to  reload");
                }
                else if (maximumAmmo < 1 && currentAmmo < 1)
                {
                    reloadDamage = 4;
                    Scene_Controller_Script.instance.UpdateAmmoText("No ammo!");
                }                  
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                if (!bReloading && maximumAmmo + currentAmmo > 0)
                {
                    if (!Scene_Controller_Script.instance.ActiveReloadScrollbarState())
                    {
                        Scene_Controller_Script.instance.ActiveReloadScrollbarOn(true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!bReloading)
                {
                    if (Scene_Controller_Script.instance.ActiveReloadScrollbarState())
                    {
                        reloadDamage = Scene_Controller_Script.instance.ActiveReloadScrollbar().value;

                        if (reloadDamage <= 0.2f)
                        {
                            reloadDamage = 1;
                        }
                        else if (reloadDamage > 0.2f && reloadDamage <= 0.4f)
                        {
                            reloadDamage = 2;
                        }
                        else if (reloadDamage > 0.4f && reloadDamage <= 0.6f)
                        {
                            reloadDamage = 3;
                        }
                        else if (reloadDamage > 0.6f && reloadDamage <= 0.8f)
                        {
                            reloadDamage = 2;
                        }
                        else if (reloadDamage > 0.8f)
                        {
                            reloadDamage = 1;
                        }

                        Scene_Controller_Script.instance.ActiveReloadScrollbarOn(false);
                        StartCoroutine("ReloadDelay");
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (bSword)
                {
                    StartCoroutine("SwordDelay");
                    swordAmin.SetTrigger("Attack");  
                }                
            }

            if (Scene_Controller_Script.instance.ActiveReloadScrollbarState())
                Scene_Controller_Script.instance.ActiveReloadScrollbar().value = Mathf.PingPong(reloadBarSpeed * Time.time, 1);
        }
    }

    protected int GetMaximumAmmo()
    {
        return (Weapons_Class.instance.MaxAmmo());
    }
    protected int GetMagazineCapacity()
    {
        return (Weapons_Class.instance.MagazineCapacity());
    }

    #region Getters
    public Vector3 PlayerPosition()
    {
        return (transform.position);
    }
    public Transform PlayerTransform()
    {
        return (transform);
    }
    public int MaxHealth()
    {
        return (maxHealth);
    }
    public int MaximumAmmo()
    {
        return (maximumAmmo);
    }
    public int CurrentAmmo()
    {
        return (currentAmmo);
    }
    #endregion

    public int ReturnReloadDamage()
    {
        return ((int)reloadDamage);
    }

    public int Health()
    {
        return (health);
    }
    public void DecreaseHealth(int healthAmount)
    {
        health -= healthAmount;
        if (health <= 0)
        {
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Game_Controller_Script.instance.UpdateMessageText("GAME OVER");
        }
    }

    IEnumerator ReloadDelay()
    {
        bReloading = true;
        Game_Controller_Script.instance.UpdateAmmoText("Reloading...");

        yield return new WaitForSeconds(reloadDamage);

        if (currentAmmo + maximumAmmo >= magazineCapacity)
        {
            maximumAmmo = maximumAmmo - (magazineCapacity - currentAmmo);
            currentAmmo = magazineCapacity;
        }
        else if (currentAmmo + maximumAmmo < magazineCapacity)
        {
            currentAmmo += maximumAmmo;
            maximumAmmo = 0;
        }
        Game_Controller_Script.instance.UpdateAmmoText(currentAmmo.ToString() + " / " + maximumAmmo.ToString());
        bReloading = false;
    }

    IEnumerator SwordDelay()
    {
        bSword = false;
        yield return new WaitForSeconds(0.5f);
        bSword = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("MaxAmmo"))
        {
            maximumAmmo = GetMaximumAmmo();
            Game_Controller_Script.instance.UpdateAmmoText(currentAmmo.ToString() + " / " + maximumAmmo.ToString());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag.Equals("MaxHealth"))
        {
            Debug.Log("Max health pickup");
            health = maxHealth;
            Game_Controller_Script.instance.UpdateHealthScrollbar();
            Destroy(other.gameObject);
        }
    }
}
