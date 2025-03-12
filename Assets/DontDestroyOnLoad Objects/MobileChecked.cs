using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MobileChecked : MonoBehaviour
{
    public Toggle mobileToggle;
    public bool isMobile;
    [SerializeField] private Game_Events ge;


    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;


        getIsMobileToggle();


        DontDestroyOnLoad(this.gameObject);
    }

    // REFERENTE A VARIAVEL isMobile
    public bool getIsMobile()
    {
        return this.isMobile;
    }

    public void setIsmobile(bool b)
    {
        this.isMobile = b;
        this.mobileToggle.isOn = this.isMobile;

        // SE GAME EVENTS ESTIVER SETADO, ENTAO CHAMA A FUNÇÂO DE LA
        // if (ge) ge.toggleMobile(this.isMobile);
    }

    // REFERENTE AO OBJETO TOGGLE
    private void getIsMobileToggle()
    {

        mobileToggle = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<GetIsMobile>().toggleIsMobile;

        // SE NAO FOR ENCONTRADO, RETORNA
        if (!mobileToggle) return;

        mobileToggle.onValueChanged.AddListener(delegate
        {
            setIsmobile(mobileToggle.isOn);
        });

        // SE O isMobile estiver ativo, entao seta o Toggle como True
        if (isMobile) setIsmobile(isMobile);


        // print("isMobile Started");


    }



    // REFERENTE A TROCA DE CENA
    private void ChangedActiveScene(Scene current, Scene next)
    {
        string currentName = current.name;
        Debug.Log("Scenes: " + currentName + ", " + next.name);
        // getIsMobileToggle();
        //
        // SetMobileCheckedInGameEvents();

    }

    // private void SetMobileCheckedInGameEvents()
    // {
    //     try
    //     {
    //         ge = GameObject.FindGameObjectWithTag("Game_Events").GetComponent<Game_Events>();
    //         ge.setMobileChecked(gameObject.GetComponent<MobileChecked>());
    //     }
    //     catch (System.Exception)
    //     {
    //
    //         throw;
    //     }
    // }


}
