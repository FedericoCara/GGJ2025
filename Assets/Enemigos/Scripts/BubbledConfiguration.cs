using System.Collections.Generic;
using UnityEngine;

namespace Enemigos.Scripts
{
    [CreateAssetMenu(menuName = "Create BubbledAlphas", fileName = "BubbledAlphas", order = 0)]
    public class BubbledConfiguration : ScriptableObject
    {
        public List<float> alphas = new() { .1f,.2f,.35f,0.6f,1f};
        public float healthPerSecond = 1;
        public float timeToDissappear = 1;
    }
}