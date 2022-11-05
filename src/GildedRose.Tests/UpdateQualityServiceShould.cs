using GildedRose.Console;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GildedRose.Tests;

public class UpdateQualityServiceShould
{
    private readonly UpdateQualityService sut;

    public UpdateQualityServiceShould()
    {
        this.sut = new UpdateQualityService();
    }

    [Fact]
    public void Sulfuras_being_a_legendary_itemX2C_never_has_to_be_sold_or_decreases_in_Quality()
    {
        var items = new List<Item>
        {
            new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 10, Quality = 80 }
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Sulfuras, Hand of Ragnaros", 10, 80);
    }

    [Fact]
    public void Lower_SellIn_and_Quality()
    {
        var items = new List<Item>
        {
            new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 }
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "+5 Dexterity Vest", 9, 19);
    }

    [Theory]
    [InlineData("Elixir of the Mongoose", -2, 1)]
    [InlineData("+5 Dexterity Vest", 2, 0)]
    public void Never_lower_Quality_of_an_item_past_negative(
        string name, int sellIn, int quality
    )
    {
        var items = new List<Item>
        {
            new Item { Name = name, SellIn = sellIn, Quality = quality }
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), name, sellIn - 1, 0);
    }

    [Fact]
    public void Aged_Brie_increases_in_Quality_the_older_it_gets()
    {
        var items = new List<Item>
        {
            new Item {Name = "Aged Brie", SellIn = 2, Quality = 0}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Aged Brie", 1, 1);
    }

    [Fact]
    public void Quality_of_an_item_never_increases_more_than_50()
    {
        var items = new List<Item>
        {
            new Item {Name = "Aged Brie", SellIn = 2, Quality = 50}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Aged Brie", 1, 50);
    }

    [Theory]
    [InlineData("Elixir of the Mongoose", 0, 50)]
    [InlineData("+5 Dexterity Vest", -2, 2)]
    public void Once_the_sell_by_date_has_passedX2C_Quality_degrades_twice_as_fast(
        string name, int sellIn, int quality
    )
    {
        var items = new List<Item>
        {
            new Item {Name = name, SellIn = sellIn, Quality = quality}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), name, sellIn - 1, quality - 2);
    }

    [Fact]
    public void Backstage_passes_Quality_increases_as_itX27s_SellIn_value_approaches()
    {
        var items = new List<Item>
        {
            new Item {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 11, Quality = 0}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Backstage passes to a TAFKAL80ETC concert", 10, 1);
    }

    [Theory]
    [InlineData(10, 0, 2)]
    [InlineData(9, 48, 50)]
    [InlineData(9, 50, 50)]
    public void Backstage_passes_Quality_increases_by_2_when_SellIn_le_10_days(
        int sellIn, int quality, int expectedQuality
    )
    {
        var items = new List<Item>
        {
            new Item {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = sellIn, Quality = quality}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Backstage passes to a TAFKAL80ETC concert", sellIn - 1, expectedQuality);
    }

    [Theory]
    [InlineData(5, 0, 3)]
    [InlineData(4, 46, 49)]
    [InlineData(1, 49, 50)]
    public void Backstage_passes_Quality_increases_by_3_when_SellIn_le_5_days(
        int sellIn, int quality, int expectedQuality
    )
    {
        var items = new List<Item>
        {
            new Item {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = sellIn, Quality = quality}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Backstage passes to a TAFKAL80ETC concert", sellIn - 1, expectedQuality);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-1, 50, 0)]
    public void Backstage_passes_Quality_drop_to_0_after_concert(
        int sellIn, int quality, int expectedQuality
    )
    {
        var items = new List<Item>
        {
            new Item {Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = sellIn, Quality = quality}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Backstage passes to a TAFKAL80ETC concert", sellIn - 1, expectedQuality);
    }

    [Theory]
    [InlineData(3, 6, 4)]
    [InlineData(0, 6, 2)]
    public void Conjured_items_degrade_in_Quality_twice_as_fast_as_normal_items(
        int sellIn, int quality, int expectedQuality
    )
    {
        var items = new List<Item>
        {
            new Item {Name = "Conjured Mana Cake", SellIn = sellIn, Quality = quality}
        };

        var result = this.sut.Update(items);

        AssertItemEqual(result.First(), "Conjured Mana Cake", sellIn - 1, expectedQuality);
    }

    private static void AssertItemEqual(Item item, string name, int sellIn, int quality)
    {
        Assert.Equal(name, item.Name);
        Assert.Equal(sellIn, item.SellIn);
        Assert.Equal(quality, item.Quality);
    }
}