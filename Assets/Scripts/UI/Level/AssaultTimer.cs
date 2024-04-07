using System;
using System.Collections;
using GlobalStates.Game;
using TMPro;
using UnityEngine;

namespace UI.Level
{
    public class AssaultTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private Game Game => Game.Instance;
        
        private IEnumerator _updateAssaultTimeRoutine; 

        private void OnEnable()
        {
            Game.StateChangedEvent += OnStateChanged;
        }

        private void OnDisable()
        {
            Game.StateChangedEvent -= OnStateChanged;
        }
        
        private void OnStateChanged(StateType stateType)
        {
            print(stateType);
            if (stateType == StateType.Assault)
            {
                if (_updateAssaultTimeRoutine != null)
                {
                    StopCoroutine(_updateAssaultTimeRoutine);
                }

                _updateAssaultTimeRoutine = UpdateAssaultTime(Game.AssaultDurationSeconds);
                StartCoroutine(_updateAssaultTimeRoutine);
            }
            else if (_updateAssaultTimeRoutine != null)
            {
                StopCoroutine(_updateAssaultTimeRoutine);
            }
        }

        private IEnumerator UpdateAssaultTime(float assaultTimeSeconds)
        {
            TimeSpan currentAssaultTime = TimeSpan.FromSeconds(assaultTimeSeconds + 1);
            
            while (currentAssaultTime > TimeSpan.Zero)
            {
                currentAssaultTime -= TimeSpan.FromSeconds(Time.fixedDeltaTime);
                _text.text = currentAssaultTime.ToString("m\\:ss");
                
                yield return new WaitForFixedUpdate();
            }
        }
    }
}