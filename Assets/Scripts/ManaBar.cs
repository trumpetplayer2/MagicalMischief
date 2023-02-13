using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Mana mana;
    private Image barImage;
    private void Awake()
    {
        barImage = transform.GetComponent<Image>();

        mana = new Mana();
    }

    private void Update()
    {
        mana.Update();

        barImage.fillAmount = mana.GetManaNormalized();
    }

    public Mana GetMana()
    {
        return mana;
    }
}

public class Mana
{
    public const int MANA_MAX = 100;
    private float manaAmount;
    private float regenRate;

    public Mana()
    {
        manaAmount = 0;
        regenRate = 30f;
    }

    public void Update()
    {
        manaAmount += regenRate * Time.deltaTime;
        manaAmount = Mathf.Clamp(manaAmount, 0f, MANA_MAX);
    }

    public bool TrySpendMana(int amount)
    {
        if(manaAmount>= amount)
        {
            manaAmount -= amount;
            return true;
        }
        return false;
    }

    public float GetManaNormalized()
    {
        return manaAmount / MANA_MAX;
    }
}
