using UnityEngine;

public class AnimatePlayerOnStageCleared : MonoBehaviour
{
    public AnimationCurve movementCurve; // Curva de movimento no tempo
    [SerializeField] private float duration = 3f;          // Tempo total da animação
    [SerializeField] private float targetY = 100f;         // Y final desejado

    [SerializeField] private bool callHangar;

    [SerializeField] private float timer = 0f;
    private float startY;

    void OnEnable()
    {
        timer = 0;
        startY = transform.position.y;
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);


            float curveValue = movementCurve.Evaluate(t); // Pode ir de -0.05 até 1.0 por ex
            float yDisplacement = curveValue * targetY;

            Vector3 pos = transform.position;
            pos.y = startY + yDisplacement;
            transform.position = pos;
        }
        else
        {
            // Logica para chamar o Hangar apenas uma vez, sem ter q usar boll
            if (targetY >= 100f && !callHangar )
            {
                GetComponent<Player_Behavior>().CallHangarScreen(); // Chama o evento de Hangar
                callHangar = true; // Evita que o evento seja chamado novamente
                
            }


        }
    }
}

