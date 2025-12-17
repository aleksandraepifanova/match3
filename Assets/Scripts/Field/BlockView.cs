using UnityEngine;
using System.Collections;

public class BlockView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimatorOverrideController blueAnimator;
    [SerializeField] private AnimatorOverrideController orangeAnimator;

    private Animator animator;

    private static readonly int DestroyHash = Animator.StringToHash("Destroy");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Cell Cell { get; private set; }

    public void Init(Cell cell)
    {
        Cell = cell;

        switch (cell.Block.Type)
        {
            case BlockType.Blue:
                animator.runtimeAnimatorController = blueAnimator;
                break;

            case BlockType.Orange:
                animator.runtimeAnimatorController = orangeAnimator;
                break;
        }
    }

    public System.Action<BlockView> OnClicked;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void UpdateCell(Cell newCell)
    {
        Cell = newCell;
    }

    private void OnMouseDown()
    {
        OnClicked?.Invoke(this);
    }
    public IEnumerator PlayDestroy()
    {
        animator.SetTrigger(DestroyHash);

        yield return new WaitForSeconds(GetDestroyClipLength());
        Destroy(gameObject);
    }

    private float GetDestroyClipLength()
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
            if (clip.name.Contains("Destroy"))
                return clip.length;

        return 0.15f;
    }
}
