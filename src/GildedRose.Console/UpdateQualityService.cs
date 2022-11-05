namespace GildedRose.Console;

public abstract class Product
{
    public string Name { get; }
    public int SellIn { get; private set; }
    public int Quality { get; private set; }

    protected Product(string name, int sellIn, int quality) =>
        (Name, SellIn, Quality) = (name, sellIn, quality);

    public void IncreaseQuality()
    {
        if (Quality < 50)
        {
            Quality = Quality + 1;
        }
    }

    public void DegradeQuality()
    {
        if (Quality > 0)
        {
            Quality = Quality - 1;
        }
    }

    public void DropQuality()
    {
        Quality = 0;
    }

    public void ForwardDay()
    {
        SellIn = SellIn - 1;
    }

    public abstract Product NextDay();

    public static Product Create(Item item)
    {
        switch (item.Name.ToLowerInvariant())
        {
            case "aged brie":
                return new AgedBrieItem(item.Name, item.SellIn, item.Quality);
            case "backstage passes to a tafkal80etc concert":
                return new BackstagePassItem(item.Name, item.SellIn, item.Quality);
            case "sulfuras, hand of ragnaros":
                return new LegendaryItem(item.Name, item.SellIn, item.Quality);
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
        var result = new RegularItem(Name, SellIn, Quality);
        result.DegradeQuality();
        result.ForwardDay();
        if (result.SellIn < 0)
        {
            result.DegradeQuality();
        }
        return result;
    }
}

public sealed class AgedBrieItem : Product
{
    public AgedBrieItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay()
    {
        var result = new AgedBrieItem(Name, SellIn, Quality);
        result.IncreaseQuality();
        result.ForwardDay();
        if (result.SellIn < 0)
        {
            result.IncreaseQuality();
        }
        return result;
    }
}

public sealed class BackstagePassItem : Product
{
    public BackstagePassItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay()
    {
        var result = new BackstagePassItem(Name, SellIn, Quality);
        result.IncreaseQuality();
        if (result.SellIn <= 10)
        {
            result.IncreaseQuality();
        }

        if (result.SellIn <= 5)
        {
            result.IncreaseQuality();
        }
        result.ForwardDay();

        if (result.SellIn < 0)
        {
            result.DropQuality();
        }
        return result;
    }
}

public sealed class LegendaryItem : Product
{
    public LegendaryItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }

    public override Product NextDay() =>
        new LegendaryItem(Name, SellIn, Quality);
}

public class UpdateQualityService
{
    public IEnumerable<Item> Update(IEnumerable<Item> items)
    {
        return items
            .Select(Product.Create)
            .Select(x => UpdateItem(x));
    }

    private static Item UpdateItem(Product item)
    {
        var itemNextDay = item.NextDay();
        return new Item { Name = itemNextDay.Name, SellIn = itemNextDay.SellIn, Quality = itemNextDay.Quality };
    }
}