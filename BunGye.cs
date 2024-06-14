using Org.BouncyCastle.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunGye : ActiveSkill
{
    private float attackDelay = 0.0f;

    public BunGye(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        List<Collider2D> inRangeHits = new List<Collider2D>();
        float activateTime = 0.0f;
        while (activateTime < skillData.duration)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(shooter.position, skillData.attackDistance);
            inRangeHits.Clear();
            if (hit != null)
            {
                foreach (Collider2D collider in hit)
                {
                    inRangeHits.Add(collider);
                }
            }
            List<Collider2D> outOfRangeHits = new List<Collider2D>(hit);
            foreach (Collider2D collider in inRangeHits)
            {
                outOfRangeHits.Remove(collider);
            }
            foreach (Collider2D collider in inRangeHits)
            {
                if (collider.TryGetComponent(out Monster monster))
                {
                    if(attackDelay>=0.5f)
                    {
                        monster.Hit(skillData.damage);
                        attackDelay= 0.0f;
                    }
                    else
                    {
                        attackDelay += Time.fixedDeltaTime;
                    }
                }
            }
            activateTime += Time.fixedDeltaTime;
            yield return frame;
        }
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
}
