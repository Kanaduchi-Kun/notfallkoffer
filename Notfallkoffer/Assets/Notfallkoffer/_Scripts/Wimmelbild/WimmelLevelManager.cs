using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WimmelLevelManager : MonoBehaviour
{
    public static WimmelLevelManager instance;
    public int maxActiveCats;

    private int totalCatsFound = 0;

    [SerializeField]
    private List<HiddenCatsData> hiddenCatsList;

    private List<HiddenCatsData> activeHiddenCatsList;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        activeHiddenCatsList = new List<HiddenCatsData>();
        AssignHiddenObjects();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.zero);

            if(hit && hit.collider != null)
            {
                Debug.Log("Object Name: " + hit.collider.gameObject.name);

                //Hier kommt alles rein was passiert wenn der User die richtige Katze klickt (Animationen etc)
                GameObject cat = hit.collider.gameObject;
                LeanTween.scale(cat, new Vector3(1.25f, 1.25f, 1.25f), 0.1f).setOnComplete(() => LeanTween.scale(cat, Vector3.zero, .3f));


                //cat.SetActive(false);

                for(int i = 0; i < activeHiddenCatsList.Count; i++)
                {
                    if(activeHiddenCatsList[i].catObject.name == cat.name)
                    {
                        activeHiddenCatsList.RemoveAt(i);
                        break;
                    }
                }

                totalCatsFound++;

                if(totalCatsFound >= maxActiveCats)
                {
                    Debug.Log("�bung abgeschlossen");
                }
            }
        }
    }

    void AssignHiddenObjects()
    {
        activeHiddenCatsList.Clear();
        totalCatsFound = 0;

        for (int i = 0; i < hiddenCatsList.Count; i++)
        {
            hiddenCatsList[i].catObject.GetComponent<Collider2D>().enabled = false;        
        }

        int count = 0;
        while(count < maxActiveCats)
        {
            int random = Random.Range(0, hiddenCatsList.Count);

            if(!hiddenCatsList[random].makeHidden)
            {
                hiddenCatsList[random].catObject.name = "" + count;
                hiddenCatsList[random].makeHidden = true;
                hiddenCatsList[random].catObject.GetComponent<Collider2D>().enabled = true;
                activeHiddenCatsList.Add(hiddenCatsList[random]);

                count++;
            }

        }
    }
}


[System.Serializable]
public class HiddenCatsData
{
    public string name;
    public GameObject catObject;
    public bool makeHidden = false;

}
