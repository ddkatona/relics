using UnityEngine;

public class RangedAnimation : ParametricAnimation {

    public Field origin;
    public Field target;
    public Unit host;

    public GameObject projectilePrefab;
    public GameObject projectile;
    public Vector3 prevPosition;

    // Audio
    public AudioClip arrowHit;

    void OnEnable() {
        host = transform.parent.GetComponentInParent<Unit>();
    }

    public override void ParametricUpdate(float t) {
        Vector3 way = target.transform.position - origin.transform.position;
        float distance = way.magnitude;
        float travelled = t * distance;
        float height = Mathf.Sin(t * Mathf.PI) / 2f;
        projectile.transform.position = origin.transform.position + way.normalized * travelled + Vector3.up * height;
        projectile.transform.LookAt(prevPosition);
        prevPosition = projectile.transform.position;
    }

    public override void ParametricEnd() {
        projectile.transform.position = target.transform.position;
        Destroy(projectile);
        EndAnimation();
    }

    public void Register(Move move) {
        origin = move.GetStart(host.Board);
        target = move.GetEnd(host.Board);
        Game.MAIN.animationManager.Add(this);
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = host.transform.position;
        projectile.transform.localScale /= 3;
    }

    public override void EndAnimation() {
        PlayClip(arrowHit);
        base.EndAnimation();
    }

}