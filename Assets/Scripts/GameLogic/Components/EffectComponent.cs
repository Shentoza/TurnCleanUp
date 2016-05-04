using UnityEngine;
using System.Collections;

public class EffectComponent : MonoBehaviour {

    ArrayList betroffeneZellen;
    Enums.Effects effekt;
    int dauer;
    int momentaneRunde;
    public ManagerSystem ms;
    public HealthSystem hs;

    // Use this for initialization
    void Awake() {
        ms = (ManagerSystem) GameObject.Find("Manager").GetComponent(typeof(ManagerSystem));
        hs = (HealthSystem) GameObject.Find("Manager").GetComponent(typeof(HealthSystem));
    }

    // Update is called once per frame
    void Update() {
        if (dauer == ms.rounds)
        {
            if (effekt == Enums.Effects.Fire)
            {
                foreach (Cell c in betroffeneZellen)
                {
                    c.setOnFire = false;
                }
                ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
                ps.loop = false;
                if (ps.particleCount == 0)
                {
                    Destroy(this.gameObject);
                }
            }
            if (effekt == Enums.Effects.Smoke)
            {
                foreach (Cell c in betroffeneZellen)
                {
                    c.smoked = false;
                }
                ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
                ps.loop = false;
                if (ps.particleCount == 0)
                {
                    Destroy(this.gameObject);
                }
            }
            if (effekt == Enums.Effects.Gas)
            {
                foreach (Cell c in betroffeneZellen)
                {
                    c.setOnGas = false;
                }
                ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
                ps.loop = false;
                if (ps.particleCount == 0)
                {
                    Destroy(this.gameObject);
                }
            }
            if (effekt == Enums.Effects.Explosion)
            {
                ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
                if (!ps.isPlaying)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        if(momentaneRunde != ms.rounds)
        {
            if (effekt == Enums.Effects.Fire)
            {
                foreach (Cell c in betroffeneZellen)
                {
                    if (c.isOccupied)
                    {
                        if (c.objectOnCell.tag == "FigurSpieler1" || c.objectOnCell.tag == "FigurSpieler2")
                        {
                            hs.inflictFireDamage((AttributeComponent)c.objectOnCell.GetComponent(typeof(AttributeComponent)));
                        }
                    }
                }
            }
            if (effekt == Enums.Effects.Gas)
            {
                foreach (Cell c in betroffeneZellen)
                {
                    if (c.isOccupied)
                    {
                        if (c.objectOnCell.tag == "FigurSpieler1" || c.objectOnCell.tag == "FigurSpieler2")
                        {
                            hs.inflictGasDamage((AttributeComponent)c.objectOnCell.GetComponent(typeof(AttributeComponent)));
                        }
                    }
                }
            }
            momentaneRunde = ms.rounds;
        }
    }

    public void zellenSetzen(ArrayList zellen)
    {
        betroffeneZellen = zellen;
    }

    public void setEffekt(Enums.Effects effect)
    {
        effekt = effect;

        if (effekt == Enums.Effects.Fire)
        {
            foreach (Cell c in betroffeneZellen)
            {
                c.setOnFire = true;
                if (c.isOccupied)
                {
                    if (c.objectOnCell.tag == "FigurSpieler1" || c.objectOnCell.tag == "FigurSpieler2")
                    {
                        hs.inflictFireDamage((AttributeComponent)c.objectOnCell.GetComponent(typeof(AttributeComponent)));
                    }
                }
            }
        }
        if (effekt == Enums.Effects.Smoke)
        {
            foreach (Cell c in betroffeneZellen)
            {
                c.smoked = true;
            }
        }
        if (effekt == Enums.Effects.Gas)
        {
            foreach (Cell c in betroffeneZellen)
            {
                c.setOnGas = true;
                if (c.isOccupied)
                {
                    if (c.objectOnCell.tag == "FigurSpieler1" || c.objectOnCell.tag == "FigurSpieler2")
                    {
                        hs.inflictGasDamage((AttributeComponent)c.objectOnCell.GetComponent(typeof(AttributeComponent)));
                    }
                }
            }
        }
        if (effekt == Enums.Effects.Explosion)
        {
            foreach (Cell c in betroffeneZellen)
            {
                if(c.isOccupied)
                {
                    if (c.objectOnCell.tag == "FigurSpieler1" || c.objectOnCell.tag == "FigurSpieler2")
                    {
                        hs.inflictGrenadeDamage((AttributeComponent)c.objectOnCell.GetComponent(typeof(AttributeComponent)));
                    }
                }
            }
        }
    }

    public void setDauer(int time)
    {
        dauer = time;
        dauer += ms.rounds;
        momentaneRunde = ms.rounds;
    }
}
