﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    public int turns = 3;
    [SerializeField] private float _time = 3f;
    public int playerTurn = 1;
    public static int oraclesPlaced = 0;
    public GameObject canvas;
    public GameObject canvas2;
    public GameObject canvas3;
    private bool waiting = false;
    public static List<GameObject> observedObjects = new List<GameObject>();
    private bool defenderPostTurn = false;
    public Material pipe1Material;
    public Material pipe2Material;

    public static int attacksSelected = 0;
    private bool attackerPostTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false); 
        canvas2.SetActive(false);
        canvas3.SetActive(false);
        AttackerTurnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(turns <= 0)
        {
            canvas3.SetActive(true);
        }
        if (oraclesPlaced == 2 && defenderPostTurn == false)
        {
            if (!canvas.activeSelf)
            {
                canvas.SetActive(true);
            }
        }
        else if (attacksSelected == 1 && attackerPostTurn == false)
        {
            if(!canvas.activeSelf)
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
        }
    }
    public void AttackerTurnStart()
    {
        playerTurn--;
        print("Attacker turn starting...");
        print("Turns left: " + turns);

        canvas.SetActive(false);
        canvas2.SetActive(false);
        defenderPostTurn = false;
        attackerPostTurn = false;
        attacksSelected = 0;
        oraclesPlaced = 0;
        observedObjects.Clear();

        GameObject[] sidewaysPipes;
        GameObject[] forwardPipes;
        sidewaysPipes = GameObject.FindGameObjectsWithTag("SidewaysPipe");
        forwardPipes = GameObject.FindGameObjectsWithTag("ForwardPipe");
        foreach(GameObject pipe in sidewaysPipes)
        {
            var changeMaterial = pipe.GetComponent<Renderer>();
            if (pipe.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material.SetColor("_Color", Color.red);
            }
            else
            {
                changeMaterial.material = pipe2Material;
            }
        }
        foreach (GameObject pipe in forwardPipes)
        {
            var changeMaterial = pipe.GetComponent<Renderer>();
            if (pipe.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material.SetColor("_Color", Color.red);
            }
            else
            {
                changeMaterial.material = pipe1Material;
            }
        }

        GameObject[] oracles;
        oracles = GameObject.FindGameObjectsWithTag("Oracle");

        foreach(GameObject destroy in oracles)
        {
            Destroy(destroy);
        }
    }


    private void DefenderTurnStart()
    {
        playerTurn++;
        defenderPostTurn = false;
        attackerPostTurn = false;
        print("Defender turn starting...");
        canvas.SetActive(false);
        canvas2.SetActive(false);
        attacksSelected = 0;
        oraclesPlaced = 0;
        observedObjects.Clear();


        GameObject[] sidewaysPipes;
        GameObject[] forwardPipes;
        sidewaysPipes = GameObject.FindGameObjectsWithTag("SidewaysPipe");
        forwardPipes = GameObject.FindGameObjectsWithTag("ForwardPipe");
        foreach (GameObject pipe in sidewaysPipes)
        {
            var changeMaterial = pipe.GetComponent<Renderer>();
            if (pipe.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material = pipe2Material;
            }
        }
        foreach (GameObject pipe in forwardPipes)
        {
            var changeMaterial = pipe.GetComponent<Renderer>();
            if (pipe.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material = pipe1Material;
            }
        }
    }

    public void ResetSelection()
    {
        if (playerTurn == 0)
        {
            ResetAttackerTurn();
        }
        if (playerTurn == 1)
        {
            ResetDefenderTurn();
        }
    }

    private void AttackerPost()
    {
        canvas.SetActive(false);
        canvas2.SetActive(true);

        foreach (GameObject tracked in observedObjects)
        {
            var changeMaterial = tracked.GetComponent<Renderer>();
            tracked.GetComponent<PipeClick>().flow = 0;

        }

        GameObject[] sidewaysPipes;
        GameObject[] forwardPipes;
        sidewaysPipes = GameObject.FindGameObjectsWithTag("SidewaysPipe");
        forwardPipes = GameObject.FindGameObjectsWithTag("ForwardPipe");
        foreach (GameObject pipe in sidewaysPipes)
        {
            var changeMaterial = pipe.GetComponent<Renderer>();
            if (pipe.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material.SetColor("_Color", Color.red);
            }
        }
        foreach (GameObject pipe in forwardPipes)
        {
            var changeMaterial = pipe.GetComponent<Renderer>();
            if (pipe.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material.SetColor("_Color", Color.red);
            }
        }
    }

    public void DefenderPost()
    {
        defenderPostTurn = true;
        foreach (GameObject tracked in observedObjects)
        {
            var changeMaterial = tracked.GetComponent<Renderer>();
            if(tracked.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material.SetColor("_Color", Color.red);
            }
            else
            {
                changeMaterial.material.SetColor("_Color", Color.green);
            }
        }
        canvas.SetActive(false);
        Invoke("FixObjects", _time);
    }

    private void FixObjects()
    {
        foreach (GameObject tracked in observedObjects)
        {
            var changeMaterial = tracked.GetComponent<Renderer>();
            if (tracked.GetComponent<PipeClick>().flow == 0)
            {
                changeMaterial.material.SetColor("_Color", Color.green);
                tracked.GetComponent<PipeClick>().flow = 1;
            }
        }
        canvas2.SetActive(true);

    }

    public void ResetDefenderTurn()
    {
        foreach (GameObject tracked in observedObjects)
        {
            var changeMaterial = tracked.GetComponent<Renderer>();
            if(tracked.tag == "SidewaysPipe")
            {
                changeMaterial.material = pipe2Material;
            }
            else if (tracked.tag == "ForwardPipe")
            {
                changeMaterial.material = pipe1Material;
            }
            GameObject[] oracles;
            oracles = GameObject.FindGameObjectsWithTag("Oracle");

            foreach(GameObject destroy in oracles)
            {
                Destroy(destroy);
            }
            canvas.SetActive(false);
            oraclesPlaced = 0;
        }
    }
    public void ResetAttackerTurn()
    {
        canvas.SetActive(false);
        foreach (GameObject tracked in observedObjects)
        {
            var changeMaterial = tracked.GetComponent<Renderer>();
            if (tracked.tag == "SidewaysPipe" && tracked.GetComponent<PipeClick>().flow == 1)
            {
                changeMaterial.material = pipe2Material;
            }
            else if (tracked.tag == "ForwardPipe" && tracked.GetComponent<PipeClick>().flow == 1)
            {
                changeMaterial.material = pipe1Material;
            }
            attacksSelected = 0;
        }
    }

    public void EnterPostTurn()
    {
        if (playerTurn == 0)
        {
            attackerPostTurn = true;
            canvas.SetActive(false);
            AttackerPost();
        }
        if (playerTurn == 1)
        {
            defenderPostTurn = true;
            canvas.SetActive(false);
            DefenderPost();
        }
    }

    public void EndTurn()
    {
        if (playerTurn == 0)
        {
            DefenderTurnStart();
        }
        else if (playerTurn == 1)
        {
            turns--;
            AttackerTurnStart();

        }
    }

}
