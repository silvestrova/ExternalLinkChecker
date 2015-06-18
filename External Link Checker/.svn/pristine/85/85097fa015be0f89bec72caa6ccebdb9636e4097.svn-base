// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadPool.cs" company="">
//   
// </copyright>
// <summary>
//   The tasks pool.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Threading;

  /// <summary>
  /// The tasks pool.
  /// </summary>
  public sealed class TasksPool : IDisposable
  {
    #region Fields

    /// <summary>
    /// The tasks.
    /// </summary>
    private readonly LinkedList<Action> tasks = new LinkedList<Action>(); // actions to be processed by worker threads

    /// <summary>
    /// The workers.
    /// </summary>
    private readonly LinkedList<Thread> workers; // queue of worker threads ready to process actions

    /// <summary>
    /// The disallow add.
    /// </summary>
    private bool disallowAdd; // set to true when disposing queue but there are still tasks pending

    /// <summary>
    /// The disposed.
    /// </summary>
    private bool disposed; // set to true when disposing queue and no more tasks are pending

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksPool"/> class.
    /// </summary>
    /// <param name="size">
    /// The size.
    /// </param>
    public TasksPool(int size)
    {
      this.workers = new LinkedList<Thread>();
      for (int i = 0; i < size; ++i)
      {
        var worker = new Thread(this.Worker)
                       {
                         Name = string.Concat("Worker ", i)
                       };
        worker.Start();
        this.workers.AddLast(worker);
      }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
      lock (this.tasks)
      {
        if (!this.disposed)
        {
          GC.SuppressFinalize(this);

          this.disallowAdd = true; // wait for all tasks to finish processing while not allowing any more new tasks
          while (this.tasks.Count > 0)
          {
            Monitor.Wait(this.tasks);
          }

          this.disposed = true;
          Monitor.PulseAll(this.tasks); // wake all workers (none of them will be active at this point; disposed flag will cause then to finish so that we can join them)

          // waitForThreads = true;
        }
      }
    }

    /// <summary>
    /// The queue task.
    /// </summary>
    /// <param name="task">
    /// The task.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// </exception>
    public void QueueTask(Action task)
    {
      lock (this.tasks)
      {
        if (this.disallowAdd)
        {
          throw new InvalidOperationException("This Pool instance is in the process of being disposed, can't add anymore");
        }

        if (this.disposed)
        {
          throw new ObjectDisposedException("This Pool instance has already been disposed");
        }

        this.tasks.AddLast(task);
        Monitor.PulseAll(this.tasks); // pulse because tasks count changed
      }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// The worker.
    /// </summary>
    private void Worker()
    {
      Action task;
      while (true)
      {
        // loop until threadpool is disposed
        lock (this.tasks)
        {
          // finding a task needs to be atomic
          while (true)
          {
            // wait for our turn in _workers queue and an available task
            if (this.disposed)
            {
              return;
            }

            if (null != this.workers.First && ReferenceEquals(Thread.CurrentThread, this.workers.First.Value) && this.tasks.Count > 0)
            {
              // we can only claim a task if its our turn (this worker thread is the first entry in _worker queue) and there is a task available
              task = this.tasks.First.Value;
              this.tasks.RemoveFirst();
              this.workers.RemoveFirst();
              Monitor.PulseAll(this.tasks); // pulse because current (First) worker changed (so that next available sleeping worker will pick up its task)
              break; // we found a task to process, break out from the above 'while (true)' loop
            }

            Monitor.Wait(this.tasks); // go to sleep, either not our turn or no task to process
          }
        }

        task(); // process the found task
        lock (this.tasks)
        {
          this.workers.AddLast(Thread.CurrentThread);
        }

        task = null;
      }
    }

    #endregion
  }
}