using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetParameters
{
    public static float StrategyTime { get; } = 15;
    public static float CommonGameTime { get; } = 240;//4 minutes
    //Values in array are in order of increasing levels
    // {Level1, Level2, Level3, Level4}

    // Common to all 4 attacking ships
    public static int SmallShipMenCount { get; } = 2;
    public static int MediumShipMenCount { get; } = 4;
    public static int LargeShipMenCount { get; } = 6;

    // Archer Values
    public static float ArcherLineWidth { get; } = 0.01f;
    public static float ArcherArrowVelocity { get; } = 2.5f;
    public static float ArchersleastDistanceForStraightHit { get; } = 2f;
    public static float ArcherAdjustCurveAngle { get; } = 0.7f;

    // Gunmen Values
    public static float GunmanLineWidth { get; } = 0.01f;
    public static float GunmanBulletVelocity { get; } = 10f;

    // Cannon Values
    public static float CannonLineWidth { get; } = 0.02f;
    public static float CannonBallVelocity { get; } = 10f;
    public static float CannonShootAngleRange { get; } = 15f;

    // Mortar Values
    public static float MortarLineWidth { get; } = 0.07f;
    public static float MortarBombVelocity { get; } = 5f;
    public static float MortarAdjustCurveAngle { get; } = -0.7f;

    // Common Archer and Mortar Values
    public static int CurvePointsTotalCount { get; } = 20;

    // Ship Attributes on basis of Levels
    public static float[] ArcherWeaponRange { get; } = { 3f, 5f, 7f, 10f };
    public static float[] CannonWeaponRange { get; } = { 3f, 5f, 7f, 10f };
    public static float[] GunmanWeaponRange { get; } = { 3f, 5f, 7f, 10f };
    public static float[] MortarWeaponRange { get; } = { 3f, 5f, 7f, 10f };

    // Varying Ship Health
    public static int SupplyShipHealth { get; } = 140;
    public static int[] ArcherShipHealth { get; } = { 160, 220, 280, 340 };
    public static int[] CannonShipHealth { get; } = { 160, 220, 280, 340 };
    public static int[] GunmanShipHealth { get; } = { 160, 220, 280, 340 };
    public static int[] MortarShipHealth { get; } = { 160, 220, 280, 340 };

    // Varying Ship Men Health
    public static int[] ArcherShipMenHealth { get; } = { 160, 220, 280, 340 };
    public static int[] CannonShipMenHealth { get; } = { 160, 220, 280, 340 };
    public static int[] GunmanShipMenHealth { get; } = { 160, 220, 280, 340 };
    public static int[] MortarShipMenHealth { get; } = { 160, 220, 280, 340 };

    // Varying Weapon Damage Levels
    public static int[] ArcherWeaponDamage { get; } = { 7, 12, 15, 20 };
    public static int[] CannonWeaponDamage { get; } = { 7, 12, 15, 20 };
    public static int[] GunmanWeaponDamage { get; } = { 7, 12, 15, 20 };
    public static int[] MortarWeaponDamage { get; } = { 7, 12, 15, 20 };

    // Weapon Reload Speed
    public static float[] ArcherWaitBeforeShootAiming { get; } = { 4f, 3.5f, 3f, 2.5f };
    public static float[] ArcherWaitAfterShoot { get; } = { 4f, 3.5f, 3f, 2.5f };

    public static float[] GunmanWaitBeforeShootAiming { get; } = { 4f, 3.5f, 3f, 2.5f };
    public static float[] GunmanWaitAfterShoot { get; } = { 4f, 3.5f, 3f, 2.5f };

    public static float[] CannonWaitBeforeShootAiming { get; } = { 4f, 3.5f, 3f, 2.5f };
    public static float[] CannonWaitAfterShoot { get; } = { 4f, 3.5f, 3f, 2.5f };

    public static float[] MortarWaitBeforeShootAiming { get; } = { 4f, 3.5f, 3f, 2.5f };
    public static float[] MortarWaitAfterShoot { get; } = { 4f, 3.5f, 3f, 2.5f };

    // Weapon Ammunition
    // Ammo is total no of players times no of projectiles
    // Small, Medium, Large, in multiples of 2,4 and 6.
    public static int[] ArcherWeaponMaxAmmo { get; } = { 20, 40, 60 };
    public static int[] GunmanWeaponMaxAmmo { get; } = { 20, 40, 60 };
    public static int[] CannonWeaponMaxAmmo { get; } = { 20, 40, 60 };
    public static int[] MortarWeaponMaxAmmo { get; } = { 20, 40, 60 };

    //Ship Cost(Based on 3 factors: ShipType + ShipLevel + ShipSize)

    //Ship Cost based on ship type
    public static int[] ArcherShipCost { get; } = { 40, 80, 120, 160 };
    public static int[] GunmanShipCost { get; } = { 60, 100, 140, 180 };
    public static int[] CannonShipCost { get; } = { 80, 120, 160, 200 };
    public static int[] MortarShipCost { get; } = { 100, 140, 180, 220 };

    //Ship Cost based on ship size
    public static int SmallShipCost { get; } = 50;
    public static int MediumShipCost { get; } = 100;
    public static int LargeShipCost { get; } = 200;

    // Ship Rotation Speed to align to the enemy ship
    public static float[] ShipRotationSpeed { get; } = { 0.2f, 0.5f, 1f, 2f };
    public static float[] ShipSpeed { get; } = { 1.05f, 1.10f, 1.15f, 1.20f };

    //Max Building Health
    public static int BuildingHealth { get; } = 500;
}
