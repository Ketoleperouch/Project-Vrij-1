using System.Collections;
using UnityEngine;

public class MoveObject : GameEvent {

    public Vector3 moveTowards;
    public float moveSpeed = 1;
    public float positionError = 0.01f;
    public Space space = Space.Self;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public override void Execute()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, space == 0 ? moveTowards : originalPosition + moveTowards) > positionError)
        {
            transform.position = Vector3.MoveTowards(transform.position, space == 0 ? moveTowards : originalPosition + moveTowards, Time.deltaTime * moveSpeed);
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Bounds rendererBounds = GetComponent<MeshRenderer>().bounds;
        Gizmos.color = Color.yellow;
        if (space == Space.Self)
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.DrawWireCube(originalPosition + moveTowards, rendererBounds.size);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position + moveTowards, rendererBounds.size);
            }
        }
        else
        {
            Gizmos.DrawWireCube(moveTowards, rendererBounds.size);
        }
    }
#endif
}
