using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Views
{
    public class LightsCarrousel : MonoBehaviour
    {
        enum LightSwitchStyle
        {
            Parity,
            Snake
        }
        [SerializeField] private List<GameObject> lights = new List<GameObject>();
        [SerializeField] private float changeTime = 0.5f;
        [SerializeField] private LightSwitchStyle lightSwitchStyle;
        [SerializeField] private float timeToStopAfterSwitch = 1;
        private float timeAccumulator = 0;
        private float timeAccumulatorForStatic = 0;
        private float parity = 0;
        private int snakeCounter = 0;
        // Use this for initialization
        void Start()
        {
            timeAccumulator = changeTime;
            timeAccumulatorForStatic = 0;
        }

        // Update is called once per frame
        void Update()
        {
            timeAccumulator += Time.deltaTime;
            if (snakeCounter == lights.Count)
                timeAccumulatorForStatic += Time.deltaTime;
            if (timeAccumulator >= changeTime)
            {
                timeAccumulator = 0;
                switch (lightSwitchStyle)
                {
                    case LightSwitchStyle.Parity:
                        SwitchByParity();
                        break;
                    case LightSwitchStyle.Snake:
                        if (snakeCounter == lights.Count)
                        {
                            if (timeAccumulatorForStatic <= timeToStopAfterSwitch)
                                lights.ForEach(light => light.SetActive(true));
                            else
                            {
                                timeAccumulatorForStatic = 0;
                                snakeCounter = 0;
                            }    
                            break;
                        }
                        SnakeSwitch();
                        break;
                    default:
                        break;
                }
            }
        }

        private void SnakeSwitch()
        {
            lights.ForEach(light => light.SetActive(false));
            lights[snakeCounter].SetActive(true);
            snakeCounter++;
        }

        private void SwitchByParity()
        {
            parity = parity == 1 ? 0 : 1;
            for (int i = 0; i < lights.Count; i++)
            {
                if (i % 2 == parity)
                {
                    lights[i].SetActive(true);
                }
                else
                    lights[i].SetActive(false);
            }
        }
    }
}