using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatVisualManager : MonoBehaviour
{
    [Header("Hull and Flag")][Space(5)]
    public List<Sprite> HullList = new List<Sprite>();
    public List<Sprite> FlagList = new List<Sprite>();
    [Space(10)]

    [Header("Large Sails")][Space(5)]
    public List<Sprite> WhiteSailLargeList = new List<Sprite>();
    public List<Sprite> BlackSailLargeList = new List<Sprite>();
    public List<Sprite> RedSailLargeList = new List<Sprite>();
    [Space(10)]

    [Header("Small Sails")][Space(5)]
    public List<Sprite> WhiteSailSmallList = new List<Sprite>();
    public List<Sprite> BlackSailSmallList = new List<Sprite>();
    public List<Sprite> RedSailSmallList = new List<Sprite>();
    [Space(10)]

    [Header("Particles")][Space(5)]
    public List<Sprite> CrewList = new List<Sprite>();
    public List<Sprite> WoodList = new List<Sprite>();
    [Space(10)]

    private List<List<Sprite>> _sailLargeList = new List<List<Sprite>>();
    private List<List<Sprite>> _sailSmallList = new List<List<Sprite>>();
    private ParticleSystem _boatComponenetsParticles;
    private ParticleSystem _boatHullParticles;

    void Awake()
    {
        // Stack the sails lists so it becomes a matrix.
        _sailLargeList.Add(WhiteSailLargeList);
        _sailLargeList.Add(BlackSailLargeList);
        _sailLargeList.Add(RedSailLargeList);

        _sailSmallList.Add(WhiteSailSmallList);
        _sailSmallList.Add(BlackSailSmallList);
        _sailSmallList.Add(RedSailSmallList);

        _boatComponenetsParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        _boatHullParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    // Damages the boat visually by stages, based on its color and current state.
    public void ChangeVisual(GameObject boat, int state)
    {
        int colorTagValue = 0;
        Transform hullLarge = boat.transform.GetChild(0).Find("HullLarge");
        Transform sailLarge = boat.transform.GetChild(0).Find("SailLarge");
        Transform sailSmall = boat.transform.GetChild(0).Find("SailSmall");

        switch (boat.tag)
        {
            case "White":
                colorTagValue = 0;
            break;
            case "Black":
                colorTagValue = 1;
            break;
            case "Red":
                colorTagValue = 2;
            break;
        }

        switch(state)
        {
            // Sets the default boat colors based on its tag.
            case 0:
                sailLarge.GetComponent<SpriteRenderer>().sprite = _sailLargeList[colorTagValue][state];
                sailSmall.GetComponent<SpriteRenderer>().sprite = _sailSmallList[colorTagValue][state];
                boat.transform.GetChild(0).Find("Flag").GetComponent<SpriteRenderer>().sprite = FlagList[colorTagValue];
            break;
            // Visually damages the boat;
            case 1: 
                hullLarge.GetComponent<SpriteRenderer>().sprite = HullList[state];

                sailLarge.GetComponent<SpriteRenderer>().sprite = _sailLargeList[colorTagValue][state];
                sailSmall.GetComponent<SpriteRenderer>().sprite = _sailSmallList[colorTagValue][state];

                EmitParticle(boat.transform, CrewList[Random.Range(0, CrewList.ToArray().Length)], Random.Range(1, 3), .4f, .4f);

            break;
            // Destroys the small sail and visually damages the boat.
            case 2:
                hullLarge.GetComponent<SpriteRenderer>().sprite = HullList[state];
                sailLarge.GetComponent<SpriteRenderer>().sprite = _sailLargeList[colorTagValue][state];
                sailSmall.GetComponent<SpriteRenderer>().sprite = _sailSmallList[colorTagValue][state];

                EmitParticle(sailSmall.transform, sailSmall.GetComponent<SpriteRenderer>().sprite, 1, .65f, .12f);
                sailSmall.gameObject.SetActive(false);

                // Launches the crew.
                EmitParticle(boat.transform, CrewList[Random.Range(0, CrewList.ToArray().Length)], Random.Range(2, 4), .4f, .4f);

            break;
            // Destroys the original game object and launches the sail, hull and flag to the sea.
            case 3:

                hullLarge.GetComponent<SpriteRenderer>().sprite = HullList[state];
                sailLarge.GetComponent<SpriteRenderer>().sprite = _sailLargeList[colorTagValue][state];

                Transform boatFlag = boat.transform.GetChild(0).Find("Flag");
                Transform boatNest = boat.transform.GetChild(0).Find("Nest");

                EmitParticle(sailLarge.transform, sailLarge.GetComponent<SpriteRenderer>().sprite, 1, 1, .65f);
                EmitParticle(boatNest.transform, boatNest.GetComponent<SpriteRenderer>().sprite, 1, .3f, .28f);
                EmitParticle(boatFlag.transform, boatFlag.GetComponent<SpriteRenderer>().sprite, 1, .1f, .35f);

                EmitParticle(boat.transform, hullLarge.GetComponent<SpriteRenderer>().sprite, 1, .8f, 1.7f);
                

                // Launches the crew.
                EmitParticle(boat.transform, CrewList[Random.Range(0, CrewList.ToArray().Length)], Random.Range(2, 4), .4f, .4f);

                Destroy(boat.transform.gameObject);
            break;
        }
        
        if (state == 0)
        {
            return;
        }
        
        // Throw randomly some woods.
        for (int i = 0; i < 4; i++)
        {
            Sprite chosenWoodSprite = WoodList[0];
            float sizeX = 0;
            float sizeY = 0;

            switch (Random.Range(0, 4))
            {
                case 0:
                    chosenWoodSprite = WoodList[0];
                    sizeX = .2f;
                    sizeY = .1f;
                break;
                case 1:
                    chosenWoodSprite = WoodList[1];
                    sizeX = .4f;
                    sizeY = .15f;
                break;
                case 2:
                    chosenWoodSprite = WoodList[2];
                    sizeX = .23f;
                    sizeY = .12f;
                break;
                case 3:
                    chosenWoodSprite = WoodList[3];
                    sizeX = .4f;
                    sizeY = .1f;
                break;
            }

            EmitParticle(boat.transform, chosenWoodSprite, 1, sizeX, sizeY);
        }
    }

    // Emmits a particle with the desired sprite, position and size.
    private void EmitParticle(Transform anchorObject, Sprite sprite, int amount, float sizeX, float sizeY)
    {
        ParticleSystem tempParticles;

        // The Hull has a slightly different particleSystem, so it's separate.
        if (sprite.name.Contains("hull"))
        {
            tempParticles = Instantiate(
                _boatHullParticles.gameObject,
                anchorObject.position,
                anchorObject.rotation).GetComponent<ParticleSystem>();
        }
        else
        {
            tempParticles = Instantiate(
                _boatComponenetsParticles.gameObject,
                anchorObject.position,
                anchorObject.rotation).GetComponent<ParticleSystem>();
        }

        var particlesMain = tempParticles.main;
        particlesMain.startSizeX = sizeX;
        particlesMain.startSizeY = sizeY;

        // Gets the current rotation of the boat, so the object starts rotatin from its actual start point.
        particlesMain.startRotation = -(anchorObject.transform.eulerAngles.z + 180) * Mathf.Deg2Rad;

        tempParticles.GetComponent<ParticleSystemRenderer>().material.mainTexture = sprite.texture;
        tempParticles.Emit(amount);
    }
}