using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkSung : ActiveSkill
{
    private List<Projectile> projectiles = new List<Projectile>();
    //private List<Transform> allTargets = new List<Transform>();
    //private List<Transform> targets = new List<Transform>();

    public ParkSung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        List<Transform> targets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);

        if (targets.Count <= 0)
        {
            yield break;
        }

        float time = 0.5f;
        for (int i = 0; i < Mathf.Min(skillData.projectileCount, targets.Count); i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);

            if (targets[i].TryGetComponent(out Monster monster))
            {
                for (int j = 0; j < skillData.skillEffect.Count; j++)
                {
                    monster.SkillEffectActivation(skillData.skillEffect[j], float.Parse(skillData.skillEffectParam[j]));
                    monster.Hit(skillData.damage);

                    if (skillData.skillEffect[j] == SKILL_EFFECT.RESTRAINT)
                    {
                        time = float.Parse(skillData.skillEffectParam[j]);
                    }
                }
            }
            
            projectile.transform.position = targets[i].transform.position;
            projectiles.Add(projectile);
            yield return frame;
        }

        yield return new WaitForSeconds(time);

        foreach (Projectile projectile in projectiles)
        {
            SkillManager.Instance.DeSpawnProjectile(projectile);
        }
        projectiles.Clear();

        //allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        //for (int i = 0; i < Mathf.Min(skillData.projectileCount, allTargets.Count); i++)
        //{
        //    targets.Add(allTargets[i]);
        //    Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
        //    projectile.transform.position = targets[i].transform.position;
        //    projectiles.Add(projectile);
        //}
        //foreach (Transform target in targets)
        //{
        //    if (target.TryGetComponent(out Monster monster))
        //    {
        //        monster.SkillEffectActivation(skillData.skillEffect[0], float.Parse(skillData.skillEffectParam[0]));
        //        monster.Hit(skillData.damage);
        //    }
        //}
        //yield return new WaitForSeconds(int.Parse(skillData.skillEffectParam[0]));
        //for (int i = 0; i < projectiles.Count; i++)
        //{
        //    SkillManager.Instance.DeSpawnProjectile(projectiles[i]);
        //    targets.Clear();
        //}
    }

}