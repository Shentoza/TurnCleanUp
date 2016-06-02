using UnityEngine;
using System.Collections;

public class AbilitySystem : MonoBehaviour {

    public GameObject smoke;
    public GameObject fire;
    public GameObject explosion;
    public GameObject gas;

    public int effektDauer;
    //Shit für animationen zum drehen
    private float turning_Speed = 360.0f;
    private bool turning_Started;
    private float turning_Direction;


    //Grenade Stuff
    private GameObject throwing_Object;
    private float throwing_TimePerCell = 0.25f;
    private float throwing_TimeNeeded;
    private float throwing_TimeSum;
    private float throwing_Length;
    private Vector3 throwing_StartPos;
    private Vector3 throwing_DestinationPos;
    private Cell throwing_DestinationCell;
    private bool throwing_Active = false;
    private Enums.Effects throwing_effect;
    
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (throwing_Active)
            continousThrowing(Time.deltaTime);
	}

    //Methode um anfangen shit zu schmeißen
    public void throwGrenade(Cell ziel, GameObject figur, Enums.Effects effectType)
    {
        AttributeComponent playerAttr = figur.GetComponent<AttributeComponent>();
        InventoryComponent invent = figur.GetComponent<InventoryComponent>();
        int amountOfGrenades = 0;
        switch(effectType)
        {
            case Enums.Effects.Explosion:
                amountOfGrenades = invent.amountGrenades;
                break;
            case Enums.Effects.Fire:
                amountOfGrenades = invent.amountMolotovs;
                break;
            case Enums.Effects.Gas:
                amountOfGrenades = invent.amountTeargas;
                break;
            case Enums.Effects.Smoke:
                amountOfGrenades = invent.amountSmokes;
                break;
        }

        if (ziel.dij_GesamtKosten <= playerAttr.attackRange && amountOfGrenades > 0)
        {
            figur.GetComponentInParent<PlayerComponent>().useAP();
            //while (!checkRotation(ziel, playerAttr))
                            //yield return null;
            checkRotation(ziel, playerAttr);
            throwing_DestinationCell = ziel;
            //Einsatz von AP durch Faehigkeit
            figur.GetComponentInParent<PlayerComponent>().useAP();
            throwing_effect = effectType;
            playerAttr.anim.SetTrigger("Throw");
            switch (effectType)
            {
                case Enums.Effects.Explosion:
                    invent.amountGrenades--;
                    break;
                case Enums.Effects.Fire:
                    invent.amountMolotovs--;
                    break;
                case Enums.Effects.Gas:
                    invent.amountTeargas--;
                    AudioManager.playTeargasLauncher();
                    break;
                case Enums.Effects.Smoke:
                    invent.amountSmokes--;
                    break;
            }

        }
        else {
            Debug.Log("OutOfRange");
        }
    }

    void smokeEffect()
	{
            GameObject smokeTmp = Instantiate(smoke);
  	        smokeTmp.transform.position = new Vector3 (throwing_DestinationCell.transform.position.x, throwing_DestinationCell.transform.position.y + 0.2f, throwing_DestinationCell.transform.position.z);
            EffectComponent ec = smokeTmp.AddComponent<EffectComponent>();
            ArrayList cellList = new ArrayList();
			cellList.Add(throwing_DestinationCell);
			if (throwing_DestinationCell.upperNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour);
			if (throwing_DestinationCell.lowerNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour);
			if (throwing_DestinationCell.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.leftNeighbour);
			if (throwing_DestinationCell.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.rightNeighbour);
			if (throwing_DestinationCell.upperNeighbour.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour.leftNeighbour);
			if (throwing_DestinationCell.upperNeighbour.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour.rightNeighbour);
			if (throwing_DestinationCell.lowerNeighbour.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour.leftNeighbour);
			if (throwing_DestinationCell.lowerNeighbour.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour.rightNeighbour);
            ec.zellenSetzen(cellList);
            ec.setEffekt(Enums.Effects.Smoke);
            ec.setDauer(effektDauer);
	}

	void molotovEffect()
	{
            GameObject fireTmp = Instantiate (fire);
	        fireTmp.transform.position = new Vector3 (throwing_DestinationCell.transform.position.x, throwing_DestinationCell.transform.position.y + 0.2f, throwing_DestinationCell.transform.position.z);
            EffectComponent ec = fireTmp.AddComponent<EffectComponent>();
            ArrayList cellList = new ArrayList();
			cellList.Add(throwing_DestinationCell);
			if (throwing_DestinationCell.upperNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour);
			if (throwing_DestinationCell.lowerNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour);
			if (throwing_DestinationCell.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.leftNeighbour);
			if (throwing_DestinationCell.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.rightNeighbour);
			if (throwing_DestinationCell.upperNeighbour.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour.leftNeighbour);
			if (throwing_DestinationCell.upperNeighbour.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour.rightNeighbour);
			if (throwing_DestinationCell.lowerNeighbour.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour.leftNeighbour);
			if (throwing_DestinationCell.lowerNeighbour.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour.rightNeighbour);
            ec.zellenSetzen(cellList);
            ec.setEffekt(Enums.Effects.Fire);
            ec.setDauer(effektDauer);
	}




    void grenadeEffect()
    {
            //Einsatz von AP durch Faehigkeit
            GameObject explosionTmp = Instantiate(explosion);
            explosionTmp.transform.position = new Vector3(throwing_DestinationCell.transform.position.x, throwing_DestinationCell.transform.position.y + 0.2f, throwing_DestinationCell.transform.position.z);
            EffectComponent ec = explosionTmp.AddComponent<EffectComponent>();
            ArrayList cellList = new ArrayList();
            cellList.Add(throwing_DestinationCell);
            if (throwing_DestinationCell.upperNeighbour != null)
                cellList.Add(throwing_DestinationCell.upperNeighbour);
            if (throwing_DestinationCell.lowerNeighbour != null)
                cellList.Add(throwing_DestinationCell.lowerNeighbour);
            if (throwing_DestinationCell.leftNeighbour != null)
                cellList.Add(throwing_DestinationCell.leftNeighbour);
            if (throwing_DestinationCell.rightNeighbour != null)
                cellList.Add(throwing_DestinationCell.rightNeighbour);
            if (throwing_DestinationCell.upperNeighbour.leftNeighbour != null)
                cellList.Add(throwing_DestinationCell.upperNeighbour.leftNeighbour);
            if (throwing_DestinationCell.upperNeighbour.rightNeighbour != null)
                cellList.Add(throwing_DestinationCell.upperNeighbour.rightNeighbour);
            if (throwing_DestinationCell.lowerNeighbour.leftNeighbour != null)
                cellList.Add(throwing_DestinationCell.lowerNeighbour.leftNeighbour);
            if (throwing_DestinationCell.lowerNeighbour.rightNeighbour != null)
                cellList.Add(throwing_DestinationCell.lowerNeighbour.rightNeighbour);
            ec.zellenSetzen(cellList);
            ec.setEffekt(Enums.Effects.Explosion);
            ec.setDauer(0);
    }

    void gasEffect()
    {       
            GameObject gasTmp = Instantiate(gas);
            gasTmp.transform.position = new Vector3(throwing_DestinationCell.transform.position.x, throwing_DestinationCell.transform.position.y + 0.2f, throwing_DestinationCell.transform.position.z);
            EffectComponent ec = gasTmp.AddComponent<EffectComponent>();
            ArrayList cellList = new ArrayList();
			cellList.Add(throwing_DestinationCell);
			if (throwing_DestinationCell.upperNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour);
			if (throwing_DestinationCell.lowerNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour);
			if (throwing_DestinationCell.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.leftNeighbour);
			if (throwing_DestinationCell.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.rightNeighbour);
			if (throwing_DestinationCell.upperNeighbour.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour.leftNeighbour);
			if (throwing_DestinationCell.upperNeighbour.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.upperNeighbour.rightNeighbour);
			if (throwing_DestinationCell.lowerNeighbour.leftNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour.leftNeighbour);
			if (throwing_DestinationCell.lowerNeighbour.rightNeighbour != null)
				cellList.Add(throwing_DestinationCell.lowerNeighbour.rightNeighbour);
            ec.zellenSetzen(cellList);
            ec.setEffekt(Enums.Effects.Gas);
            ec.setDauer(1);
    }

    public bool checkRotation(Cell targetCell, AttributeComponent playerAttr)
    {

        Cell currentCell = (Cell)playerAttr.getCurrentCell();

        //Richtung, in die der Pfad von currentCell zu  targetCell schaut.
        Vector3 walkingDirection = targetCell.transform.position - currentCell.transform.position;

        //Aktuelle Richtung in die wir schauen
        Vector3 facingDirection = playerAttr.transform.forward;

        //Winkel zwischen unseren zwei Vektoren
        float angle = Vector3.Angle(walkingDirection.normalized, facingDirection);

        //Schaue ich schon in die passende Richtung?
        if (angle != 0.0f)
        {
            if (!turning_Started)
            {
                if (Vector3.Cross(walkingDirection.normalized, facingDirection).y < 0.0f)
                {
                    turning_Direction = 1.0f;
                }
                else
                {
                    turning_Direction = -1.0f;
                }

                turning_Started = true;
            }

            float yRotation = Mathf.Clamp(Time.deltaTime * turning_Speed * turning_Direction, -angle, angle);
            angle += yRotation;
            Vector3 euler = playerAttr.transform.rotation.eulerAngles;
            euler.y += angle;
            playerAttr.transform.rotation = Quaternion.Euler(euler);
        }
        else
            turning_Started = false;

        return angle == 0.0f;
    }

    public void throwIt(GameObject grenade)
    {
        throwing_Object = grenade;
        throwing_StartPos = throwing_Object.transform.position;
        throwing_DestinationPos = throwing_DestinationCell.gameObject.transform.position;
        

        Vector3 baselength = throwing_DestinationPos - throwing_StartPos;
        baselength.y = 0.0f;

        throwing_TimeSum = 0.0f;
        throwing_TimeNeeded = throwing_TimePerCell * baselength.magnitude;

        throwing_Active = true;
        
    }
    void continousThrowing(float deltaTime)
    {
        if (throwing_TimeSum <= throwing_TimeNeeded)
        {
            throwing_TimeSum += deltaTime;
        }

        float progress = Mathf.Clamp01(throwing_TimeSum / throwing_TimeNeeded);
        float yValue = -20 * (progress * progress) + 20 * progress;

        Vector3 result = Vector3.Lerp(throwing_StartPos, throwing_DestinationPos, progress);
        result.y += yValue;
        throwing_Object.transform.position = result;
        if(progress == 1.0f)
        {
            grenadeReachedDestination();
        }


    }

    void grenadeReachedDestination()
    {
        switch(throwing_effect)
        {
            case Enums.Effects.Explosion:
                grenadeEffect();
                AudioManager.playGrenade();
                break;
            case Enums.Effects.Fire:
                molotovEffect();
                AudioManager.playMolotov();
                break;
            case Enums.Effects.Gas:
                gasEffect();
                break;
            case Enums.Effects.Smoke:
                smokeEffect();
                AudioManager.playSmoke();
                break;
        }
        throwing_Active = false;
        GameObject.Destroy(throwing_Object);
    }


    public void setThrowDestination(Cell destination)
    {
        throwing_DestinationCell = destination;
    }
}
