using UnityEngine;
using UnityEngine.UI;


public class Entity : MonoBehaviour
{
    protected Animator m_Animator;
    protected SpriteRenderer m_Renderer;

    public int m_MaxHealth = 5;
    public float m_CurrentHealth;
    public int m_AttackPower = 2;
    public Image[] playerHeartsUI;


    protected virtual void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        m_CurrentHealth = m_MaxHealth;
    }


    public virtual void PlayDeathAnimation()
    {
        if (m_Animator)
            m_Animator.SetTrigger("Die");
    }

    public virtual void DestroyEntity()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
	{
        m_CurrentHealth -= damage;

        // check if we're dealing with player
        if(gameObject.tag == "Player")
        {
            UpdatePlayerHealthUI();

            if (m_CurrentHealth <= 0)
                PlayDeathAnimation();
        }
        else if(m_CurrentHealth <= 0)
        {
            // destroy object as they dead
            DestroyEntity();
        }

	}

    public virtual void AddHealth (float HealthToAdd)
    {
        m_CurrentHealth += HealthToAdd;

        if (m_CurrentHealth > m_MaxHealth)
        {
            m_CurrentHealth = m_MaxHealth;
            //Debug.Log(m_CurrentHealth);
            UpdatePlayerHealthUI();
        }

        UpdatePlayerHealthUI();
    }

    public virtual void UpdatePlayerHealthUI()
    {
        for(int i = 0; i < m_MaxHealth; i++)
        {
            if(i <= m_CurrentHealth - 1)
            {
                playerHeartsUI[i].enabled = true;
                //Debug.Log("Show Hearts");
            }
            else
            {
                playerHeartsUI[i].enabled = false;  
            }
        }
    }
}
