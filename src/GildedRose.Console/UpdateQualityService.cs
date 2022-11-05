namespace GildedRose.Console;

public abstract class Product
{
    public string Name { get; }
    public int SellIn { get; }
    public int Quality { get; }

    public Product(string name, int sellIn, int quality) =>
        (Name, SellIn, Quality) = (name, sellIn, quality);

    public abstract Product NextDay();

    public static Product Create(Item item)
    {
        switch (item.Name.ToLowerInvariant())
        {
            case "aged brie":
                return new AgedBrie(item.Name, item.SellIn, item.Quality);
            case "backstage passes to a tafkal80etc concert":
                return new BackstagePass(item.Name, item.SellIn, item.Quality);
            case "sulfuras, hand of ragnaros":
                return new LegendaryItem(item.Name, item.SellIn);
            case "conjured mana cake":
                return new ConjuredItem(item.Name, item.SellIn, item.Quality);
            default:
                return new RegularItem(item.Name, item.SellIn, item.Quality);
        }
    }
}

public sealed class RegularItem : Product
{
    public RegularItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay()
    {
        var quality = SellIn switch
        {
            <= 0 => Quality - 2,
            _ => Quality - 1
        };
        return new RegularItem(Name, SellIn - 1, Math.Min(Math.Max(0, quality), 50));
    }
}

public sealed class ConjuredItem : Product
{
    public ConjuredItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay()
    {
        var quality = SellIn switch
        {
            <= 0 => Quality - 4,
            _ => Quality - 2
        };
        return new ConjuredItem(Name, SellIn - 1, Math.Min(Math.Max(0, quality), 50));
    }
}

public sealed class AgedBrie : Product
{
    public AgedBrie(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay()
    {
        var quality = SellIn switch
        {
            <= 0 => Quality + 2,
            _ => Quality + 1
        };
        return new AgedBrie(Name, SellIn - 1, Math.Min(Math.Max(0, quality), 50));
    }
}

public sealed class BackstagePass : Product
{
    public BackstagePass(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay()
    {
        int quality = SellIn switch
        {
            <= 0 => 0,
            <= 5 => Quality + 3,
            <= 10 => Quality + 2,
            _ => Quality + 1
        };
        return new BackstagePass(Name, SellIn - 1, Math.Min(Math.Max(0, quality), 50));
    }
}

public sealed class LegendaryItem : Product
{
    public LegendaryItem(string name, int sellIn) : base(name, sellIn, 80)
    { }

    public override Product NextDay() =>
        new LegendaryItem(Name, SellIn);
}

public class UpdateQualityService
{
    public IEnumerable<Item> Update(IEnumerable<Item> items) =>
        items.Select(Product.Create)
            .Select(x => x.NextDay())
            .Select(x => new Item { Name = x.Name, SellIn = x.SellIn, Quality = x.Quality });
}