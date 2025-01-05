using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip[] musicList;
    [SerializeField] AudioSource musicSource;

    int currentTrackIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.clip = musicList[currentTrackIndex];
        musicSource.Play();
        StartCoroutine(PlayNextTrack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayNextTrack()
    {
        //yield return null;

        while (true)
        {
            
            yield return new WaitForSeconds(musicList[currentTrackIndex].length);
            currentTrackIndex++;

            if (currentTrackIndex > musicList.Length)
            {
                currentTrackIndex = 0;
            }

            musicSource.clip = musicList[currentTrackIndex];
            musicSource.Play();
        }
        

        
    }
}
