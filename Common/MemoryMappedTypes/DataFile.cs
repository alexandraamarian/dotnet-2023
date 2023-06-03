using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mapster.Common.MemoryMappedTypes;

/// <summary>
///     Action to be called when iterating over <see cref="MapFeature" /> in a given bounding box via a call to
///     <see cref="DataFile.ForeachFeature" />
/// </summary>
/// <param name="feature">The current <see cref="MapFeature" />.</param>
/// <param name="label">The label of the feature, <see cref="string.Empty" /> if not available.</param>
/// <param name="coordinates">The coordinates of the <see cref="MapFeature" />.</param>
/// <returns></returns>
public delegate bool MapFeatureDelegate(MapFeatureData featureData);

/// <summary>
///     Aggregation of all the data needed to render a map feature
/// </summary>



public class PropertiesClass
{
    public enum Highway
    {
        MOTORWAY,
        TRUNK,
        PRIMARY,
        SECONDARY,
        TERTIARY,
        UNCLASSIFIED,
        RESIDENTIAL,
        ROAD,
        NOTHING,
        UNKNOWN,
    }


    public enum Water
    {
        NOTHING,
        UNKNOWN,
    }
    public enum Boundary
    {
        NOTHING,
        ADMINISTRATIVE,
        FOREST,
    }

    public enum AdminLevel
    {
        LEVEL2,
        NOTENTERED

    }

    public enum PopulatedPlace
    {
        CITY,
        TOWN,
        LOCALITY,
        HAMLET,
        NOTENTERED
    }

    public enum Railway
    {
        NOTHING
    }
    public enum Natural
    {
        NOTHING,
        FELL,
        GRASSLAND,
        HEATH,
        MOOR,
        SCRUB,
        WETLAND,
        WOOD,
        TREE_ROW,
        BARE_ROCK,
        ROCK,
        SCREE,
        BEACH,
        SAND,
        WATER
    };

    public enum Landuse
    {
        NOTHING,
        FOREST,
        ORCHARD,
        RESIDENTIAL,
        CEMETERY,
        INDUSTRIAL,
        COMMERCIAL,
        SQUARE,
        CONSTRUCTION,
        MILITARY,
        QUARRY,
        BROWNFIELD,
        FARM,
        MEADOW,
        GRASS,
        GREENFIELD,
        RECREATION_GROUND,
        WINTER_SPORTS,
        ALLOTMENTS,
        RESERVOIR,
        BASIN,
    };

    public enum Building
    {
        NOTHING,
    };
    public enum Leisure
    {
        NOTHING,
    };
    public enum Amenity
    {
        NOTHING,
    };
    public Highway highway = Highway.NOTHING;

    private static Dictionary<string, Highway> MapHighway = new Dictionary<string, Highway>(){
        { "motorway", Highway.MOTORWAY },
        { "trunk", Highway.TRUNK },
        { "primary", Highway.PRIMARY },
        { "secondary", Highway.SECONDARY },
        { "tertiary", Highway.TERTIARY },
        { "unclassified", Highway.UNCLASSIFIED },
        { "residential", Highway.RESIDENTIAL },
        { "road", Highway.ROAD },
    };
    public Water water = Water.NOTHING;

    private static Dictionary<string, Water> MapWater = new Dictionary<string, Water>() { };
    public Boundary boundary = Boundary.NOTHING;

    private static Dictionary<string, Boundary> MapBoundary = new Dictionary<string, Boundary>(){
        { "administrative", Boundary.ADMINISTRATIVE },
        { "forest", Boundary.FOREST },
    };
    public AdminLevel adminLevel = AdminLevel.NOTENTERED;

    private static Dictionary<string, AdminLevel> MapAdminLevel = new Dictionary<string, AdminLevel>() {
    { "2", AdminLevel.LEVEL2 },
    };
    public PopulatedPlace populatedPlace = PopulatedPlace.NOTENTERED;

    private static Dictionary<string, PopulatedPlace> MapPopulatedPlace = new Dictionary<string, PopulatedPlace>() {
        { "city", PopulatedPlace.CITY },
        { "town", PopulatedPlace.TOWN },
        { "locality", PopulatedPlace.LOCALITY },
        { "hamlet", PopulatedPlace.HAMLET }
    };
    public Railway railway = Railway.NOTHING;

    private static Dictionary<string, Railway> MapRailway = new Dictionary<string, Railway>() { };
    public Natural natural = Natural.NOTHING;

    private static Dictionary<string, Natural> MapNatural = new Dictionary<string, Natural>() {
        { "fell", Natural.FELL },
        { "grassland", Natural.GRASSLAND },
        { "heath", Natural.HEATH },
        { "moor", Natural.MOOR },
        { "scrub", Natural.SCRUB },
        { "wetland", Natural.WETLAND },
        { "wood", Natural.WOOD },
        { "tree_row", Natural.TREE_ROW },
        { "bare_rock", Natural.BARE_ROCK },
        { "rock", Natural.ROCK },
        { "scree", Natural.SCREE },
        { "beach", Natural.BEACH },
        { "sand", Natural.SAND },
        { "water", Natural.WATER },
    };
    public Landuse landuse = Landuse.NOTHING;

    private static Dictionary<string, Landuse> MapLanduse = new Dictionary<string, Landuse>() {
        { "forest", Landuse.FOREST },
        { "orchard", Landuse.ORCHARD },
        { "residential", Landuse.RESIDENTIAL },
        { "cemetery", Landuse.CEMETERY },
        { "industrial", Landuse.INDUSTRIAL },
        { "commercial", Landuse.COMMERCIAL },
        { "square", Landuse.SQUARE },
        { "construction", Landuse.CONSTRUCTION },
        { "military", Landuse.MILITARY },
        { "quarry", Landuse.QUARRY },
        { "brownfield", Landuse.BROWNFIELD },
        { "farm", Landuse.FARM },
        { "meadow", Landuse.MEADOW },
        { "grass", Landuse.GRASS },
        { "greenfield", Landuse.GREENFIELD },
        { "recreation_ground", Landuse.RECREATION_GROUND },
        { "winter_sports", Landuse.WINTER_SPORTS },
        { "allotments", Landuse.ALLOTMENTS },
        { "reservoir", Landuse.RESERVOIR },
        { "basin", Landuse.BASIN },
    };
    public Building building = Building.NOTHING;

    private static Dictionary<string, Building> MapBuilding = new Dictionary<string, Building>() { };
    public Leisure leisure = Leisure.NOTHING;

    private static Dictionary<string, Leisure> MapLeisure = new Dictionary<string, Leisure>() { };
    public Amenity amenity = Amenity.NOTHING;

    private static Dictionary<string, Amenity> MapAmenity = new Dictionary<string, Amenity>() { };
    public string? name = null;

    public PropertiesClass(Dictionary<string, string> properties)
    {
        foreach (KeyValuePair<string, string> entered in properties)
        {
            if (entered.Key.StartsWith("highway"))
            {
                string helper = MapHighway.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapHighway.TryGetValue(helper, out highway);
            }
            else if (entered.Key.StartsWith("water"))
            {
                string helper = MapWater.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapWater.TryGetValue(helper, out water);
            }
            else if (entered.Key.StartsWith("boundary"))
            {
                string helper = MapBoundary.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapBoundary.TryGetValue(helper, out boundary);
            }
            else if (entered.Key == "admin_level")
            {
                string helper = MapAdminLevel.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapAdminLevel.TryGetValue(helper, out adminLevel);
            }
            else if (entered.Key.StartsWith("place"))
            {
                string helper = MapPopulatedPlace.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapPopulatedPlace.TryGetValue(helper, out populatedPlace);
            }
            else if (entered.Key.StartsWith("railway"))
            {
                string helper = MapRailway.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapRailway.TryGetValue(helper, out railway);
            }
            else if (entered.Key.StartsWith("natural"))
            {
                string helper = MapNatural.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapNatural.TryGetValue(helper, out natural);
            }
            else if (entered.Key.StartsWith("landuse"))
            {
                string helper = MapLanduse.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapLanduse.TryGetValue(helper, out landuse);
            }
            else if (entered.Key.StartsWith("building"))
            {
                string helper = MapBuilding.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapBuilding.TryGetValue(helper, out building);
            }
            else if (entered.Key.StartsWith("leisure"))
            {
                string helper = MapLeisure.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapLeisure.TryGetValue(helper, out leisure);
            }
            else if (entered.Key.StartsWith("amenity"))
            {
                string helper = MapAmenity.Keys.Where(p => entered.Value.StartsWith(p)).FirstOrDefault("");
                MapAmenity.TryGetValue(helper, out amenity);
            }
            else if (entered.Key.StartsWith("name"))
            {
                name = entered.Value;
            }
        }
    }


}


public readonly ref struct MapFeatureData
{
    public long Id { get; init; }

    public GeometryType Type { get; init; }
    public ReadOnlySpan<char> Label { get; init; }
    public ReadOnlySpan<Coordinate> Coordinates { get; init; }
    public PropertiesClass Properties { get; init; }
}

/// <summary>
///     Represents a file with map data organized in the following format:<br />
///     <see cref="FileHeader" /><br />
///     Array of <see cref="TileHeaderEntry" /> with <see cref="FileHeader.TileCount" /> records<br />
///     Array of tiles, each tile organized:<br />
///     <see cref="TileBlockHeader" /><br />
///     Array of <see cref="MapFeature" /> with <see cref="TileBlockHeader.FeaturesCount" /> at offset
///     <see cref="TileHeaderEntry.OffsetInBytes" /> + size of <see cref="TileBlockHeader" /> in bytes.<br />
///     Array of <see cref="Coordinate" /> with <see cref="TileBlockHeader.CoordinatesCount" /> at offset
///     <see cref="TileBlockHeader.CharactersOffsetInBytes" />.<br />
///     Array of <see cref="StringEntry" /> with <see cref="TileBlockHeader.StringCount" /> at offset
///     <see cref="TileBlockHeader.StringsOffsetInBytes" />.<br />
///     Array of <see cref="char" /> with <see cref="TileBlockHeader.CharactersCount" /> at offset
///     <see cref="TileBlockHeader.CharactersOffsetInBytes" />.<br />
/// </summary>
public unsafe class DataFile : IDisposable
{
    private readonly FileHeader* _fileHeader;
    private readonly MemoryMappedViewAccessor _mma;
    private readonly MemoryMappedFile _mmf;

    private readonly byte* _ptr;
    private readonly int CoordinateSizeInBytes = Marshal.SizeOf<Coordinate>();
    private readonly int FileHeaderSizeInBytes = Marshal.SizeOf<FileHeader>();
    private readonly int MapFeatureSizeInBytes = Marshal.SizeOf<MapFeature>();
    private readonly int StringEntrySizeInBytes = Marshal.SizeOf<StringEntry>();
    private readonly int TileBlockHeaderSizeInBytes = Marshal.SizeOf<TileBlockHeader>();
    private readonly int TileHeaderEntrySizeInBytes = Marshal.SizeOf<TileHeaderEntry>();

    private bool _disposedValue;

    public DataFile(string path)
    {
        _mmf = MemoryMappedFile.CreateFromFile(path);
        _mma = _mmf.CreateViewAccessor();
        _mma.SafeMemoryMappedViewHandle.AcquirePointer(ref _ptr);
        _fileHeader = (FileHeader*)_ptr;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _mma?.SafeMemoryMappedViewHandle.ReleasePointer();
                _mma?.Dispose();
                _mmf?.Dispose();
            }

            _disposedValue = true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private TileHeaderEntry* GetNthTileHeader(int i)
    {
        return (TileHeaderEntry*)(_ptr + i * TileHeaderEntrySizeInBytes + FileHeaderSizeInBytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private (TileBlockHeader? Tile, ulong TileOffset) GetTile(int tileId)
    {
        ulong tileOffset = 0;
        for (var i = 0; i < _fileHeader->TileCount; ++i)
        {
            var tileHeaderEntry = GetNthTileHeader(i);
            if (tileHeaderEntry->ID == tileId)
            {
                tileOffset = tileHeaderEntry->OffsetInBytes;
                return (*(TileBlockHeader*)(_ptr + tileOffset), tileOffset);
            }
        }

        return (null, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private MapFeature* GetFeature(int i, ulong offset)
    {
        return (MapFeature*)(_ptr + offset + TileBlockHeaderSizeInBytes + i * MapFeatureSizeInBytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private ReadOnlySpan<Coordinate> GetCoordinates(ulong coordinateOffset, int ithCoordinate, int coordinateCount)
    {
        return new ReadOnlySpan<Coordinate>(_ptr + coordinateOffset + ithCoordinate * CoordinateSizeInBytes, coordinateCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void GetString(ulong stringsOffset, ulong charsOffset, int i, out ReadOnlySpan<char> value)
    {
        var stringEntry = (StringEntry*)(_ptr + stringsOffset + i * StringEntrySizeInBytes);
        value = new ReadOnlySpan<char>(_ptr + charsOffset + stringEntry->Offset * 2, stringEntry->Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void GetProperty(ulong stringsOffset, ulong charsOffset, int i, out ReadOnlySpan<char> key, out ReadOnlySpan<char> value)
    {
        if (i % 2 != 0)
        {
            throw new ArgumentException("Properties are key-value pairs and start at even indices in the string list (i.e. i % 2 == 0)");
        }

        GetString(stringsOffset, charsOffset, i, out key);
        GetString(stringsOffset, charsOffset, i + 1, out value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void ForeachFeature(BoundingBox b, MapFeatureDelegate? action)
    {
        if (action == null)
        {
            return;
        }

        var tiles = TiligSystem.GetTilesForBoundingBox(b.MinLat, b.MinLon, b.MaxLat, b.MaxLon);
        for (var i = 0; i < tiles.Length; ++i)
        {
            var header = GetTile(tiles[i]);
            if (header.Tile == null)
            {
                continue;
            }
            for (var j = 0; j < header.Tile.Value.FeaturesCount; ++j)
            {
                var feature = GetFeature(j, header.TileOffset);
                var coordinates = GetCoordinates(header.Tile.Value.CoordinatesOffsetInBytes, feature->CoordinateOffset, feature->CoordinateCount);
                var isFeatureInBBox = false;

                for (var k = 0; k < coordinates.Length; ++k)
                {
                    if (b.Contains(coordinates[k]))
                    {
                        isFeatureInBBox = true;
                        break;
                    }
                }

                var label = ReadOnlySpan<char>.Empty;
                if (feature->LabelOffset >= 0)
                {
                    GetString(header.Tile.Value.StringsOffsetInBytes, header.Tile.Value.CharactersOffsetInBytes, feature->LabelOffset, out label);
                }

                if (isFeatureInBBox)
                {
                    var properties = new Dictionary<string, string>(feature->PropertyCount);
                    for (var p = 0; p < feature->PropertyCount; ++p)
                    {
                        GetProperty(header.Tile.Value.StringsOffsetInBytes, header.Tile.Value.CharactersOffsetInBytes, p * 2 + feature->PropertiesOffset, out var key, out var value);
                        properties.Add(key.ToString(), value.ToString());
                    }

                    if (!action(new MapFeatureData
                    {
                        Id = feature->Id,
                        Label = label,
                        Coordinates = coordinates,
                        Type = feature->GeometryType,
                        Properties = new PropertiesClass(properties)
                    }))
                    {
                        break;
                    }
                }
            }
        }
    }
}
