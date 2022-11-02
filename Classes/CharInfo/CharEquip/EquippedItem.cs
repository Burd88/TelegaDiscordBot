using System.Collections.Generic;

namespace DiscordBot
{
    public class EquippedItem
    {
        public ItemLoot item { get; set; }
        public Slot slot { get; set; }
        public int quantity { get; set; }
        public int context { get; set; }
        public List<int> bonus_list { get; set; }
        public Quality quality { get; set; }
        public string name { get; set; }
        public int modified_appearance_id { get; set; }
        public Media media { get; set; }
        public ItemClass item_class { get; set; }
        public ItemSubclass item_subclass { get; set; }
        public InventoryType inventory_type { get; set; }
        public Binding binding { get; set; }
        public ArmorChar armor { get; set; }
        public List<Stat> stats { get; set; }
        public SellPrice sell_price { get; set; }
        public Requirements requirements { get; set; }
        public Set set { get; set; }
        public Level level { get; set; }
        public Transmog transmog { get; set; }
        public Durability durability { get; set; }
        public NameDescription name_description { get; set; }
        public List<Socket> sockets { get; set; }
        public bool? is_subclass_hidden { get; set; }
        public List<Spells> spells { get; set; }
        public List<Enchantment> enchantments { get; set; }
        public string limit_category { get; set; }
        public string description { get; set; }
        public string unique_equipped { get; set; }
        public Weapon weapon { get; set; }
    }
}
