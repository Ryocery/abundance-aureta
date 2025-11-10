# Abundance

A very simple Aeruta mod that multiplies money and material drops.

## Features

- Configurable money multiplier (default: 2.0x)
- Configurable monster material multiplier (default: 2.0x)

## Configuration

The mod creates a config file at `BepInEx/config/com.ryocery.abundance_aeruta.cfg`:
```ini
[Money Settings]
EnableMoneyMultiplier = true
MoneyMultiplier = 2.0

[Material Settings]
EnableMaterialMultiplier = true
MaterialMultiplier = 2.0
```

## Installation

1. Install **BepInEx IL2CPP** branch for Aeruta
2. Extract the mod folder into `BepInEx/plugins`
3. Launch the game to create the config
4. Adjust config values as desired