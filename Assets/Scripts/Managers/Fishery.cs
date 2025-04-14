using UnityEngine;

public class Fishery : MonoBehaviour
{
    //TODO: This whole class is somewhat hardcoded due to time constraints, go through to refine and automate
    [Header("Management Style")]
    [SerializeField] ManageType manageType;

    [Header("Fish Holders")]
    [SerializeField] GameObject shallowFishHolder;
    private enum ManageType{
        Traditional,
        EBFM
    };

    private int _curCaughtFish;
    private int _maxCatchLimit;
    private int _expectedCatch;
    private float _catchChance;
    private int _curPopulation;

    private float _seasonTimer = 60;

    [Header("Debug")]
    [SerializeField] private int _debug_curMaxLimit;

    private void FixedUpdate()
    {
        _seasonTimer -= Time.deltaTime;

        if (_seasonTimer <= 0)
        {
            switch (manageType)
            {
                case ManageType.Traditional:
                    TraditionalCatchFish();
                    break;
                case ManageType.EBFM:
                    EBFMCatchFish();
                    break;
            }
        }

        _debug_curMaxLimit = _maxCatchLimit;
    }

    private void EBFMCatchFish()
    {
        _curCaughtFish = 0;
        _catchChance = Random.Range(1f, 10f);

        GameObject seasonalFishHolder = GameObject.Find("ShallowFishHolder");

        int maxPop = 0;
        int seasonalFishID = 0;
        int seasonalFishToCatch = 0;

        for (int i = 0; i < GameManager.Instance.creatureLimits.Length; i++)
        {
            if(GameManager.Instance.creatureLimits[i].currentPopulation > maxPop)
            {
                maxPop = GameManager.Instance.creatureLimits[i].currentPopulation;
                seasonalFishID = GameManager.Instance.creatureLimits[i].creatureIDToLimit;
                _curPopulation = GameManager.Instance.creatureLimits[i].currentPopulation;
                seasonalFishToCatch = i;
            }
        }

        switch (seasonalFishID)
        {
            //Shallow Fish
            case 0:
                seasonalFishHolder = GameObject.Find("ShallowFishHolder");
                break;
            //DeepFish
            case 2:
                seasonalFishHolder = GameObject.Find("DeepFishHolder");
                break;
            //VeryDeepHolder
            case 6:
                seasonalFishHolder = GameObject.Find("VeryDeepHolder");
                break;
            //FourthFish
            case 7:
                seasonalFishHolder = GameObject.Find("FourthHolder");
                break;
        }

        _maxCatchLimit = _curPopulation / 2;

        for (int i = 0; i < seasonalFishHolder.transform.childCount; i++)
        {
            GameObject fish = seasonalFishHolder.transform.GetChild(i).gameObject;
            if (Random.Range(0, 100) >= _catchChance)
            {
                Destroy(seasonalFishHolder.transform.GetChild(i).gameObject);
                GameManager.Instance.creatureLimits[seasonalFishToCatch].currentPopulation--;
                _curCaughtFish++;
            }
            if (_curCaughtFish >= _maxCatchLimit)
            {
                break;
            }
        }
        print("Caught " + _curCaughtFish + " from: " + seasonalFishID + " " + seasonalFishHolder.name);
        _seasonTimer = 60f;
        _debug_curMaxLimit = _maxCatchLimit;

    }

    private void TraditionalCatchFish()
    {
        _curCaughtFish = 0;
        _maxCatchLimit = 20;
        _catchChance = Random.Range(1f,10f);

        int seasonalFishToCatch = Random.Range(0, 3);
        int seasonalFishID = GameManager.Instance.creatureLimits[seasonalFishToCatch].creatureIDToLimit;
        GameObject seasonalFishHolder = GameObject.Find("ShallowFishHolder");

        switch (seasonalFishID)
        {
            //Shallow Fish
            case 0:
                seasonalFishHolder = GameObject.Find("ShallowFishHolder");
                break;
            //DeepFish
            case 2:
                seasonalFishHolder = GameObject.Find("DeepFishHolder");
                break;
            //VeryDeepHolder
            case 6:
                seasonalFishHolder = GameObject.Find("VeryDeepHolder");
                break;
            //FourthFish
            case 7:
                seasonalFishHolder = GameObject.Find("FourthHolder");
                break;

        }

        for(int i = 0; i < seasonalFishHolder.transform.childCount; i++)
        {
            GameObject fish = seasonalFishHolder.transform.GetChild(i).gameObject;
            if(Random.Range(0,100) >= _catchChance)
            {
                Destroy(seasonalFishHolder.transform.GetChild(i).gameObject);
                GameManager.Instance.creatureLimits[seasonalFishToCatch].currentPopulation--;
                _curCaughtFish++;
            }
            if (_curCaughtFish >= _maxCatchLimit)
            {
                break;
            }
        }
        print("Caught " + _curCaughtFish + " from: " + seasonalFishID + " " + seasonalFishHolder.name);
        _seasonTimer = 60f;
        _debug_curMaxLimit = _maxCatchLimit;
    }

    /* Code Draft - TRADITIONAL
     * var curCaughtFish
     * var maxCathLimit
     * var expectedCatch (Half of max)
     * var chanceToCatch (per fish ex: 50%)
     * 
     * For each desired fish
     * if above chanceToCatch -> catch (destroy fish) -> add 1 to curCaughtFish
     * 
     * at end of season
     * if curCaughtFish is >= expected && <= maxCaughtFish -> no change
     * if < expected -> lower maxCatchLimit
     * if == maxCatchLimit -> raise maxCatchLimit
     * 
     * Code Draft - EBFM
     * var curPopulation (+ || - 30% to account for real world challenges)
     * 
     * var maxCatchLimit = curPopulation / 2
     */
}

