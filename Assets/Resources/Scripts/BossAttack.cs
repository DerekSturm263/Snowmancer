using System;
using UnityEngine;

public class BossAttack
{
    public Boss UserBoss;

    public enum BossUser
    {
        Fire_Boss, Electric_Boss, Wind_Boss, Final_Boss
    }

    public Enemy.ElementType Type;

    public float ChargeTime;
    public float Damage;
    public float Size;
    public float Speed;
    public float LifeTime;

    public Action AttackAction;

    public LongRangedAttack Spell;

    public BossAttack(Enemy.ElementType type, float chargeTime, float damage, float size, float speed, float lifeTime, BossUser user)
    {
        Type = type;

        ChargeTime = chargeTime;
        Damage = damage;
        Size = size;
        Speed = speed;
        LifeTime = lifeTime;

        switch (user)
        {
            case BossUser.Fire_Boss:
                BossAttacks.fireBossAttacks.Add(this);
                break;

            case BossUser.Electric_Boss:
                BossAttacks.electricBossAttacks.Add(this);
                break;

            case BossUser.Wind_Boss:
                BossAttacks.windBossAttacks.Add(this);
                break;

            case BossUser.Final_Boss:
                BossAttacks.finalBossAttacks.Add(this);
                break;
        }
    }

    public void SetUser(Boss user)
    {
        UserBoss = user;
    }

    public void SetAction(Action action)
    {
        AttackAction = action;
    }
}
