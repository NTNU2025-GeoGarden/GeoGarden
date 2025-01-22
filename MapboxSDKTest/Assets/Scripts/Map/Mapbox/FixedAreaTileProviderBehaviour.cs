using Mapbox.Example.Scripts.TileProviderBehaviours;
using Mapbox.UnityMapService.TileProviders;

namespace Mapbox
{
    public class FixedAreaTileProviderBehaviour : TileProviderBehaviour
    {
        public UnityFixedAreaTileProvider TileProvider;
        public override TileProvider Core => TileProvider;
    }
}