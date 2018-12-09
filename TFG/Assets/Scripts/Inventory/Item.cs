using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {STAMINAPOTION, HEALTHPOTION, AGUASAGRADA, ELIXIRPODER};

public class Item : MonoBehaviour {

    public ItemType type;

    public Sprite spriteNeutral;

    public Sprite spriteHighlighted;

    public int maxSize;

    public void Use()
    {
        Questlog.MyInstance.UpdateQuestItemCount(this);
        switch (type)
        {
            case ItemType.STAMINAPOTION:
                PlayerHealth.MyInstance.Recover();
                break;
            case ItemType.HEALTHPOTION:
                PlayerHealth.MyInstance.Heal();
                break;
            case ItemType.AGUASAGRADA:
                Debug.Log("Used Agua sagrada");
                break;
            case ItemType.ELIXIRPODER:
                PlayerStats.MyInstance.UseElixir();
                //Añadir Sonido o Efecto
                break;
        }
    }
}
