using UnityEngine;

public class Animationchange : MonoBehaviour
{
    Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();
    }
    public void BanishEffect()
    {

        animatior.SetBool("BanishEffect", true);
        animatior.SetBool("StartedEffect", false);


    }
    public void EndEffect()
    {
        animatior.SetBool("BanishEffect", false);
    }
    
}
