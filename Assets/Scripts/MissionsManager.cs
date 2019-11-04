using System.Collections;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    public enum Task
    {
        WAITING,
        ENTRANCE,
        WORKSHOP,
        FLYING,
        TANK,
        END,
    }

    public GameObject Rig;

    [Space]
    public AudioManager audioManager;
    public GameObject Vortex;

    [Space]
    public bool FlyingTaskSucceed = false;
    public bool TankTaskSucceed = false;

    [Space]
    public GameObject EntrancePosition;
    public GameObject WorkshopPosition;
    public GameObject FlyingPosition;
    public GameObject TankPosition;

    [Space]
    public ObjectsInCollision headDetection;

    [Space]
    public GameObject keyholeTrigger;
    public GameObject flyingTableTrigger;
    public GameObject flyingSeatTrigger;
    public GameObject tankTableTrigger;
    public GameObject tankSeatTrigger;
    public GameObject endOfFlyingTaskTrigger;
    public GameObject endOfTankTaskTrigger;

    [Space]
    public Material FadeInOutMaterial;

    [Space]
    public Task currentTask = Task.ENTRANCE;

    [Space]
    public float FadeSpeed = 0.5f;

    [Space]
    public Animator chestAnimator;
    // Open, Shake
    public GameObject monaLisa;

    [Space]
    public GameObject flyingCity;
    public GameObject tankCity;

    [Space]
    public FlyingScript flyingScript;
    public TankScript tankScript;

    private void Awake()
    {
        Vortex.SetActive(false);
        flyingSeatTrigger.SetActive(false);
        tankSeatTrigger.SetActive(false);

        tankCity.SetActive(false);

        Color color = FadeInOutMaterial.color;
        color.a = 0;
        FadeInOutMaterial.color = color;

        flyingScript.enabled = false;
        tankScript.enabled = false;
    }

    private void Start()
    {
        Rig.transform.position = EntrancePosition.transform.position;

        currentTask = Task.ENTRANCE;
        StartCoroutine(EntranceAnimation());
    }

    private void Update()
    {
        if (currentTask == Task.ENTRANCE)
        {
            if (headDetection.objectsInCollision.Contains(keyholeTrigger))
            {
                currentTask = Task.WAITING;
                StopAllCoroutines();
                StartCoroutine(EnterInWorkshopAnimation());
            }
        }
        else if (currentTask == Task.WORKSHOP)
        {
            if (!FlyingTaskSucceed && headDetection.objectsInCollision.Contains(flyingTableTrigger))
            {
                currentTask = Task.WAITING;
                StopAllCoroutines();
                StartCoroutine(EnterFlyingTaskAnimation());
            }
            else if (!TankTaskSucceed && headDetection.objectsInCollision.Contains(tankTableTrigger))
            {
                currentTask = Task.WAITING;
                StopAllCoroutines();
                StartCoroutine(EnterTankTaskAnimation());
            }

            if (FlyingTaskSucceed && TankTaskSucceed)
            {
                currentTask = Task.WAITING;
                StartCoroutine(EndOfGameAnimation());
            }
        }
        else if (currentTask == Task.FLYING)
        {
            if (headDetection.objectsInCollision.Contains(endOfFlyingTaskTrigger))
            {
                currentTask = Task.WAITING;
                StopAllCoroutines();
                StartCoroutine(BackToWorkshopAfterFlyingTaskAnimation());
            }
        }
        else if (currentTask == Task.TANK)
        {
            if (headDetection.objectsInCollision.Contains(endOfTankTaskTrigger))
            {
                currentTask = Task.WAITING;
                StopAllCoroutines();
                StartCoroutine(BackToWorkshopAfterTankTaskAnimation());
            }
        }
    }

    private void OnDestroy()
    {
        Color color = FadeInOutMaterial.color;
        color.a = 0;
        FadeInOutMaterial.color = color;
    }

    private IEnumerator EntranceAnimation()
    {
        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 1);
        yield return new WaitForSeconds(2);
        audioManager.PlayAudioOneTime(0, 2);
        yield return new WaitForSeconds(10);
        audioManager.PlayAudioOneTime(0, 3);
    }

    private IEnumerator EnterInWorkshopAnimation()
    {
        Debug.Log("Begin EnterInWorkshopAnimation");

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(FadeOut());

        // door sound
        audioManager.PlayAudioOneTime(1, 0);

        Rig.transform.position = WorkshopPosition.transform.position;

        yield return StartCoroutine(FadeIn());

        currentTask = Task.WORKSHOP;

        yield return new WaitForSeconds(1);
        chestAnimator.SetBool("Shake", true);
        audioManager.PlayAudioOneTime(0, 4);
        yield return new WaitForSeconds(4);
        chestAnimator.SetBool("Shake", false);
        audioManager.PlayAudioOneTime(0, 5);
        yield return new WaitForSeconds(10);
        audioManager.PlayAudioOneTime(0, 6);
        chestAnimator.SetBool("Shake", true);
        yield return new WaitForSeconds(7);
        audioManager.PlayAudioOneTime(0, 7);
        yield return new WaitForSeconds(4);
        audioManager.PlayAudioOneTime(0, 8);
        chestAnimator.SetBool("Shake", false);
        yield return new WaitForSeconds(7);
        audioManager.PlayAudioOneTime(0, 9);
        yield return new WaitForSeconds(11);
        audioManager.PlayAudioOneTime(0, 10);
        yield return new WaitForSeconds(8);
        audioManager.PlayAudioOneTime(0, 11);
        yield return new WaitForSeconds(12);
        audioManager.PlayAudioOneTime(0, 12);
        yield return new WaitForSeconds(6);

        yield return new WaitForSeconds(15);
        audioManager.PlayAudioOneTime(0, 45);
        yield return new WaitForSeconds(4);

        yield return new WaitForSeconds(15);
        audioManager.PlayAudioOneTime(0, 46);
        yield return new WaitForSeconds(4);
    }

    private IEnumerator EnterFlyingTaskAnimation()
    {
        Debug.Log("Begin EnterFlyingTaskAnimation");

        yield return new WaitForSeconds(2);
        audioManager.PlayAudioOneTime(0, 13);
        yield return new WaitForSeconds(4);
        audioManager.PlayAudioOneTime(0, 14);
        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 15);
        yield return new WaitForSeconds(6);
        audioManager.PlayAudioOneTime(0, 16);
        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 17);
        yield return new WaitForSeconds(2);
        audioManager.PlayAudioOneTime(0, 18);

        flyingSeatTrigger.SetActive(true);

        yield return new WaitForSeconds(3);

        // wait for user to seat
        yield return new WaitUntil(() => CheckIfSeatedOnFlyingArea());

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(FadeOut());

        flyingSeatTrigger.SetActive(false);

        Rig.transform.SetParent(FlyingPosition.transform);
        Rig.transform.localPosition = Vector3.zero;

        audioManager.SetAudioSource3D(0, false);

        yield return StartCoroutine(FadeIn());

        currentTask = Task.FLYING;

        flyingScript.enabled = true;

        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 19);
        yield return new WaitForSeconds(2);
        audioManager.PlayAudioOneTime(0, 20);
        yield return new WaitForSeconds(3);
        audioManager.PlayAudioOneTime(0, 21);
        yield return new WaitForSeconds(3);

        yield return new WaitForSeconds(10);
        audioManager.PlayAudioOneTime(0, 22);
        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 23);
        yield return new WaitForSeconds(6);
    }

    private IEnumerator BackToWorkshopAfterFlyingTaskAnimation()
    {
        FlyingTaskSucceed = true;

        Debug.Log("Begin BackToWorkshopAfterFlyingTaskAnimation");

        yield return new WaitForSeconds(5);

        flyingScript.enabled = false;

        if (!FlyingTaskSucceed)
            audioManager.PlayAudioOneTime(0, 42);
        else
            audioManager.PlayAudioOneTime(0, 44);
        yield return new WaitForSeconds(6);

        yield return StartCoroutine(FadeOut());

        Rig.transform.SetParent(null);
        Rig.transform.position = WorkshopPosition.transform.position;

        audioManager.SetAudioSource3D(0, true);

        yield return StartCoroutine(FadeIn());

        currentTask = Task.WORKSHOP;

        if (!TankTaskSucceed)
        {
            yield return new WaitForSeconds(5);
            audioManager.PlayAudioOneTime(0, 47);
            chestAnimator.SetBool("Shake", true);
            yield return new WaitForSeconds(3);
            chestAnimator.SetBool("Shake", false);
        }
    }

    private IEnumerator EnterTankTaskAnimation()
    {
        Debug.Log("Begin EnterTankTaskAnimation");

        yield return new WaitForSeconds(2);
        audioManager.PlayAudioOneTime(0, 24);
        yield return new WaitForSeconds(3);
        audioManager.PlayAudioOneTime(0, 25);
        yield return new WaitForSeconds(4);
        audioManager.PlayAudioOneTime(0, 26);
        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 27);
        yield return new WaitForSeconds(4);
        audioManager.PlayAudioOneTime(0, 28);
        yield return new WaitForSeconds(3);
        audioManager.PlayAudioOneTime(0, 29);
        yield return new WaitForSeconds(7);
        audioManager.PlayAudioOneTime(0, 30);
        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 31);

        tankSeatTrigger.SetActive(true);

        yield return new WaitForSeconds(6);
        audioManager.PlayAudioOneTime(0, 32);
        yield return new WaitForSeconds(2);

        // wait for user to seat
        yield return new WaitUntil(() => CheckIfSeatedOnTankArea());

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(FadeOut());

        tankCity.SetActive(true);
        flyingCity.SetActive(false);

        tankSeatTrigger.SetActive(false);

        Rig.transform.SetParent(TankPosition.transform);
        Rig.transform.localPosition = Vector3.zero;

        audioManager.SetAudioSource3D(0, false);

        yield return StartCoroutine(FadeIn());

        currentTask = Task.TANK;

        tankScript.enabled = true;

        yield return new WaitForSeconds(5);
        audioManager.PlayAudioOneTime(0, 33);
        yield return new WaitForSeconds(4);
        audioManager.PlayAudioOneTime(0, 34);
        yield return new WaitForSeconds(10);

        // BOOM BOOM BOOM

        audioManager.PlayAudioOneTime(0, 35);
        yield return new WaitForSeconds(5);

        // BOOM BOOM

        audioManager.PlayAudioOneTime(0, 36);
        yield return new WaitForSeconds(3);
    }

    private IEnumerator BackToWorkshopAfterTankTaskAnimation()
    {
        Debug.Log("Begin BackToWorkshopAfterTankTaskAnimation");

        TankTaskSucceed = true;

        yield return new WaitForSeconds(2);

        tankScript.enabled = false;

        if (!FlyingTaskSucceed)
            audioManager.PlayAudioOneTime(0, 42);
        else
            audioManager.PlayAudioOneTime(0, 44);
        yield return new WaitForSeconds(6);

        yield return StartCoroutine(FadeOut());

        Rig.transform.SetParent(null);
        Rig.transform.position = WorkshopPosition.transform.position;

        audioManager.SetAudioSource3D(0, true);

        flyingCity.SetActive(true);

        yield return StartCoroutine(FadeIn());

        currentTask = Task.WORKSHOP;

        if (!FlyingTaskSucceed)
        {
            yield return new WaitForSeconds(5);
            chestAnimator.SetBool("Shake", true);
            audioManager.PlayAudioOneTime(0, 46);
            yield return new WaitForSeconds(3);
            chestAnimator.SetBool("Shake", false);
        }
    }

    private bool CheckIfSeatedOnFlyingArea()
    {
        return headDetection.objectsInCollision.Contains(flyingSeatTrigger);
    }

    private bool CheckIfSeatedOnTankArea()
    {
        return headDetection.objectsInCollision.Contains(tankSeatTrigger);
    }

    private IEnumerator EndOfGameAnimation()
    {
        Debug.Log("Begin EndOfGameAnimation");

        chestAnimator.SetBool("Shake", true);
        yield return new WaitForSeconds(5);
        chestAnimator.SetBool("Shake", false);
        yield return new WaitForSeconds(1);

        chestAnimator.SetBool("Open", true);
        yield return new WaitForSeconds(1);

        StartCoroutine(MoveUpMonaLisa());

        audioManager.PlayAudioOneTime(0, 48);
        yield return new WaitForSeconds(3);
        audioManager.PlayAudioOneTime(0, 49);
        yield return new WaitForSeconds(8);
        audioManager.PlayAudioOneTime(0, 50);
        yield return new WaitForSeconds(5);

        yield return StartCoroutine(FadeOut());

        currentTask = Task.END;
    }

    private IEnumerator MoveUpMonaLisa()
    {
        Vector3 target = monaLisa.transform.position + new Vector3(0, 0.8f, 0);

        while (monaLisa.transform.position != target)
        {
            Vector3.MoveTowards(monaLisa.transform.position, target, Time.deltaTime * 10);
            yield return null;
        }
    }


    private IEnumerator FadeOut()
    {
        Debug.Log("Begin FadeOut");

        Vortex.SetActive(true);

        Color color = FadeInOutMaterial.color;

        while (color.a < 1)
        {
            color.a += Time.deltaTime * FadeSpeed;

            if (color.a >= 1)
                color.a = 1;

            FadeInOutMaterial.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        Vortex.SetActive(false);

        Debug.Log("End FadeOut");
    }

    private IEnumerator FadeIn()
    {
        Debug.Log("Begin FadeIn");

        yield return new WaitForSeconds(0.25f);

        Vortex.SetActive(true);

        Color color = FadeInOutMaterial.color;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime * FadeSpeed;

            if (color.a <= 0)
                color.a = 0;

            FadeInOutMaterial.color = color;

            yield return null;
        }

        Vortex.SetActive(false);

        Debug.Log("End FadeIn");
    }
}
