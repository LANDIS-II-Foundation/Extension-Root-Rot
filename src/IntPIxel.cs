using Landis.SpatialModeling;

namespace Landis.Extension.RootRot
{
    public class IntPixel : Pixel
    {
        public Band<double> MapCode = "The numeric code for each raster cell";

        public IntPixel()
        {
            SetBands(MapCode);
        }
    }
    
}
