using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ability : MonoBehaviour
{
    private PlayerManager _playerManager;

    [Header("Shoot")]
    public Image shootingImage;
    public float cooldown1 = 1f;
    bool isCooldown1 = false;

    [Header("Ability")]
    public Image AbilityImage;
    public float cooldown2 = 10f;
    bool isCooldown2 = false;

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        cooldown1 = _playerManager.m_BaseAttackTime;
        cooldown2 = _playerManager.m_SpecialCooldown;
        shootingImage.fillAmount = 0;
        AbilityImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot1();
        Ability1();
    }
    void Shoot1()
    {
        if (Input.GetButtonUp("Fire1") && isCooldown1 == false)
        {
            isCooldown1 = true;
            shootingImage.fillAmount = 1;
        }
        if (isCooldown1)
        {
            shootingImage.fillAmount -= 1 / cooldown1 * Time.deltaTime;
            if (shootingImage.fillAmount <= 0)
            {
                shootingImage.fillAmount = 0;
                isCooldown1 = false;

            }
        }
    }
    void Ability1()
    {
        if (Input.GetButtonUp("Fire2") && isCooldown2 == false)
        {
            isCooldown2 = true;
            AbilityImage.fillAmount = 1;
        }
        if (isCooldown2)
        {
            AbilityImage.fillAmount -= 1 / cooldown2 * Time.deltaTime;
            if (AbilityImage.fillAmount <= 0)
            {
                AbilityImage.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }
}
