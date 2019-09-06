using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows.Threading;

namespace SciChart.Examples.Demo.Common
{
    /// <summary>
    /// Represents an object that schedules units of work on a <see cref="T:System.Windows.Threading.Dispatcher" />.
    /// </summary>
    /// <remarks>
    /// This scheduler type is typically used indirectly through the <see cref="M:System.Reactive.Linq.DispatcherObservable.ObserveOnDispatcher``1(System.IObservable{``0})" /> and <see cref="M:System.Reactive.Linq.DispatcherObservable.SubscribeOnDispatcher``1(System.IObservable{``0})" /> methods that use the Dispatcher on the calling thread.
    /// </remarks>
    public class DispatcherSchedulerEx : LocalScheduler, ISchedulerPeriodic
    {
        private Dispatcher _dispatcher;
        private DispatcherPriority _priority;

        /// <summary>
        /// Gets the scheduler that schedules work on the current <see cref="T:System.Windows.Threading.Dispatcher" />.
        /// </summary>
        [Obsolete("Use the Current property to retrieve the DispatcherScheduler instance for the current thread's Dispatcher object.")]
        public static DispatcherSchedulerEx Instance
        {
            get
            {
                return new DispatcherSchedulerEx(Dispatcher.CurrentDispatcher);
            }
        }

        /// <summary>
        /// Gets the scheduler that schedules work on the <see cref="T:System.Windows.Threading.Dispatcher" /> for the current thread.
        /// </summary>
        public static DispatcherSchedulerEx Current
        {
            get
            {
                Dispatcher dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
                if (dispatcher == null)
                    throw new InvalidOperationException("There is no current Dispatcher thread");
                return new DispatcherSchedulerEx(dispatcher);
            }
        }

        /// <summary>
        /// Constructs a DispatcherScheduler that schedules units of work on the given <see cref="T:System.Windows.Threading.Dispatcher" />.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to schedule work on.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dispatcher" /> is null.</exception>
        public DispatcherSchedulerEx(Dispatcher dispatcher)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            this._dispatcher = dispatcher;
            this._priority = DispatcherPriority.Normal;
        }

        /// <summary>
        /// Constructs a DispatcherScheduler that schedules units of work on the given <see cref="T:System.Windows.Threading.Dispatcher" /> at the given priority.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to schedule work on.</param>
        /// <param name="priority">Priority at which units of work are scheduled.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dispatcher" /> is null.</exception>
        public DispatcherSchedulerEx(Dispatcher dispatcher, DispatcherPriority priority)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            this._dispatcher = dispatcher;
            this._priority = priority;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> associated with the DispatcherScheduler.
        /// </summary>
        public Dispatcher Dispatcher
        {
            get
            {
                return this._dispatcher;
            }
        }

        /// <summary>
        /// Gets the priority at which work items will be dispatched.
        /// </summary>
        public DispatcherPriority Priority
        {
            get
            {
                return this._priority;
            }
        }

        /// <summary>Schedules an action to be executed on the dispatcher.</summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="action" /> is null.</exception>
        public override IDisposable Schedule<TState>(
          TState state,
          Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            SingleAssignmentDisposable d = new SingleAssignmentDisposable();
            this._dispatcher.BeginInvoke(this._priority, (Action)(() =>
            {
                if (d.IsDisposed)
                    return;
                d.Disposable = action((IScheduler)this, state);
            }));
            return (IDisposable)d;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime on the dispatcher, using a <see cref="T:System.Windows.Threading.DispatcherTimer" /> object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="action" /> is null.</exception>
        public override IDisposable Schedule<TState>(
          TState state,
          TimeSpan dueTime,
          Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            TimeSpan timeSpan = Scheduler.Normalize(dueTime);
            if (timeSpan.Ticks == 0L)
                return this.Schedule<TState>(state, action);
            MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
            DispatcherTimer timer = new DispatcherTimer(this._priority, this._dispatcher);
            timer.Tick += (EventHandler)((s, e) =>
            {
                DispatcherTimer dispatcherTimer = Interlocked.Exchange<DispatcherTimer>(ref timer, (DispatcherTimer)null);
                if (dispatcherTimer == null)
                    return;
                try
                {
                    d.Disposable = action((IScheduler)this, state);
                }
                finally
                {
                    dispatcherTimer.Stop();
                    action = (Func<IScheduler, TState, IDisposable>)null;
                }
            });
            timer.Interval = timeSpan;
            timer.Start();
            d.Disposable = Disposable.Create((Action)(() =>
            {
                DispatcherTimer dispatcherTimer = Interlocked.Exchange<DispatcherTimer>(ref timer, (DispatcherTimer)null);
                if (dispatcherTimer == null)
                    return;
                dispatcherTimer.Stop();
                action = (Func<IScheduler, TState, IDisposable>)((_, __) => Disposable.Empty);
            }));
            return (IDisposable)d;
        }

        /// <summary>
        /// Schedules a periodic piece of work on the dispatcher, using a <see cref="T:System.Windows.Threading.DispatcherTimer" /> object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="action" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="period" /> is less than TimeSpan.Zero.</exception>
        public IDisposable SchedulePeriodic<TState>(
          TState state,
          TimeSpan period,
          Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            DispatcherTimer timer = new DispatcherTimer(this._priority, this._dispatcher);
            TState state1 = state;
            timer.Tick += (EventHandler)((s, e) => state1 = action(state1));
            timer.Interval = period;
            timer.Start();
            return Disposable.Create((Action)(() =>
            {
                DispatcherTimer dispatcherTimer = Interlocked.Exchange<DispatcherTimer>(ref timer, (DispatcherTimer)null);
                if (dispatcherTimer == null)
                    return;
                dispatcherTimer.Stop();
                action = (Func<TState, TState>)(_ => _);
            }));
        }
    }
}
