using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator meshAnimator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (meshAnimator == null)
        {
            GameObject lucy = GameObject.Find("Lucy");
            if (lucy != null)
                meshAnimator = lucy.GetComponent<Animator>();
        }
    }

    public void UpdateMovementAnimation(bool isWalking)
    {
        if (meshAnimator != null)
        {
            meshAnimator.SetBool("Walking", isWalking);
        }
    }

    public void UpdateCrouchState(bool isCrouched)
    {
        if (meshAnimator != null)
        {
            meshAnimator.SetBool("Crouched", isCrouched);
        }

        if (animator != null)
        {
            animator.SetBool("Crouched", isCrouched);
        }
    }
}
