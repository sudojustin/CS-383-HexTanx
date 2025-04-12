classDiagram
    %% ===== Existing Classes ===== %%
    class PlaceTile {
        +bool runOnStart
        -int choseTerrain
        -int isTerrain
        -float hexwidth
        +float hexHeight
        +float heightspacing
        +int width
        +int height
        +float ypos
        +float xpos
        +float offset
        +float offset2
        +int min
        +int max
        +GameObject Tile
        +GameObject Terrain1
        +GameObject Terrain2
        +GameObject Terrain3
        +Vector3[,] Grid
        -Camera cam
        +void Start()
        +void MakeMap(int width, int height)
        -void TileFactory(int x, int y)
    }

    class Tiles {
        +Color Base
        +Color Offset
        +Color hightlight
        +SpriteRenderer rend
        +Color playerhere
        +void TurnOff()
    }

    class Terrains {
        -bool isactive
        -GameObject playertankOBJ
        -GameObject enemyTank
        -int playerHealthshown
        -int enemyHealthShown
        -TerrainDamageBC Damage
        -PlayerTank playertc
        -TankType enemytc
        +void Start()
        +void Update()
        -void takeDamage()
        -void enemyTakeDamage()
        -void getPlayer()
        -void getEnemy()
    }

    class TerrainDamageBC {
        +int getDamage()
    }

    class TerrainDamage {
        +int getDamage()
    }

    class EarthTerrain {
        -List~GameObject~ Bullets
        +void Start()
        +void Update()
        -void getProjectile()
    }

    class TerrainIce {
        -bool isactive
        -GameObject playertankOBJ
        -GameObject enemyTank
        -int playerActionPointsCurrent
        -int enemyHealthShown
        -TerrainDamageBC Damage
        -PlayerTank playertc
        -TankType enemytc
        +void Start()
        +void Update()
        -void getPlayer()
        -void getEnemy()
        -void SlowMovement()
    }

    %% ===== Inheritance ===== %%
    Terrains --|> Tiles
    EarthTerrain --|> Tiles
    TerrainIce --|> Tiles
    TerrainDamage --|> TerrainDamageBC

    %% ===== Dependencies ===== %%
    Terrains --> TerrainDamageBC
    TerrainIce --> TerrainDamageBC
    PlaceTile --> Tiles
