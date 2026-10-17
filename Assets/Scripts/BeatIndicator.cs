using TMPro;
using UnityEngine;


public class BeatIndicator : MonoBehaviour
{
    public int subDivision = 0;
    public RhythmManager rhythm;
    [SerializeField] TextMeshProUGUI beatTimeDisplay;
    // Update is called once per frame
    void Update()
    {
        int[] clock = rhythm.getBeatClock();
        
        if(GetComponent<SpriteRenderer>() != null){
            if(GetComponent<SpriteRenderer>().color.r < 1) GetComponent<SpriteRenderer>().color += new Color(2f, 0, 0);
            if(GetComponent<SpriteRenderer>().color.g < 1) GetComponent<SpriteRenderer>().color += new Color(0, 2f, 0);
            if(GetComponent<SpriteRenderer>().color.b < 1) GetComponent<SpriteRenderer>().color += new Color(0, 0, 2f);

            switch (subDivision)
            {
                case 0:
                    if(clock[1] == 0) GetComponent<SpriteRenderer>().color = Color.black;
                    break;
                case 1:
                    if(clock[2] == 0) GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 2:
                    if(clock[3] == 0) GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 3:
                    if(clock[4] == 0) GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                
            }
        }
        if(beatTimeDisplay != null)
        {
            beatTimeDisplay.text = clock[0].ToString();
        }
        
    }
}
