using UnityEngine;
using System.Collections;

public class InventorySystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    //Wird durch reloadAktion aufgerufen
    public void reloadAmmo(GameObject figurine)
    {
        InventoryComponent inventory = figurine.GetComponent<InventoryComponent>();
        //Prüfe ob überhaupt genug Munition im Magazin zum nachladen
        if(inventory.amountMagazines > 0)
        {
            //Prüfe welcher Spieler dran ist und ziehe dann AP ab
            if (GameObject.Find("Manager").GetComponent<ManagerSystem>().getPlayerTurn())
            {
                if (GameObject.Find("Player1").GetComponent<PlayerComponent>().actionPoints > 0)
                {
                    GameObject.Find("Player1").GetComponent<PlayerComponent>().useAP();
                }
            }
            else
            {
                if (GameObject.Find("Player2").GetComponent<PlayerComponent>().actionPoints > 0)
                {
                    GameObject.Find("Player2").GetComponent<PlayerComponent>().useAP();
                }
            }

            AudioManager.playReload();

            //Verringert Anzahl der Magazine im Inventar
            inventory.amountMagazines--;
            WeaponComponent weapon = inventory.primary;
            //Stellt Anzahl an Kugeln in der Waffe auf Maximum
            weapon.currentBulletsInMagazine = weapon.magazineSize;
        }
    }

    //Wird durch GranatAktion aufgerufen
    public void decreaseGrenades(GameObject figurine)
    {
        InventoryComponent inventory = figurine.GetComponent<InventoryComponent>();
        inventory.amountGrenades--;
    }

    //Wird durch RauchgranatenAktion aufgerufen
    public void decreaseSmokegrenades(GameObject figurine)
    {
        InventoryComponent inventory = figurine.GetComponent<InventoryComponent>();
        inventory.amountSmokes--;
    }
    
    public void decreaseAmmoInMagazine(GameObject figurine, int amount)
    {
        WeaponComponent weapon = figurine.GetComponentInChildren<WeaponComponent>();
        Debug.Log("Munition im Magazin verringert");
        weapon.currentBulletsInMagazine -= amount;
    }

    //Wird durch MolotovAktion aufgerufen
    public void decreaseMolotovs(GameObject figurine)
    {
        InventoryComponent inventory = figurine.GetComponent<InventoryComponent>();
        inventory.amountMolotovs--;
    }

    //Wird durch MedikitAktion aufgerufen
    public int decreaseMedikits(GameObject figurine)
    {
        InventoryComponent inventory = figurine.GetComponent<InventoryComponent>();
        if(inventory.amountMediKits > 0)
            inventory.amountMediKits--;
        return inventory.amountMediKits;
    }

   /* 
    * //Wird durch MinenAktion aufgerufen
    public void decreaseMines(GameObject figurine)
    {
        InventoryComponent inventory = figurine.GetComponent<InventoryComponent>();
        inventory.amountMines--;
    }
    */
}
