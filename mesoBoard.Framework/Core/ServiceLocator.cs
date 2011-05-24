using Ninject;
using Ninject.Parameters;

namespace mesoBoard.Framework.Core
{
    public static class ServiceLocator
    {
        public static IKernel Kernel { get; private set; }

        public static void Initialize(IKernel kernel)
        {
            Kernel = kernel;
        }

        public static T Get<T>(params IParameter[] parameters)
        {
            return Kernel.Get<T>(parameters);
        }     
    }
}