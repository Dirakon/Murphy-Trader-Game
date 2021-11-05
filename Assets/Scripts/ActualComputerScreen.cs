using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualComputerScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(electricityWatcher());
        StartCoroutine(autoBethoven());
    }
    void Awake(){
        singelton = this;
    }
    public static ActualComputerScreen singelton;
    public bool canOperate = true;
    public void Init()
    {
        canOperate = true;
        foreach (var a in objectsOnScreen)
        {
            a?.SetActive(true);
        }
    }
    public void DeInit()
    {
        canOperate = false;
        foreach (var a in objectsOnScreen)
        {
            a?.SetActive(false);
        }
    }
    public static float currentBethovenValue = 0;
    public static int currentlyBoughtBethovens = 0;
    public static float currentMoney = 0;
    public float minChangeSeconds = 0.1f;
    public float maxChangeSeconds = 1.5f;
    public float speed = 1f;
    public float maxSlope = 2f;
    public float rerollerAttempts = 4;
    public GameObject[] objectsOnScreen;
    public LineRenderer lineRenderer;
    public float xChangePerT = 100f;
    public static bool NearlyEqual(float f1, float f2)
    {
        // Equal if they are within 0.00001 of each other
        return Mathf.Abs(f1 - f2) < 0.00001;
    }
    public IEnumerator autoBethoven()
    {
        float minBethoven = 0.1f;
        float maxBethoven = 100f;
        float maxY = 1.6f;
        LinkedList<Vector3> points = new LinkedList<Vector3>();
        float maxX = 2.9f;
        float currentValue01 = 0f;
        float farAwayStartX = 0f;
        float farAwayStartY = 0f;
        points.AddLast(new Vector3(0, 0, 0));
        while (true)
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.2f;
            float slope = Random.Range(-maxSlope, maxSlope);
            slope = Mathf.Clamp(slope, NearlyEqual(currentValue01, 0) ? 0 : -maxSlope,
            NearlyEqual(currentValue01, 1) ? 0 : maxSlope);
            bool didWePass = true;
            for (int i = 0; i < rerollerAttempts; ++i)
            {
                float reroller = Random.Range(0, maxSlope);
                if (Mathf.Abs(slope) > reroller)
                {
                    didWePass = false;
                    break;
                }
            }
            if (!didWePass)
                continue;
            float secondsToWait = Random.Range(minChangeSeconds, maxChangeSeconds);
            points.AddLast(new Vector3(maxX, maxY * currentValue01, 0));
            while (secondsToWait > 0)
            {
                float t = Time.deltaTime * speed;
                float newCurrentValue01 = currentValue01 + t * (slope);
                if (newCurrentValue01 <= 0 || newCurrentValue01 >= 1)
                {
                    break;
                }
                currentValue01 = newCurrentValue01;
                for (var it = points.First; it.Next != null;)
                {
                    float newX = it.Value.x - t * xChangePerT;
                    if (newX < 0 && it == points.First)
                    {
                        farAwayStartX -= t * xChangePerT;
                        float startX = farAwayStartX;
                        float startY = farAwayStartY;
                        float endX = it.Next.Value.x - t * xChangePerT;
                        float endY = it.Next.Value.y;
                        float difY = endY - startY;
                        float difX = endX - startX;
                        float newDifY = (difY / difX) * (endX);
                        it.Value = new Vector3(0, it.Next.Value.y - newDifY, 0);
                    }
                    else
                    {
                        it.Value = new Vector3(Mathf.Clamp(newX, 0, 99999), it.Value.y, 0);
                    }

                    it = it.Next;
                }
                points.Last.Value = new Vector3(maxX, maxY * currentValue01, 0);
                while (NearlyEqual(points.First.Value.x, points.First.Next.Value.x))
                {
                    points.RemoveFirst();
                    points.First.Value = new Vector3(0, points.First.Value.y, 0);
                    farAwayStartX = 0f;
                    farAwayStartY = points.First.Value.y;
                }
                Vector3[] pointsArr = new Vector3[points.Count];
                points.CopyTo(pointsArr, 0);
                lineRenderer.positionCount = pointsArr.Length;
                lineRenderer.SetPositions(pointsArr);
                secondsToWait -= t;
                currentBethovenValue = currentValue01 * (maxBethoven - minBethoven) + minBethoven;
                yield return null;
            }
            yield return null;
        }
    }
    public IEnumerator electricityWatcher()
    {
        while (true)
        {
            if (LightControl.electricityState && PC.pcState)
            {
                Init();
                while (LightControl.electricityState && PC.pcState) { yield return null; }
            }
            else
            {
                DeInit();
                while (!LightControl.electricityState || !PC.pcState) { yield return null; }
            }

            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!canOperate)
            return;
    }
}
