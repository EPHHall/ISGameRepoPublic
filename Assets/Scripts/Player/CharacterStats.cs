﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.UI;

namespace SS.Character
{
    public class CharacterStats : MonoBehaviour
    {
        //SS.Util.CharacterStatsInterface Interface;

        public int hp;
        public int mana;
        public int actionPoints;

        public int speed;

        public int hpMax;
        public int manaMax;
        public int actionPointsMax;

        public GameObject textTemp;
        public GameObject follower;
        public Transform followerParent;

        [Space(5)]
        [Header("For interacting with the Stat Card")]
        [TextArea(6, 6)]
        public string description;
        public Spells.Spell toDisplay;

        [Space(5)]
        [Header("For spawning ranges")]
        public GameObject moveTile;
        public GameObject abilityTile;

        [Space(5)]
        [Header("For Melee")]
        public Spells.Spell_Attack meleeAttack;

        [Space(5)]
        [Header("For Death")]
        public CharacterDeath characterDeath;

        [Space(5)]
        [Header("Dont Touch")]
        public List<Vector2> initialPositions;
        public List<Vector2> takenPositions;
        public CharacterFollower characterFollower;

        private void Awake()
        {
            hpMax = hp;
            manaMax = mana;
            actionPointsMax = actionPoints;
        }

        private void Start()
        {
            if (followerParent == null)
            {
                followerParent = GameObject.Find("Character Followers").transform;
            }

            characterFollower = Instantiate(follower, followerParent).GetComponent<CharacterFollower>();
            characterFollower.GetComponent<Follow>().toFollow = transform;
        }

        private void Update()
        {
            if (hp <= 0)
            {
                if (name == "Player")
                {
                    if (characterDeath == null)
                    {
                        DefaultDeath();
                    }
                    else
                    {
                        characterDeath.Death(this);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void DefaultDeath()
        {
            Debug.Log(name + " Died!!!");

            hp = hpMax;
        }

        public void ResetMana()
        {
            mana = manaMax;
        }

        public void ResetAP()
        {
            actionPoints = actionPointsMax;
        }

        public void ResetHealth()
        {
            hp = hpMax;
        }

        public void DisplayText(string message)
        {
            GameObject tt = Instantiate(textTemp, transform);
            tt.GetComponent<SS.UI.UpdateTextTemp>().SetMessage(message);
            tt.transform.position = tt.GetComponent<SS.UI.UpdateTextTemp>().localPos;
        }

        public void ShowRangeOfAbilities(Spells.Spell spell)
        {
            if (abilityTile == null || moveTile == null)
            {
                return;
            }

            if (spell != null)
            {
                List<Vector2>[] lists = Util.SpawnRange.SpawnMovementRange(transform.position, speed - 1, moveTile, moveTile, false);
                initialPositions = new List<Vector2>();
                takenPositions = new List<Vector2>();

                initialPositions = lists[0];
                takenPositions = lists[1];

                //List<Vector2> initialPositions = new List<Vector2>();
                //List<Vector2> takenPositions = new List<Vector2>();

                Util.SpawnRange.SpawnTargetingRange(initialPositions, takenPositions, spell.range, abilityTile, null);
            }
            else
            {
                Util.SpawnRange.SpawnMovementRange(transform.position, speed, moveTile, abilityTile, false);
            }
        }
    }
}
