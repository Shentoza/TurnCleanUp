using UnityEngine;
using System.Collections;

public class InventoryComponent : MonoBehaviour {


    //Inventar (primaerwaffe, sekundaerwaffe, equipment1, equipment2) siehe Enums.cs    
    public Enums.PrimaryWeapons primaryWeaponType;
    public Enums.SecondaryWeapons secondaryWeaponType;
    public Enums.Equipment utility1;
    public Enums.Equipment utility2;

    //Primärwaffe
    public WeaponComponent primary;
    //Sekundärwaffe
    public WeaponComponent secondary;

    public ArmorComponent armor;
    //Ist Primärwaffe ausgewählt? 
    public bool isPrimary;
    //Anzahl Rauchgranaten
    public int amountSmokes;
    //Anzahl Teargas
    public int amountTeargas;
    //Anzahl Granaten
    public int amountGrenades;
    //Anzahl Molotovs
    public int amountMolotovs;
    //Anzahl Medikits
    public int amountMediKits;
    //Anzahl Magazine
    public int amountMagazines;

	// Use this for initialization
	void Start () {
        isPrimary = true;
	}
	public WeaponComponent getCurrentWeapon()
    {
        if (isPrimary)
            return primary;
        else
            return secondary;
    }
}
