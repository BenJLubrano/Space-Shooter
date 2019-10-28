using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : Ship
{
    [Header("Weak Point")]
    [SerializeField] protected BossController owner;
    [SerializeField] protected WeakPointZone zone;
    [SerializeField] protected float damageToOwner;
    [SerializeField] protected Sprite damagedVersion;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoUpdateChecks();
    }

    public override bool CanBeHitBy(int id)
    {
        return zone.IsInZone(id);
    }

    protected override void PrepareForDeath()
    {
        CreateExplosion();
        spriteRenderer.sprite = damagedVersion;
        shipCollider.enabled = false;
        owner.TakeDamage(damageToOwner);
        audioSource.clip = deathSound;
        audioSource.Play();
    }

    protected override void Die()
    {
        //do nothing
    }
}
