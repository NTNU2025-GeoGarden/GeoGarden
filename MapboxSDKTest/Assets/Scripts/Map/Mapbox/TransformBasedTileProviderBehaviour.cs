using Mapbox.Example.Scripts.TileProviderBehaviours;
using Mapbox.UnityMapService.TileProviders;

namespace Mapbox
{
    public class TransformBasedTileProviderBehaviour : TileProviderBehaviour
    {
        public TransformBasedTileProvider TileProvider;
        public override TileProvider Core
        {
            get
            {
                if (TileProvider.Transform == null)
                    TileProvider.Transform = transform;
                return TileProvider;
            }
        }
    }
}