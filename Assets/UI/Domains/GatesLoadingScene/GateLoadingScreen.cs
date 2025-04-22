using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class GateLoadingScreen : MonoBehaviour
{

   
    public static GateLoadingScreen Instance;
    private  VisualElement root ;


    void Awake()
    {
        // Se já existe uma instância diferente, destrói esse novo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Define como instância única
        Instance = this;
      
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        this.root = GetComponent<UIDocument>().rootVisualElement;
        var gate = root.Q<VisualElement>("Gate");

        gate.experimental.animation.Start(0, 100, 500, (element, value) =>
        {
            element.style.height = new Length(value, LengthUnit.Percent);
        }).Ease(Easing.InOutBack);
    }


    public void OpenGate()
    {
        // var root = GetComponent<UIDocument>().rootVisualElement;
        var gate = root.Q<VisualElement>("Gate");
        // Debug.Log("OpenGate");

        var animTime = 2000;

        gate.experimental.animation.Start(100, 0, animTime, (element, value) =>
        {
            element.style.height = new Length(value, LengthUnit.Percent);
        }).Ease(Easing.InOutBack);

        // Debug.Log("GateOpened");
        Destroy(this.gameObject, animTime / 1000f);

    }
}
