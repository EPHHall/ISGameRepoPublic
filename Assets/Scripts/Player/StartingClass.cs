﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.Spells;
using SS.UI;
using SS.Item;
using System;

namespace SS.Character
{
    public class StartingClass : MonoBehaviour
    {
        [Header("Fill In - General")]
        public string className;

        [Space(5)]
        [Header("Fill In - To Modify")]
        public CharacterStats characterStats;
        public Spell_Attack spellAttack;
        public Spell spellToModify;
        public SpellInventory spellInventory;
        public SpellCraftingScreen spellCraftingScreen;

        [Space(5)]
        [Header("Fill In - Class Stats")]
        public int newHP;
        public int newMana;
        public int newSpeed;

        [Space(5)]
        [Header("Fill In - Class Equipment")]
        public Weapon.WeaponTemplate weaponToGrant;

        [Space(5)]
        [Header("Fill In - Class Spell")]
        public GameObject mainEffectForNewSpell;
        public List<GameObject> secondaryEffectsForNewSpell;
        public List<GameObject> modifiersForNewSpell;
        public string newSpellName;

        public void ApplyClass()
        {
            ApplyStats();

            ApplyEquipment();

            ApplySpell();
        }

        private void ApplyStats()
        {
            characterStats.SetMaxHealth(newHP);
            characterStats.SetMaxMana(newMana);
            characterStats.SetSpeed(newSpeed);

            characterStats.ResetHealth();
            characterStats.ResetMana();
        }

        private void ApplyEquipment()
        {
            spellAttack.RemoveAndDestroyWeapons();

            weaponToGrant.attack = spellAttack;
            Weapon newWeapon = Weapon.CreateWeapon(weaponToGrant);

            spellAttack.weaponBeingUsed = spellAttack.activeWeapons.IndexOf(newWeapon);
        }

        private void ApplySpell()
        {
            spellInventory.RemoveAndDestroyContents();
            spellCraftingScreen.ResetFunctionalFrames();

            GameObject mainEffect = Instantiate(mainEffectForNewSpell, spellInventory.inventoryParent);

            spellInventory.AddEffect(0, mainEffect.GetComponent<Effect>());
            spellCraftingScreen.mainFrame.SetContent(mainEffect.GetComponent<Effect>());

            for(int i = 0; i < secondaryEffectsForNewSpell.Count; i++)
            {
                GameObject secondaryEffect = Instantiate(secondaryEffectsForNewSpell[i], spellInventory.inventoryParent);

                spellInventory.AddEffect(i + 1, secondaryEffect.GetComponent<Effect>());
                spellCraftingScreen.secondaryFrames[i].SetContent(secondaryEffect.GetComponent<Effect>());
            }
            for (int i = 0; i < modifiersForNewSpell.Count; i++)
            {
                GameObject modifier = Instantiate(modifiersForNewSpell[i], spellInventory.inventoryParent);

                spellInventory.AddModifier(i + 1, modifier.GetComponent<Modifier>());
                spellCraftingScreen.modifierFrames[i].SetContent(modifier.GetComponent<Modifier>());
            }

            spellCraftingScreen.spell.spellName = newSpellName;
            spellCraftingScreen.spell.name = newSpellName;

            spellCraftingScreen.ResetScreen();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<CharacterStats>() == characterStats)
            {
                ApplyClass();
            }
        }
    }
}