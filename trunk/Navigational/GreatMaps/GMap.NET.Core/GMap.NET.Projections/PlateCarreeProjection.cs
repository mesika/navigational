﻿
namespace GMap.NET.Projections
{
   using System;

   /// <summary>
   /// Plate Carrée (literally, “plane square”) projection
   /// PROJCS["WGS 84 / World Equidistant Cylindrical",GEOGCS["GCS_WGS_1984",DATUM["D_WGS_1984",SPHEROID["WGS_1984",6378137,298.257223563]],PRIMEM["Greenwich",0],UNIT["Degree",0.017453292519943295]],UNIT["Meter",1]]
   /// </summary>
   public class PlateCarreeProjection : PureProjection
   {
      static readonly double MinLatitude = -85.05112878;
      static readonly double MaxLatitude = 85.05112878;
      static readonly double MinLongitude = -180;
      static readonly double MaxLongitude = 180;

      Size tileSize = new Size(512, 512);
      public override Size TileSize
      {
         get
         {
            return tileSize;
         }
      }

      public override double Axis
      {
         get
         {
            return 6378137;
         }
      }

      public override double Flattening
      {
         get
         {
            return (1.0 / 298.257223563);
         }
      }

      public override Point FromLatLngToPixel(double lat, double lng, int zoom)
      {
         Point ret = Point.Empty;

         lat = Clip(lat, MinLatitude, MaxLatitude);
         lng = Clip(lng, MinLongitude, MaxLongitude);

         Size s = GetTileMatrixSizePixel(zoom);
         double mapSizeX = s.Width;
         double mapSizeY = s.Height;

         double scale = 360.0 / mapSizeX;

         ret.Y = (int) ((90.0 - lat) / scale);
         ret.X = (int) ((lng + 180.0) / scale);

         return ret;
      }

      public override PointLatLng FromPixelToLatLng(int x, int y, int zoom)
      {
         PointLatLng ret = PointLatLng.Empty;

         Size s = GetTileMatrixSizePixel(zoom);
         double mapSizeX = s.Width;
         double mapSizeY = s.Height;

         double scale = 360.0 / mapSizeX;

         ret.Lat = 90 - (y * scale);
         ret.Lng = (x * scale) - 180;

         return ret;
      }

      public override Size GetTileMatrixMaxXY(int zoom)
      {
         int y = (int) Math.Pow(2, zoom);
         return new Size((2*y) - 1, y - 1);
      }

      public override Size GetTileMatrixMinXY(int zoom)
      {
         return new Size(0, 0);
      }
   }
}
