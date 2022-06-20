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
            Snake,
            SnakePositive
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
                            RestartWith(true,true);
                            break;
                        }
                        SnakeSwitch(true);
                        break;
                    case LightSwitchStyle.SnakePositive:
                        if (snakeCounter == lights.Count)
                        {
                            RestartWith(false,false);
                            break;
                        }
                        SnakeSwitch(false);
                        break;
                    default:
                        break;
                }
            }
        }

        private void RestartWith(bool lightState, bool restartCounter)
        {
            if (timeAccumulatorForStatic <= timeToStopAfterSwitch)
                lights.ForEach(light => light.SetActive(lightState));
            else
            {
                timeAccumulatorForStatic = 0;
                if(restartCounter)
                    snakeCounter = 0;
            }
        }

        private void SnakeSwitch(bool turnOff)
        {
            if (turnOff)
                TurnLightsOff();
            lights[snakeCounter].SetActive(true);
            snakeCounter++;
        }

        private void TurnLightsOff()
        {
            lights.ForEach(light => light.SetActive(false));
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