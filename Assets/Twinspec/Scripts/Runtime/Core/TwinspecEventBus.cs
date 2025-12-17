// Assets/Twinspec/Scripts/Runtime/Core/TwinspecEventBus.cs
using System;
using Twinspec.State;

namespace Twinspec.Core
{
    public static class TwinspecEventBus
    {
        public static event Action<GeometryState> OnGeometryStateApplied;
        public static void PublishGeometryApplied(GeometryState s) => OnGeometryStateApplied?.Invoke(s);
    }
}
