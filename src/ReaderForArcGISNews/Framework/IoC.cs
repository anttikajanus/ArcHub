namespace ReaderForArcGISNews.Framework
{
    using System;
    using Autofac;

    public static class IoC
    {
        private static IContainer container;

        private static bool isInitialized;

        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IContainer Container
        {
            get
            {
                if (isInitialized == false)
                {
                    throw new NullReferenceException("Initialize container first.");
                }

                return container;
            }

            private set
            {
                container = value;
            }
        }

        /// <summary>
        /// Initializes <see cref="IoC"/> to use.
        /// </summary>
        /// <param name="containerToUse">Inversion of control container to use.</param>
        public static void Initialize(IContainer containerToUse)
        {
            if (containerToUse == null)
            {
                throw new ArgumentNullException("containerToUse");
            }

            if (container != null)
            {
                throw new ArgumentException("Container is already set.");
            }

            Container = containerToUse;
            isInitialized = true;
        }

        /// <summary>
        /// Resolves dependency.
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>Returns concrete intance of T.</returns>
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
