using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scene_Controller_Script : MonoBehaviour {

    public static Scene_Controller_Script instance;          // Ensures there is only 1 instance of this object

    [SerializeField]
    private Text AmmoText;                                  // Assign AmmoText from scene

    [SerializeField]
    private Scrollbar healthScrollbar;

    [SerializeField]
    private GameObject activeReloadGO;

    [SerializeField]
    private Scrollbar activeReloadScrollbar;

    [SerializeField]
    private Text messageText;

    [SerializeField]
    private Text creditsText;

    void Awake()                                            // First function to run in scene
    {
        instance = this;                                    // This object is the only instance of Game_Controller_Script
    }

    void Start()                                            // Sceond function to run in scene
    {
        if (AmmoText == null)                               // Check if the AmmoText has been assigned already
            AmmoText = GameObject.Find("Ammo_Text").GetComponent<Text>();       // If the AmmoText hasn't been assigned assign it

        if (healthScrollbar == null)
            healthScrollbar = GameObject.Find("Health_Scrollbar").GetComponent<Scrollbar>();
    }

    public void UpdateCreditsText()
    {
        creditsText.text = Game_Manager_Script.instance.playerCredits.ToString("N0") + "cr";
    }
    public void UpdateAmmoText(string message)
    {
        if (Player_Script.instance.ReturnReloadDamage() == 1)
        {
            AmmoText.color = Color.cyan;
        }
        else if (Player_Script.instance.ReturnReloadDamage() == 2)
        {
            AmmoText.color = Color.magenta;
        }
        else if (Player_Script.instance.ReturnReloadDamage() == 3)
        {
            AmmoText.color = Color.red;
        }
        else
        {
            AmmoText.color = Color.white;
        }
        AmmoText.text = message;
    }
    public void UpdateHealthScrollbar()
    {
        healthScrollbar.size = (float)Player_Script.instance.Health() / (float)Player_Script.instance.MaxHealth();
    }
    public void UpdateMessageText(string message)
    {
        messageText.text = message;
    }
    public void ActiveReloadScrollbarOn(bool on)
    {
        if (on)
        {
            activeReloadGO.SetActive(true);
            AmmoText.text = "";
        }
        else
        {
            activeReloadGO.SetActive(false);
            UpdateAmmoText(Player_Script.instance.CurrentAmmo().ToString() + " / " + Player_Script.instance.MaximumAmmo().ToString());
        }
    }
    public bool ActiveReloadScrollbarState()
    {
        if (activeReloadGO.activeInHierarchy)
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }
    public Scrollbar ActiveReloadScrollbar()
    {
        return (activeReloadScrollbar);
    }
}
