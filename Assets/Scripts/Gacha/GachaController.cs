using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class GachaController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] public GachaPull banner1;
        [SerializeField] private TextMeshProUGUI[] nomes;
        public bool coroutineOff;
        private GachaPull runtimeBanner;
        [SerializeField] private Image[] imagens;
        [SerializeField] private Sprite[] teaseGacha;

        [SerializeField] private GameObject fivePull;
        [SerializeField] private GameObject onePull;
    

        [SerializeField] private float animationSpeed = 0.15f;

        [SerializeField] private Image imagemSingle;
        [SerializeField] private GameObject newSingle;
        [SerializeField] private GameObject[] newMulti;

        [SerializeField] private Player_info playerInfo;
        [SerializeField] private TextMeshProUGUI points;
        [SerializeField] private TextMeshProUGUI power;



        [SerializeField] public string[] lines;
        public float textSpeed;
        public int i;
        public int j;
        public string testeNomePersonagem;

    void Start()
    {
        power.SetText(playerInfo.Power.ToString());
        points.SetText(playerInfo.Point.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private IEnumerator AnimateGacha(Image target)
    {
    int index = 0;

    while (true)
    {
        target.sprite = teaseGacha[index];

        index = (index + 1) % teaseGacha.Length;

        yield return new WaitForSeconds(animationSpeed);
    }
    }
    
    public void retornaUmResultado()
    {
        if(playerInfo.SpendPoint(100)){
        power.SetText(playerInfo.Power.ToString());
        points.SetText(playerInfo.Point.ToString());
            runtimeBanner = Instantiate(banner1);
            StartCoroutine(ShowResults(1));
        }
    }

    public void retornaCincoResultados()
    {
        foreach(Image img in imagens){
            img.enabled = false;
        }

        foreach(GameObject img in newMulti){
            img.SetActive(false);
        }
        if(playerInfo.SpendPoint(500)){
        power.SetText(playerInfo.Power.ToString());
        points.SetText(playerInfo.Point.ToString());
            runtimeBanner = Instantiate(banner1);
            StartCoroutine(ShowResults(5));
        }

    }

    IEnumerator ShowResults(int quantidade)
    {
        for (int j = 0; j < quantidade; j++)
        {
            GachaPull.Personagens personagem = runtimeBanner.SortearPersonagem();

            bool isNew = !runtimeBanner.repetido;
            if(quantidade == 1){
                onePull.SetActive(true);
                fivePull.SetActive(false);
            }
            if(quantidade != 1){
            onePull.SetActive(false);
            fivePull.SetActive(true);
            }
            Image target = quantidade == 1 ? imagemSingle : imagens[j];

            target.gameObject.SetActive(true);
            target.enabled = true;

            Coroutine animation = StartCoroutine(AnimateGacha(target));

            yield return new WaitForSeconds(textSpeed * 4);

            StopCoroutine(animation);

            target.sprite = personagem.sprite;

            if (quantidade == 1)
            {
                newSingle.SetActive(isNew);
            }
            else
            {
                newMulti[j].SetActive(isNew);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        Destroy(runtimeBanner);
        runtimeBanner = null;
    }
    
}
