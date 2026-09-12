<h1>2.4.21</h1>
<ul>
<li> Added ValuableItems configuration for scaling valuable drops (Amber, AmberPearl, AncientCoin/Coins, Ruby). </li>
<li> Improved valuable drop handling across drop tables and character drop processing. </li>
<li> Updated smelter and fireplace RPC calls to match current game RPC signatures. </li>
<li> Improved craft-from-containers consumption logic in player crafting flows. </li>
<li> Hardened cooking station and smelter patching logic for improved compatibility with recent game changes. </li>
<li> Updated dependency targets to BepInExPack 5.4.2350 and Jotunn 2.30.0. </li>
</ul>
<h1>2.4.19</h1>
<ul>
<li> Fixed tombstone inventory size to match player's actual inventory at death, resolving server/client config mismatch issues. </li>
<li> Tombstones now correctly store all items when playing on servers with different inventory configurations. </li>
<li> Enhanced tombstone creation to use runtime inventory height instead of local configuration values. </li>
<li> Added global debug logging toggle (enableDebugLogging) for smelter and fireplace operations. </li>
<li> Added disableMovementModifier configuration to completely remove all movement speed penalties from equipped items. </li>
<li> Reworked movementmodifier to use percentage-based adjustments (default: 0 for disabled). </li>
<li> Movement modifier now properly applies percentage modifications to items with different base penalties. </li>
<li> Improved craft from containers functionality with refined item movement modifier logic. </li>
<li> Updated configuration descriptions for clarity and better user experience. </li>
</ul>
<h1>2.4.18</h1>
<ul>
<li> Changed ship warmth effect setting to false by default. </li>
<li> Changed player rested on start setting to false by default. </li>
<li> Added duration configuration for rested on start to be able to set how long the rested bonus on spawn lasts. </li>
</ul>
<h1>2.4.17</h1>
<ul>
<li> Compliled against Valheim v0.221.10 Ashlands update. </li>
<li> Added fire effect to ships to allow for warmth. </li>
<li> Added configuration to give a base amount of rested on spawn into the server. </li>

</ul>
