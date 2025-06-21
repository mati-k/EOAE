using System.Collections.Generic;

namespace EOAE_Code.Wrappers
{
    public abstract class WrapperBase<W, T>
        where W : WrapperBase<W, T>, new()
        where T : class
    {
        // ToDo: Research more on weak references and plan out
        private static readonly Dictionary<T, W> CachedWrappers = new();

        protected WrapperBase() { }

        protected WrapperBase(T unwrappedObject)
        {
            UnwrappedObject = unwrappedObject;
        }

        public virtual T UnwrappedObject { get; private set; }

        public virtual bool IsNull => UnwrappedObject == null;

        public static W GetFor(T unwrappedObject)
        {
            if (unwrappedObject == null)
            {
                return null;
            }

            if (CachedWrappers.TryGetValue(unwrappedObject, out var wrapper))
            {
                return wrapper;
            }

            wrapper = new W { UnwrappedObject = unwrappedObject };
            CachedWrappers[unwrappedObject] = wrapper;
            return wrapper;
        }
    }
}
