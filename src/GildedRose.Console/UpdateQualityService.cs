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
}

public sealed class AgedBrieItem : Product
{
    public AgedBrieItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }
}

public sealed class BackstagePassItem : Product
{
    public BackstagePassItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }
}

public sealed class LegendaryItem : Product
{
    public LegendaryItem(string name, int sellIn, int quality) : base(name, sellIn, quality)
    { }
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
        if (item is AgedBrieItem or BackstagePassItem)
        {
            item.IncreaseQuality();

            if (item is BackstagePassItem)
            {
                if (item.SellIn <= 10)
                {
                    item.IncreaseQuality();
                }

                if (item.SellIn <= 5)
                {
                    item.IncreaseQuality();
                }
            }
        }
        else if (item is not LegendaryItem)
        {
            item.DegradeQuality();
        }

        if (item is not LegendaryItem)
        {
            item.ForwardDay();
        }

        if (item.SellIn < 0)
        {
            if (item is AgedBrieItem)
            {
                item.IncreaseQuality();
            }
            else if (item is BackstagePassItem)
            {
                item.DropQuality();
            }
            else if (item is not LegendaryItem)
            {
                item.DegradeQuality();
            }
        }
        return new Item { Name = item.Name, SellIn = item.SellIn, Quality = item.Quality };
    }
}