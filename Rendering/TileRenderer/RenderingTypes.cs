using Mapster.Common.MemoryMappedTypes;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace Mapster.Rendering;

public struct GeoFeature : BaseShape
{
    public enum GeoFeatureType
    {
        Plain,
        Hills,
        Mountains,
        Forest,
        Desert,
        Unknown,
        Water,
        Residential
    }

    public int ZIndex
    {
        get
        {
            switch (Type)
            {
                case GeoFeatureType.Plain:
                    return 10;
                case GeoFeatureType.Hills:
                    return 12;
                case GeoFeatureType.Mountains:
                    return 13;
                case GeoFeatureType.Forest:
                    return 11;
                case GeoFeatureType.Desert:
                    return 9;
                case GeoFeatureType.Unknown:
                    return 8;
                case GeoFeatureType.Water:
                    return 40;
                case GeoFeatureType.Residential:
                    return 41;
            }

            return 7;
        }
        set { }
    }

    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }
    public GeoFeatureType Type { get; set; }

    public void Render(IImageProcessingContext context)
    {
        var color = Color.Magenta;
        switch (Type)
        {
            case GeoFeatureType.Plain:
                color = Color.LightGreen;
                break;
            case GeoFeatureType.Hills:
                color = Color.DarkGreen;
                break;
            case GeoFeatureType.Mountains:
                color = Color.LightGray;
                break;
            case GeoFeatureType.Forest:
                color = Color.Green;
                break;
            case GeoFeatureType.Desert:
                color = Color.SandyBrown;
                break;
            case GeoFeatureType.Unknown:
                color = Color.Magenta;
                break;
            case GeoFeatureType.Water:
                color = Color.LightBlue;
                break;
            case GeoFeatureType.Residential:
                color = Color.LightCoral;
                break;
        }

        if (!IsPolygon)
        {
            var pen = new Pen(color, 1.2f);
            context.DrawLines(pen, ScreenCoordinates);
        }
        else
        {
            context.FillPolygon(color, ScreenCoordinates);
        }
    }

    public GeoFeature(ReadOnlySpan<Coordinate> c, GeoFeatureType type)
    {
        IsPolygon = true;
        Type = type;
        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
    }

    public GeoFeature(ReadOnlySpan<Coordinate> c, MapFeatureData feature)
    {
        IsPolygon = feature.Type == GeometryType.Polygon;
        var naturalKey = feature.Properties.natural;
        Type = GeoFeatureType.Unknown;
        if (naturalKey != PropertiesClass.Natural.NOTHING)
        {
            if (naturalKey == PropertiesClass.Natural.FELL ||
                naturalKey == PropertiesClass.Natural.GRASSLAND ||
                naturalKey == PropertiesClass.Natural.HEATH ||
                naturalKey == PropertiesClass.Natural.MOOR ||
                naturalKey == PropertiesClass.Natural.SCRUB ||
                naturalKey == PropertiesClass.Natural.WETLAND)
            {
                Type = GeoFeatureType.Plain;
            }
            else if (naturalKey == PropertiesClass.Natural.WOOD ||
                     naturalKey == PropertiesClass.Natural.TREE_ROW)
            {
                Type = GeoFeatureType.Forest;
            }
            else if (naturalKey == PropertiesClass.Natural.BARE_ROCK ||
                     naturalKey == PropertiesClass.Natural.ROCK ||
                     naturalKey == PropertiesClass.Natural.SCREE)
            {
                Type = GeoFeatureType.Mountains;
            }
            else if (naturalKey == PropertiesClass.Natural.BEACH ||
                     naturalKey == PropertiesClass.Natural.SAND)
            {
                Type = GeoFeatureType.Desert;
            }
            else if (naturalKey == PropertiesClass.Natural.WATER)
            {
                Type = GeoFeatureType.Water;
            }
        }

        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
    }
    public static bool verifyIfNatural(MapFeatureData feature)
    {
        return feature.Type == GeometryType.Polygon && feature.Properties.natural != PropertiesClass.Natural.NOTHING;
    }
    public static bool verifyIfForest(MapFeatureData feature)
    {
        return feature.Properties.boundary == PropertiesClass.Boundary.FOREST;
    }
    public static bool verifyIfLanduseForestOrOrchad(MapFeatureData feature)
    {
        return feature.Properties.landuse == PropertiesClass.Landuse.FOREST || feature.Properties.landuse == PropertiesClass.Landuse.ORCHARD;
    }
    public static bool verifyIfLanduseResidential(MapFeatureData feature)
    {
        PropertiesClass.Landuse landuse = feature.Properties.landuse;
        return landuse == PropertiesClass.Landuse.RESIDENTIAL || landuse == PropertiesClass.Landuse.CEMETERY || landuse == PropertiesClass.Landuse.INDUSTRIAL ||
          landuse == PropertiesClass.Landuse.COMMERCIAL || landuse == PropertiesClass.Landuse.SQUARE || landuse == PropertiesClass.Landuse.CONSTRUCTION ||
          landuse == PropertiesClass.Landuse.MILITARY || landuse == PropertiesClass.Landuse.QUARRY || landuse == PropertiesClass.Landuse.BROWNFIELD;
    }
    public static bool verifyIfLandusePlain(MapFeatureData feature)
    {
        PropertiesClass.Landuse landuse = feature.Properties.landuse;
        return landuse == PropertiesClass.Landuse.FARM || landuse == PropertiesClass.Landuse.MEADOW || landuse == PropertiesClass.Landuse.GRASS ||
          landuse == PropertiesClass.Landuse.GREENFIELD || landuse == PropertiesClass.Landuse.RECREATION_GROUND || landuse == PropertiesClass.Landuse.WINTER_SPORTS ||
          landuse == PropertiesClass.Landuse.ALLOTMENTS;
    }
    public static bool verifyIfWater(MapFeatureData feature)
    {
        PropertiesClass.Landuse landuse = feature.Properties.landuse;
        return landuse == PropertiesClass.Landuse.RESERVOIR || landuse == PropertiesClass.Landuse.BASIN;
    }

    public static bool verifyIfBuilding(MapFeatureData feature)
    {
        return feature.Properties.building != PropertiesClass.Building.NOTHING && feature.Type == GeometryType.Polygon;
    }

    public static bool verifyIfLeisure(MapFeatureData feature)
    {
        return feature.Properties.leisure != PropertiesClass.Leisure.NOTHING && feature.Type == GeometryType.Polygon;
    }

    public static bool verifyIfAmenity(MapFeatureData feature)
    {
        return feature.Properties.amenity != PropertiesClass.Amenity.NOTHING && feature.Type == GeometryType.Polygon;
    }
}

public struct Railway : BaseShape
{
    public int ZIndex { get; set; } = 45;
    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }

    public void Render(IImageProcessingContext context)
    {
        var penA = new Pen(Color.DarkGray, 2.0f);
        var penB = new Pen(Color.LightGray, 1.2f, new[]
        {
            2.0f, 4.0f, 2.0f
        });
        context.DrawLines(penA, ScreenCoordinates);
        context.DrawLines(penB, ScreenCoordinates);
    }

    public Railway(ReadOnlySpan<Coordinate> c)
    {
        IsPolygon = false;
        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
    }
    public static bool verifyIfRailway(MapFeatureData feature)
    {
        return feature.Properties.railway != PropertiesClass.Railway.NOTHING;
    }
}

public struct PopulatedPlace : BaseShape
{
    public int ZIndex { get; set; } = 60;
    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }
    public string Name { get; set; }
    public bool ShouldRender { get; set; }

    public void Render(IImageProcessingContext context)
    {
        if (!ShouldRender)
        {
            return;
        }
        var font = SystemFonts.Families.First().CreateFont(12, FontStyle.Bold);
        context.DrawText(Name, font, Color.Black, ScreenCoordinates[0]);
    }

    public PopulatedPlace(ReadOnlySpan<Coordinate> c, MapFeatureData feature)
    {
        IsPolygon = false;
        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
        var name = feature.Properties.name;

        if (feature.Label.IsEmpty)
        {
            ShouldRender = false;
            Name = "Unknown";
        }
        else
        {
            Name = string.IsNullOrWhiteSpace(name) ? feature.Label.ToString() : name;
            ShouldRender = true;
        }
    }

    public static bool verifyIfPopulatedPlace(MapFeatureData feature)
    {
        // https://wiki.openstreetmap.org/wiki/Key:place
        if (feature.Type != GeometryType.Point)
        {
            return false;
        }
        PropertiesClass.PopulatedPlace populatedPlace = feature.Properties.populatedPlace;
        return populatedPlace == PropertiesClass.PopulatedPlace.CITY || populatedPlace == PropertiesClass.PopulatedPlace.TOWN || populatedPlace == PropertiesClass.PopulatedPlace.LOCALITY || populatedPlace == PropertiesClass.PopulatedPlace.HAMLET;
    }
}

public struct Border : BaseShape
{
    public int ZIndex { get; set; } = 30;
    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }

    public void Render(IImageProcessingContext context)
    {
        var pen = new Pen(Color.Gray, 2.0f);
        context.DrawLines(pen, ScreenCoordinates);
    }

    public Border(ReadOnlySpan<Coordinate> c)
    {
        IsPolygon = false;
        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
    }

    public static bool verifyIfBorder(MapFeatureData feature)
    {
        return feature.Properties.boundary == PropertiesClass.Boundary.ADMINISTRATIVE && feature.Properties.adminLevel == PropertiesClass.AdminLevel.LEVEL2;
    }
}

public struct Waterway : BaseShape
{
    public int ZIndex { get; set; } = 40;
    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }

    public void Render(IImageProcessingContext context)
    {
        if (!IsPolygon)
        {
            var pen = new Pen(Color.LightBlue, 1.2f);
            context.DrawLines(pen, ScreenCoordinates);
        }
        else
        {
            context.FillPolygon(Color.LightBlue, ScreenCoordinates);
        }
    }


    public Waterway(ReadOnlySpan<Coordinate> c, bool isPolygon = false)
    {
        IsPolygon = isPolygon;
        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
    }
    public static bool verifyIfWaterway(MapFeatureData feature)
    {
        return feature.Properties.water != PropertiesClass.Water.NOTHING && feature.Type != GeometryType.Point;
    }
}

public struct Road : BaseShape
{
    public int ZIndex { get; set; } = 50;
    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }

    public void Render(IImageProcessingContext context)
    {
        if (!IsPolygon)
        {
            var pen = new Pen(Color.Coral, 2.0f);
            var pen2 = new Pen(Color.Yellow, 2.2f);
            context.DrawLines(pen2, ScreenCoordinates);
            context.DrawLines(pen, ScreenCoordinates);
        }
    }

    public Road(ReadOnlySpan<Coordinate> c, bool isPolygon = false)
    {
        IsPolygon = isPolygon;
        ScreenCoordinates = new PointF[c.Length];
        for (var i = 0; i < c.Length; i++)
            ScreenCoordinates[i] = new PointF((float)MercatorProjection.lonToX(c[i].Longitude),
                (float)MercatorProjection.latToY(c[i].Latitude));
    }
    public static bool verifyIfRoad(MapFeatureData feature)
    {
        return feature.Properties.highway != PropertiesClass.Highway.UNKNOWN && feature.Properties.highway != PropertiesClass.Highway.NOTHING;
    }
}

public interface BaseShape
{
    public int ZIndex { get; set; }
    public bool IsPolygon { get; set; }
    public PointF[] ScreenCoordinates { get; set; }

    public void Render(IImageProcessingContext context);

    public void TranslateAndScale(float minX, float minY, float scale, float height)
    {
        for (var i = 0; i < ScreenCoordinates.Length; i++)
        {
            var coord = ScreenCoordinates[i];
            var newCoord = new PointF((coord.X + minX * -1) * scale, height - (coord.Y + minY * -1) * scale);
            ScreenCoordinates[i] = newCoord;
        }
    }
}
