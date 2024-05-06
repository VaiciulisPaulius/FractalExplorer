using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField maxIterationIField;
    [SerializeField] Button maxIterationToggleBtn;

    [SerializeField] TMP_InputField threadsIField;
    [SerializeField] Button threadsToggleBtn;

    [SerializeField] TextMeshProUGUI perfomanceText;
    [SerializeField] TextMeshProUGUI threadsNumText;
    int perfomanceMs = 0;

    bool startMeasure;
    float timer;
    

    void Start()
    {
        InitState();
        SubscribeToEvents();
    }
    private void Update()
    {
        if (startMeasure) timer += Time.deltaTime;
    }
    void InitState()
    {
        int maxIterations = FindObjectOfType<AGenerator>().maxIterations;
        maxIterationIField.text = maxIterations.ToString();

        int threadsNum = ThreadPoolManager.Instance.GetThreadCount();
        threadsIField.text = threadsNum.ToString();
    }
    void SubscribeToEvents()
    {
        maxIterationToggleBtn.onClick.AddListener(ChangeMaxIteration);
        threadsToggleBtn.onClick.AddListener(ChangeThreadNum);

        ThreadPoolManager.Instance.OnJobReceived += MeasureExecutionTime;
        ThreadPoolManager.Instance.OnJobCompleted += DisplayExecutionTime;
    }

    void DisplayExecutionTime()
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            perfomanceMs = (int)(timer * 1000);
            startMeasure = false;
            timer = 0;

            perfomanceText.text = "rendered in " + perfomanceMs.ToString() + "ms";
        });
    }
    void MeasureExecutionTime()
    {
        startMeasure = true;
    }

    void ChangeMaxIteration()
    {
        try
        {
            string input = maxIterationIField.text;
            int maxIterations = Convert.ToInt32(input);

            FindObjectOfType<AGenerator>().ChangeMaxIteration(maxIterations);
            FindObjectOfType<AGenerator>().Generate();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    void ChangeThreadNum()
    {
        try
        {
            string input = threadsIField.text;
            int threadNum = Convert.ToInt32(input);

            ThreadPoolManager.Instance.ChangeThreadNumber(threadNum);

            threadsNumText.text = "Current threads num: " + threadNum.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    void validation()
    {

    }


}
