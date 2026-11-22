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

        [SerializeField] public string[] lines;
        public float textSpeed;
        public int i;
        public int j;
        public string testeNomePersonagem;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    
    public void retornaUmResultado(){
        runtimeBanner = Instantiate(banner1);
        StartCoroutine (ShowResults(1));
        if (runtimeBanner != null)
        {Destroy(runtimeBanner);
        runtimeBanner = null;
        }

    }

    public void retornaCincoResultados(){
            runtimeBanner = Instantiate(banner1);
            StartCoroutine (ShowResults(5));
            if (runtimeBanner != null)
            {Destroy(runtimeBanner);
            runtimeBanner = null;
            }
        }

    IEnumerator ShowResults(int quantidade)
{
    for (int j = 0; j < quantidade; j++)
    {
        Debug.Log("SORTEOU " + j);
        GachaPull.Personagens personagem = runtimeBanner.SortearPersonagem();
        nomes[j].text = "";

        foreach (char c in ". . . . .   ")
        {
            nomes[j].text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        nomes[j].text = personagem.nome;
    }
}
}
