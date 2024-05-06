using System.Collections.Generic;
using System;
using System.Threading;

public class ThreadPoolManager
{
    private static ThreadPoolManager instance;
    public static ThreadPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ThreadPoolManager(1);
            }
            return instance;
        }
    }

    public event Action OnJobReceived;
    private bool jobReceivedInvoked = false;

    public event Action OnJobCompleted;
    private bool jobCompletedInvoked = false;

    public event Action OnThreadChange;

    class JobState
    {
        public int tasksToComplete;
        public int completedtasks;
        public bool jobReceivedInvoked;
    }

    Dictionary<int, JobState> KeyValueJobs = new Dictionary<int, JobState>();


    private List<Thread> threads = new List<Thread>();
    private Queue<Action> tasks = new Queue<Action>();

    private int threadCount;

    public ThreadPoolManager(int threadCount)
    {
        InitThreads(threadCount);
    }

    void InitThreads(int threadCount)
    {
        TerminateThreads();
        this.threadCount = threadCount;

        for (int i = 0; i < threadCount; i++)
        {
            Thread thread = new Thread(ThreadWorker);
            threads.Add(thread);
            thread.Start();
        }
    }
    void TerminateThreads()
    {
        if (threads.Count == 0) return;
        foreach (Thread thread in threads)
        {
            thread.Abort();
        }
        threads.Clear();
    }

    public void QueueTask(Action task)
    {
        lock (tasks)
        {
            tasks.Enqueue(task);
            Monitor.PulseAll(tasks);
        }
    }
    public void QueueJob(Action task, int jobsNum, int identifier)
    {
        if(!KeyValueJobs.ContainsKey(identifier))
        {
            jobReceivedInvoked = true;

            JobState state = new JobState();
            state.tasksToComplete = jobsNum;
            state.completedtasks = 0;
            state.jobReceivedInvoked = true;

            KeyValueJobs.Add(identifier, state);

            OnJobReceived?.Invoke();
        }
        lock (tasks)
        {
            JobState state = KeyValueJobs[identifier];
            tasks.Enqueue(() => {
                task();

                state.completedtasks++;
                if (state.completedtasks == state.tasksToComplete)
                {
                    KeyValueJobs.Remove(identifier);
                    OnJobCompleted?.Invoke();
                }
            });
            Monitor.PulseAll(tasks);
        }
    }
    public void QueueTask(Action<object> task, object state)
    {
        QueueTask(() => task(state));
    }
    public void QueueJob(Action<object> task, object state, int jobsNum, int identifier)
    {
        QueueJob(() => task(state), jobsNum, identifier);
    }

    public void ThreadWorker()
    {
        while (true)
        {
            Action task = null;

            lock (tasks)
            {
                while (tasks.Count == 0)
                {
                    Monitor.Wait(tasks);
                }
                task = tasks.Dequeue();
            }

            if (task != null)
            {
                task();
            }
        }
    }
    public void ChangeThreadNumber(int newThreadNumber)
    {
        if(OnThreadChange != null) OnThreadChange.Invoke();
        InitThreads(newThreadNumber);
    }
    public int GetThreadCount()
    {
        return threadCount;
    }
}
