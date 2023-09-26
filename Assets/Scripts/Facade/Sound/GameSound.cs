using System;
using Facade.Sound;
using UnityEngine;

[Serializable]
public class GameSound
{
    public SoundId id;
    public SoundGroupId groupId = SoundGroupId.Sounds;
    public AudioSource audio;
}