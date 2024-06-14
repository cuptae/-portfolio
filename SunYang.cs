using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SunYang : ActiveSkill
{
    private Projectile[] projectiles;

    private Vector2 look;

    public SunYang(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        projectiles = new Projectile[skillData.projectileCount];
    for (int i = 0; i < projectiles.Length; i++)
    {
        projectiles[i] = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        if (shooter.TryGetComponent(out Player player))
        {
            look = player.lookDirection;
        }

        float projectileSpacing = 5.0f;
        if(Mathf.Abs(look.x)>0)
        {
            look.y = 0;
            projectiles[i].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if(Mathf.Abs(look.y)>0)
        {
            look.x = 0;
            projectiles[i].transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        float offsetY = projectileSpacing * (i - (projectiles.Length - 1) * 0.5f);
        Vector2 offset = new Vector2(look.y, -look.x) * offsetY;
        projectiles[i].transform.localPosition = look * skillData.attackDistance + offset;

    }
        yield return Move();
    }
    private IEnumerator Move()
    {
        Projectile projectile1 = projectiles[0];
        Projectile projectile2 = projectiles[1];
        projectile1.transform.localScale = new Vector3(1.0f, projectile1.transform.localScale.y, projectile1.transform.localScale.z);
        projectile2.transform.localScale = new Vector3(1.0f, projectile2.transform.localScale.y, projectile1.transform.localScale.z);
        float angle = 0.0f;
        float weight = 0.0f;
        float time = 0.0f;
        do
        {
            weight += skillData.speed / 100;
            angle -= Time.fixedDeltaTime * skillData.speed + weight;
            projectile1.transform.RotateAround(shooter.position, Vector3.forward, angle);
            projectile2.transform.RotateAround(shooter.position, Vector3.back, angle);
            time += Time.fixedDeltaTime;
            if(time>=1.0f)
            {
                break;
            }
            yield return frame;
        } while (Vector2.Distance(projectile1.transform.position, projectile2.transform.position) > 0.3f);
        for(int i = 0; i < projectiles.Length; i++)
        {
            SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        }       
    }
}