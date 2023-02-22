using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public const int MAX_STATE = 2;

    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    public class Heart
    {
        // represent single heart

        private int states;

        public Heart(int states)
        {
            this.states = states;
        }

        public int GetStates()
        {
            return states;
        }
        
        public void SetStates(int states)
        {
            this.states = states;
        }

        public void Damage(int damageAmount)
        {
            if (damageAmount >= states)
            {
                states = 0;
            }

            else
            {
                states -= damageAmount;
            }
        }

        public void Heal(int healAmount)
        {
            if (states + healAmount > MAX_STATE)
            {
                states = MAX_STATE;
            }
            else
            {
                states += healAmount;
            }
        }
    }

    private List<Heart> heartList;
    public Health(int heartAmount)
    {
        heartList = new List<Heart>();
        for (int i = 0; i < heartAmount; i++)
        {
            // default state = 2;
            Heart heart = new Heart(2);
            heartList.Add(heart);
        }
    }
    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public void Damage(int damageAmount)
    {
        // loop through all hearts from start to end
        for (int i = heartList.Count - 1; i >= 0; i--)
        {
            Heart heart = heartList[i];

            // test if current heart can take damageAmount
            if (damageAmount > heart.GetStates())
            {
                // damage current heart and keep going
                damageAmount -= heart.GetStates();
                heart.Damage(heart.GetStates());
            }

            else
            {
                // damage current heart and break
                heart.Damage(damageAmount);
                break;
            }
        }

        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

        if (IsDead())
        {
            if (OnDead != null) OnDead(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            Heart heart = heartList[i];
            int healable = MAX_STATE - heart.GetStates();

            if (healAmount > healable)
            {
                healAmount -= healable;
                heart.Heal(healable);
            }
            else
            {
                heart.Heal(healAmount);
                break;
            }
        }

        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return heartList[0].GetStates() == 0;
    }
}
