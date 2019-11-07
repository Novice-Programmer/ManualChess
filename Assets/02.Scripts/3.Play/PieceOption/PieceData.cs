using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceData : MonoBehaviour
{
    public Text pieceName;
    public GameObject attack;
    public Text attackDamage;
    public Image[,] attackRange = new Image[3, 3];
    public GameObject skill;
    public Text skillDamage;
    public Image[,] skillRange = new Image[3, 3];
    public GameObject move;
    public Image[,] moveRange = new Image[3, 3];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            int x = i % 3;
            int z = i / 3;
            attackRange[x, Mathf.Abs(z - 2)] = attack.transform.GetChild(i).GetComponent<Image>();
            skillRange[x, Mathf.Abs(z - 2)] = skill.transform.GetChild(i).GetComponent<Image>();
            moveRange[x, Mathf.Abs(z - 2)] = move.transform.GetChild(i).GetComponent<Image>();
        }
    }
    
    public void DataViewSet(CardData cardData)
    {

    }
}
