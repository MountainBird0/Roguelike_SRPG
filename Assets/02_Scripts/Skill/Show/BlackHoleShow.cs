using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackHoleShow : SkillVisualEffect
{
    public GameObject hole;
    public GameObject explosion;


    public override void Apply(SkillEffect effect)
    {
        StartCoroutine(HoleApply(effect));
    }

    public override float GetDuration()
    {
        return 7.5f;
    }

    private IEnumerator HoleApply(SkillEffect effect)
    {
        var pos = Turn.selectedPos;
        transform.position = new Vector3Int(pos.x, pos.y, 5);

        hole.SetActive(true);

        yield return new WaitForSeconds(4.5f);

        effect.Apply();
        
        hole.SetActive(false);
        explosion.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        explosion.SetActive(false);
    }
}
