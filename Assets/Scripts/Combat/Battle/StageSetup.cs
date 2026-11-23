using UnityEngine;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
public class StageSetup : MonoBehaviour
{
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private CharacterVisuals visuals;
    [SerializeField] private PartyLoadout party;
    [SerializeField] private StageEncounter encounter;
    [SerializeField] private List<CombatStats> allySlots = new List<CombatStats>();
    [SerializeField] private List<CombatStats> enemySlots = new List<CombatStats>();

    private void Awake()
    {
        if (database == null || party == null || encounter == null)
        {
            Debug.LogError($"{name}: database, party or encounter not assigned.", this);
            return;
        }

        FillSlots(allySlots, party.Members, BattleSide.Player);
        FillSlots(enemySlots, encounter.Enemies, BattleSide.Enemy);
    }

    private void FillSlots(List<CombatStats> slots, IReadOnlyList<UnitEntry> entries, BattleSide side)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            CombatStats slot = slots[i];

            if (slot == null)
            {
                continue;
            }

            bool hasEntry = i < entries.Count && !entries[i].IsEmpty;

            if (!hasEntry || !slot.Initialize(database, entries[i].CharacterId, entries[i].Level, side))
            {
                slot.gameObject.SetActive(false);
                continue;
            }

            AttachVisual(slot, entries[i].CharacterId);
        }
    }

    private void AttachVisual(CombatStats slot, string characterId)
    {
        GameObject prefab = visuals != null ? visuals.FindPrefab(characterId) : null;

        if (prefab == null)
        {
            return;
        }

        GameObject visual = Instantiate(prefab, slot.transform);
        visual.transform.localPosition = Vector3.zero;

        foreach (Rigidbody2D body in visual.GetComponentsInChildren<Rigidbody2D>())
        {
            Destroy(body);
        }

        foreach (Collider2D collider in visual.GetComponentsInChildren<Collider2D>())
        {
            Destroy(collider);
        }

        if (slot.GetComponent<CombatAnimator>() == null)
        {
            slot.gameObject.AddComponent<CombatAnimator>();
        }

        SpriteRenderer placeholder = slot.GetComponent<SpriteRenderer>();

        if (placeholder != null)
        {
            placeholder.enabled = false;
        }
    }   
}
