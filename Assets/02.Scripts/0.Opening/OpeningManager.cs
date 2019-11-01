using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    public float animSpeed;
    public float animEndTime;
    public float exitTime;
    public int mainSceneNumber;
    private bool keyValueB = true;
    public GameObject textContainer;
    public AudioClip sd_pressKey;
    public Animator logoAnimator;
    private List<Animator> _animators;
    public AudioSource bgmSource;
    // Start is called before the first frame update
    void Start()
    {
        AlwaysObject.Instance.BgmSourceSet(bgmSource);
        _animators = new List<Animator>(textContainer.GetComponentsInChildren<Animator>());
        StartCoroutine(ActionAnim());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            keyValueB = false;
            animSpeed = 0.0f;
            animEndTime = 0.0f;
        }
    }

    IEnumerator ActionAnim()
    {
        while (keyValueB)
        {
            foreach(var animator in _animators)
            {
                animator.SetTrigger("Updown");
                yield return new WaitForSeconds(animSpeed);
            }
            yield return new WaitForSeconds(animEndTime);
        }

        AlwaysObject.Instance.SoundOn(sd_pressKey);

        foreach (var animator in _animators)
        {
            animator.SetTrigger("Size");
        }
        logoAnimator.SetTrigger("Fade");
        while (!logoAnimator.GetCurrentAnimatorStateInfo(0).IsName("LogoFadeOut"))
        {
            yield return null;
        }

        while (logoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
        {
            yield return null;
        }
        SceneManager.LoadScene(mainSceneNumber);
    }
}
