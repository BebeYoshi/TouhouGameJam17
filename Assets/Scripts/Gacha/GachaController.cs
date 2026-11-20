using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class GachaController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] public GachaPull banner1;
        [SerializeField] public TextMeshProUGUI nome;
        [SerializeField] public string[] lines;
        public float textSpeed;
        public int i;
        public string testeNomePersonagem;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    
    public void retornaUmResultado(){
        GachaPull.Personagens personagem = banner1.SortearPersonagem();
        testeNomePersonagem = personagem.nome;
        StartDialogue();
    }

     void StartDialogue(){
        i = 0;
        nome.text = "";
        StartCoroutine(ShowLine());
    }

    IEnumerator ShowLine(){
        foreach (char c in lines[i].ToCharArray()){
            nome.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        nome.text = "";
        nome.text = testeNomePersonagem;
    }

}
