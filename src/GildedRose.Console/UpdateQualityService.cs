namespace GildedRose.Console;

public class UpdateQualityService
{
    public void Update(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            UpdateItem(item);
        }
    }

    private static void UpdateItem(Item item)
    {
        if (item.Name == "Aged Brie" || item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            IncreaseQuality(item);

            if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
            {
                if (item.SellIn <= 10)
                {
                    IncreaseQuality(item);
                }

                if (item.SellIn <= 5)
                {
                    IncreaseQuality(item);
                }
            }
        }
        else if (item.Name != "Sulfuras, Hand of Ragnaros")
        {
            DegradeQuality(item);
        }

        if (item.Name != "Sulfuras, Hand of Ragnaros")
        {
            item.SellIn = item.SellIn - 1;
        }

        if (item.SellIn < 0)
        {
            if (item.Name == "Aged Brie")
            {
                IncreaseQuality(item);
            }
            else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
            {
                DropQuality(item);
            }
            else if (item.Name != "Sulfuras, Hand of Ragnaros")
            {
                DegradeQuality(item);
            }
        }
    }

    private static void IncreaseQuality(Item item)
    {
        if (item.Quality < 50)
        {
            item.Quality = item.Quality + 1;
        }
    }

    private static void DegradeQuality(Item item)
    {
        if (item.Quality > 0)
        {
            item.Quality = item.Quality - 1;
        }
    }

    private static void DropQuality(Item item)
    {
        item.Quality = item.Quality - item.Quality;
    }
}