using GildedRose.Console;
using System.Collections.Generic;
using Xunit;

namespace GildedRose.Tests;

public class UpdateQualityServiceShould
{
    [Fact]
    public void Not_update_Sulfuras_that_already_at_peak_quality()
    {
        var items = new List<Item>
        {
            new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 }
        };

        new UpdateQualityService().Update(items);

        Assert.Equal("Sulfuras, Hand of Ragnaros", items[0].Name);
        Assert.Equal(0, items[0].SellIn);
        Assert.Equal(80, items[0].Quality);
    }
}