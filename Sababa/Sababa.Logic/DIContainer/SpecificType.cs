using System;

namespace Sababa.Logic.DIContainer
{
    public class SpecificType
    {
        public Type ConcreteType { get; set; }
        public bool IsExternallyOwned { get; set; }
        public bool IsSingleInstance { get; set; }
    }
}
