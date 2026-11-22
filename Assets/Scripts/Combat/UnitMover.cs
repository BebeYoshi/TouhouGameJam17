using UnityEngine;

public static class UnitMover
{
    public static void TeleportTo(CombatStats unit, Vector2 destination)
    {
        Rigidbody2D body = unit.GetComponent<Rigidbody2D>();

        if (body != null)
        {
            body.position = destination;
            body.linearVelocity = Vector2.zero;
        }

        unit.transform.position = new Vector3(destination.x, destination.y, unit.transform.position.z);
    }
}
