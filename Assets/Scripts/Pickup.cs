using UnityEngine;
using TMPro;

public class Pickup : MonoBehaviour
{
    public GameObject pickupWeapon;
    public TextMeshProUGUI text;

    private GameObject displayWeapon;
    private bool isVisible = false;

    private void Awake()
    {
        displayWeapon = Instantiate(pickupWeapon, transform);
        text.text = pickupWeapon.GetComponent<Weapon>().weaponName;
    }

    private void Update()
    {
        if (GameManager.instance.isPaused)
        {
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Weapon weapon = player.GetComponentInChildren<Weapon>();
            if (weapon != null && weapon.weaponName != pickupWeapon.GetComponent<Weapon>()?.weaponName)
            {
                RevealPickup();
                return;
            }
        }

        HidePickup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isVisible)
        {
            collision.GetComponent<PlayerController>().SwapWeapon(pickupWeapon);
        }
    }

    private void HidePickup()
    {
        displayWeapon.SetActive(false);
        text.gameObject.SetActive(false);
        isVisible = false;
    }

    private void RevealPickup()
    {
        displayWeapon.SetActive(true);
        text.gameObject.SetActive(true);
        isVisible = true;
    }
}
