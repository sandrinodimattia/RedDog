using System.Threading.Tasks;

namespace RedDog.Messenger.Contracts
{
    public interface ISession
    {
        string Id
        {
            get;
        }

        /// <summary>
        /// Read the session state.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        Task<TState> GetState<TState>()
            where TState : class;

        /// <summary>
        /// Persiste the session state.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        Task SetState<TState>(TState state)
            where TState : class;
    }
}