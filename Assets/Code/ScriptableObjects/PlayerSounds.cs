using System.Collections;
using UnityEngine;

namespace Assets.Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSounds")]
    public class PlayerSounds : ScriptableObject
    {
        [SerializeField] private AudioClip jump;
        [SerializeField] private AudioClip dieStabbed;
        [SerializeField] private AudioClip dieBurnt;
        [SerializeField] private AudioClip dieFellDown;
        [SerializeField] private AudioClip dieSmashed;
        [SerializeField] private AudioClip dieExploded;

        public AudioClip Jump { get => jump; }
        public AudioClip DieStabbed { get => dieStabbed; }
        public AudioClip DieBurnt { get => dieBurnt; }
        public AudioClip DieFellDown { get => dieFellDown; }
        public AudioClip DieSmashed { get => dieSmashed; }
        public AudioClip DieExploded { get => dieExploded; }
    }
}