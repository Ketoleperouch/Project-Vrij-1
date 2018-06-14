using UnityEngine;

public class ActivateAnimation : GameEvent {

    public string parameter = "Active";

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Execute()
    {
        anim.SetBool(parameter, true);
    }
}
