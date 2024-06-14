using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churk : ActiveSkill
{
    private List<Transform> allTargets = new List<Transform>();

    public Churk(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }
    public override IEnumerator Activation()
    {
        allTargets = Scanner.RangeTarget(shooter, skillData.attackDistance, (int)LayerConstant.MONSTER);
        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);

        foreach (Transform target in allTargets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.SkillEffectActivation(skillData.skillEffect[0], float.Parse(skillData.skillEffectParam[0]));
            }
        }
        
        Debug.Log("척 발동");
        yield return frame;

        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
}
