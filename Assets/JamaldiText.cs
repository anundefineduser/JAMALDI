using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JamaldiText : MonoBehaviour
{
    public Animator textAnimator;
    public float time;
    public float meet;

    private void Awake()
    {
        meet = Random.RandomRange(2, 5f);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("TrullUnlocked") == 1)
            GetComponent<TextMeshProUGUI>().text = "THOSE WHO KNOW";
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= meet)
        {
            time = 0f;
            meet = Random.RandomRange(2f, 5f);
            textAnimator.SetFloat("AnimSpeed", Random.Range(1, 3) * (System.Convert.ToBoolean(Random.Range(0, 2)) ? -1f : 1f));
        }
    }
}
