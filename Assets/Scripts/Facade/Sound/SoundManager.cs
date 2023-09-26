using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facade.Sound;
using Interface;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<GameSound> sounds = new List<GameSound>();

    public void PlaySound(SoundId id, bool duplicate = false)
    {
        GameSound sound = sounds.Find(gS => gS.id == id);

        if (sound!=null)
        {
            if (duplicate)
            {
                sound.audio.PlayOneShot(sound.audio.clip);
            }
            else
            {
                sound.audio.Play();
            }
        }
    }
}
