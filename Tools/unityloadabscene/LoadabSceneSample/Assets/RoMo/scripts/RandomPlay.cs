using UnityEngine;
using System.Collections;

public class RandomPlay : MonoBehaviour {
    public AudioClip[] clips;
    public float[] clipRates;
    public float interval = 10f;
    public float rate = 1f;

    private float count = 0;
    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (clips == null || clips.Length == 0) return;
        count += Time.deltaTime;
        if(count > interval)
        {
            count -= interval;
            float r = Random.Range(0, 1f);
            if (r > rate)
            {
                return;
            }
            AudioClip clip = Roll(clips, clipRates);
            source.PlayOneShot(clip);
        }
	}
    AudioClip Roll(AudioClip[] clips, float[] rate)
    {
        if (rate == null || rate.Length == 0)
        {
            int index = Random.Range(0, clips.Length);
            return clips[index];
        }
        else
        {
            float count = 0;
            for (int i = 0; i < clips.Length; i++)
            {
                if (i < rate.Length)
                {
                    count += rate[i];
                }
            }
            float r = Random.Range(0f, count);
            int index = 0;
            while (r > 0)
            {
                r -= rate[index];
                if (r <= 0)
                {
                    return clips[index];
                }
                index++;
            }
            return clips[clips.Length - 1];
        }
    }
}
