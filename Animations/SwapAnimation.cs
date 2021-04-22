using UnityEngine;

public class SwapAnimation : CustomAnimation {

    public GameObject targetMoveAnimationPrefab;

    public void Init(Unit a, Unit b) {
        AnimationManager am = Game.MAIN.animationManager;
        am.Add(Move(a, b.field.transform.position + Vector3.up * 1f));
        am.AddToLastInParallel(Move(b, a.field.transform.position + Vector3.up * 0.5f));

        am.Add(Move(a, a.field.transform.position + Vector3.up * 1f));
        am.AddToLastInParallel(Move(b, b.field.transform.position + Vector3.up * 0.5f));

        am.Add(Move(a, a.field.transform.position));
        am.AddToLastInParallel(Move(b, b.field.transform.position));

        //Destroy(gameObject);
    }

    public TargetMoveAnimation Move(Unit unit, Vector3 target) {
        TargetMoveAnimation tma = Instantiate(targetMoveAnimationPrefab).GetComponent<TargetMoveAnimation>();
        tma.destroyAfter = true;
        tma.Init(unit, target);
        return tma;
    }

}

