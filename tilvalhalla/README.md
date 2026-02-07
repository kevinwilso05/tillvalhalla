# TillValhalla - Quality of Life Mod for Valheim

[![Version](https://img.shields.io/badge/version-2.4.19-blue.svg)](https://github.com/kevinwilso05/tillvalhalla)
[![Valheim](https://img.shields.io/badge/Valheim-Ashlands_Compatible-green.svg)](https://store.steampowered.com/app/892970/Valheim/)

**Author:** Kevinwilso00

A comprehensive quality of life mod that adds numerous configurable improvements to Valheim, enhancing your gameplay experience without breaking game balance.

---

## 🌟 Features

### 📦 Inventory & Storage
- **Expanded Player Inventory** - Configure up to 20 rows (default 4-20 configurable)
- **Enhanced Tombstone System** - Graves automatically match your inventory size, preventing item loss on death (even on servers!)
- **Expanded Containers** - Resize wood chests, iron chests, black metal chests, and personal chests
- **Vehicle Storage** - Customize storage sizes for Karves, Viking Ships, and Carts
- **Top-to-Bottom Fill** - Option to fill inventory from top to bottom
- **Stack Size Multiplier** - Modify stack sizes for consumables, materials, and ammo
- **Quick Inventory Swap** - Use hotkey (default `) to swap inventory rows

### 🔨 Crafting & Building
- **Craft from Containers** - Pull materials from nearby chests when crafting
- **Workbench Range** - Configure crafting station range for containers
- **Build Anywhere** - Remove crafting station roof requirements
- **No Build Restrictions** - Disable placement restrictions for building
- **Upgrade Spacing** - Remove space requirements for crafting station upgrades

### ⚒️ Smelting & Production
- **Enhanced Smelters** - Configure production speed and capacity for:
  - Kilns
  - Smelters
  - Blast Furnaces
  - Windmills
  - Spinning Wheels
  - Eitr Refineries
- **Auto-Fuel System** - Automatically pull fuel and ore from nearby containers
- **Auto-Deposit** - Finished products automatically go to nearby chests
- **Selective Processing** - Prevent processing specific wood types in kilns
- **Debug Logging** - Toggle detailed logging for smelter operations

### 🔥 Fireplace Management
- **Infinite Fuel** - Configure fireplaces to never run out of fuel
- **Auto-Fill** - Fireplaces automatically pull wood from nearby Firewood Chests
- **Custom Firewood Chest** - Special chest type for fireplace fuel storage

### 🌱 Farming & Gathering
- **Plant Configuration** - Modify growth speed and area requirements
- **Ground Requirements** - Toggle strict ground placement rules
- **Enhanced Drop Rates** - Configure drop rates for:
  - Wood, Fine Wood, Core Wood, Elder Bark, Yggdrasil Wood
  - Stone, Black Marble
  - Ores (Iron, Tin, Copper, Silver, Chitin, Black Metal)
  - Soft Tissue
  - All meat types (Deer, Boar, Lox, Wolf, Serpent, Chicken, Neck, Hare, Seeker)
  - All hide types (Deer Hide, Scale Hide, Lox Pelt, Wolf Pelt, Leather Scraps)
  - Greydwarf Eyes, Bloodbags
- **Minimum Drop Guarantees** - Ensure at least one drop for meat and hides

### 🛡️ Player Configuration
- **Base Stats** - Modify maximum weight, HP, and stamina
- **Megingjord Buff** - Customize the carry weight bonus
- **No Wet Debuff** - Disable rain wetness effect
- **Movement Speed** - Configure movement penalties from equipment:
  - Completely disable all movement penalties
  - Or apply percentage-based modifications
- **Rested Bonus** - Spawn with configurable rested effect duration
- **Comfort Radius** - Adjust comfort detection range

### 🚢 Ship Enhancements
- **Warmth Effect** - Ships provide warmth in cold biomes (configurable)
- **Ship Stats** - Customize ship health, speed, and other properties

### 🎒 Item Management
- **Disable Teleport Prevention** - Allow teleporting with all items
- **Movement Modifier Control**:
  - Toggle to completely remove all movement speed penalties
  - Or use percentage-based modification for fine-tuning
- **Quick Container Sorting** - Move all related items to containers efficiently

### 🎵 Audio Customization
- **Custom SFX Injection** - Replace or add custom sound effects to the game

### 🎮 Gameplay
- **Beehive Improvements** - Configure production speed and capacity
- **Cooking Station** - Modify cooking speeds and capacity
- **Sap Collector** - Customize sap collection rates
- **Demister** - Configure demister range and effects
- **Custom First Spawn Message** - Personalize the welcome message

---

## 📥 Installation

### Requirements
- [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) (Latest version)
- [Jötunn](https://valheim.thunderstore.io/package/ValheimModding/Jotunn/) (Latest version)

### Manual Installation
1. Install BepInEx and Jötunn
2. Download the latest release of TillValhalla
3. Extract the entire folder into your `BepInEx/Plugins` directory
4. Launch Valheim

### Configuration
All features are configurable via the config file located at:
```
BepInEx/config/kwilson.TillValhalla.cfg
```

Or use a configuration manager mod for in-game settings adjustment.

---

## 🔧 Key Features Explained

### Craft from Containers
When enabled, you can craft items using materials from nearby chests without manually moving them to your inventory. Configure the range and whether to include workbench storage.

### Tombstone Protection
The mod now ensures your tombstone always matches your actual inventory size, even when playing on servers. This prevents item loss when the server has a different inventory configuration than your client.

### Movement Modifier System
Choose between:
- **Complete Removal**: Set `disableMovementModifier = true` to remove all movement penalties
- **Percentage Adjustment**: Use `movementmodifier = -50` to reduce penalties by 50%, or positive values to increase them

### Auto-Fuel & Auto-Deposit
Smelters, kilns, and other production buildings can automatically:
- Pull fuel and materials from nearby chests (configurable range)
- Deposit finished products into nearby chests
- Stop auto-fueling when production thresholds are met

---

## 🎯 Server Compatibility

**Network Compatibility:** `EveryoneMustHaveMod`

All players connecting to a server **must have the mod installed** with compatible configurations. Server administrators should ensure all players use the same version.

### Server Notes
- Craft from containers requires client-side configuration
- Inventory size is synchronized from the server
- Tombstones correctly handle server/client inventory differences

---

## 🐛 Bug Reports & Support

Please report issues on the [GitHub Issues page](https://github.com/kevinwilso05/tillvalhalla/issues).

When reporting bugs, please include:
- Mod version
- Valheim version
- BepInEx and Jötunn versions
- Relevant configuration settings
- Log files (if applicable)

---


### Latest (v2.4.19)
- Fixed tombstone inventory size to match player's actual inventory at death
- Added debug logging toggle for smelter operations
- Improved craft from containers functionality
- Enhanced movement modifier system with percentage-based control

---

## 🙏 Credits

**Author:** Kevinwilso00

Built with [BepInEx](https://github.com/BepInEx/BepInEx) and [Jötunn](https://github.com/Valheim-Modding/Jotunn)

Special thanks to the Valheim modding community for their tools and support.

---

## 🔗 Links

- [GitHub Repository](https://github.com/kevinwilso05/tillvalhalla)
