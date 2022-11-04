using System.Collections.Generic;

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
            if (item.Quality < 50)
            {
                item.Quality = item.Quality + 1;

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
        }
        else
        {
            if (item.Quality > 0)
            {
                if (item.Name != "Sulfuras, Hand of Ragnaros")
                {
                    item.Quality = item.Quality - 1;
                }
            }
        }

        if (item.Name != "Sulfuras, Hand of Ragnaros")
        {
            item.SellIn = item.SellIn - 1;
        }

        if (item.SellIn < 0)
        {
            if (item.Name != "Aged Brie")
            {
                if (item.Name != "Backstage passes to a TAFKAL80ETC concert")
                {
                    if (item.Quality > 0)
                    {
                        if (item.Name != "Sulfuras, Hand of Ragnaros")
                        {
                            item.Quality = item.Quality - 1;
                        }
                    }
                }
                else
                {
                    item.Quality = item.Quality - item.Quality;
                }
            }
            else
            {
                IncreaseQuality(item);
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
}