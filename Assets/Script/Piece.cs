using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece
{

    // public Vector3Int[] position  = {Vector3Int.zero, Vector3Int.zero, Vector3Int.zero, Vector3Int.zero};
    public TileBase tile;
    private int rotation = 0;

    private Dictionary<TetrominoShape, Vector3Int[][]> positions = new Dictionary<TetrominoShape, Vector3Int[][]>{
        // |4          |        |1           |     
        // |3       12 |34      |2        43 |21
        // |2          |        |3           |     
        // |1          |        |4           |       
        // .----       .----    .----        .----  
        {TetrominoShape.I, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(0,1,0), new Vector3Int(0,2,0), new Vector3Int(0,3,0)},
            new Vector3Int[]{new Vector3Int(-1,2,0), new Vector3Int(0,2,0), new Vector3Int(1,2,0), new Vector3Int(2,2,0)},
            new Vector3Int[]{new Vector3Int(0,3,0), new Vector3Int(0,2,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0)},
            new Vector3Int[]{new Vector3Int(1,2,0), new Vector3Int(0,2,0), new Vector3Int(-1,2,0), new Vector3Int(-2,2,0)},
            }
        },
        // |      |      |      |
        // |      |      |      |
        // |43    |14    |21    |32
        // |12    |23    |34    |41
        // .----  .----  .----  .----
        {TetrominoShape.O, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(1,0,0), new Vector3Int(1,1,0), new Vector3Int(0,1,0)},
            new Vector3Int[]{new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(1,0,0), new Vector3Int(1,1,0)},
            new Vector3Int[]{new Vector3Int(1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(1,0,0)},
            new Vector3Int[]{new Vector3Int(1,0,0), new Vector3Int(1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0)},
            }
        },
        // |      |      |     |
        // | 4    |      |21   |
        // | 3    |1     |3    |432
        // |12    |234   |4    |  1
        // .----  .----  .---- .----
        {TetrominoShape.J, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(1,0,0), new Vector3Int(1,1,0), new Vector3Int(1,2,0)},
            new Vector3Int[]{new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(1,0,0), new Vector3Int(2,0,0)},
            new Vector3Int[]{new Vector3Int(1,2,0), new Vector3Int(0,2,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0)},
            new Vector3Int[]{new Vector3Int(2,0,0), new Vector3Int(2,1,0), new Vector3Int(1,1,0), new Vector3Int(0,1,0)},
            }
        },
        // |      |       |       |
        // |1     |       |43     |
        // |2     |321    | 2     |  4
        // |34    |4      | 1     |123
        // .----  .----   .----   .----
        {TetrominoShape.L, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(0,2,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(1,0,0)},
            new Vector3Int[]{new Vector3Int(2,1,0), new Vector3Int(1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0)},
            new Vector3Int[]{new Vector3Int(1,0,0), new Vector3Int(1,1,0), new Vector3Int(1,2,0), new Vector3Int(0,2,0)},
            new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(1,0,0), new Vector3Int(2,0,0), new Vector3Int(2,1,0)},
            }
        },
        //   |         |        |        |
        //   |        4|        |       1| 
        //   |21      3|2       |34     2|3
        //  4|3        |1      1|2       |4
        // --.---    --.---   --.---   --.---
        {TetrominoShape.S, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(-1,0,0)},
            new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(0,1,0), new Vector3Int(-1,1,0), new Vector3Int(-1,2,0)},
            new Vector3Int[]{new Vector3Int(-1,0,0), new Vector3Int(0,0,0), new Vector3Int(0,1,0), new Vector3Int(1,1,0)},
            new Vector3Int[]{new Vector3Int(-1,2,0), new Vector3Int(-1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0)},
            }
        },
        //   |        |       |      |
        //   |        |2      |      |4
        //  2|34     1|3      |1     |31
        //   |1       |4     4|32    |2
        // --.---   --.---  --.--- --.---
        {TetrominoShape.T, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(-1,1,0), new Vector3Int(0,1,0), new Vector3Int(1,1,0)},
            new Vector3Int[]{new Vector3Int(-1,1,0), new Vector3Int(0,2,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0)},
            new Vector3Int[]{new Vector3Int(0,1,0), new Vector3Int(1,0,0), new Vector3Int(0,0,0), new Vector3Int(-1,0,0)},
            new Vector3Int[]{new Vector3Int(1,1,0), new Vector3Int(0,0,0), new Vector3Int(0,1,0), new Vector3Int(0,2,0)},
            }
        },
        //   |       |        |       |
        //   |       |1       |       |4
        //  1|2     3|2      4|3     2|3
        //   |34    4|        |21    1|
        // --.---  --.---   --.---  --.---
        {TetrominoShape.Z, new Vector3Int[][]{
            new Vector3Int[]{new Vector3Int(-1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(1,0,0)},
            new Vector3Int[]{new Vector3Int(0,2,0), new Vector3Int(0,1,0), new Vector3Int(-1,1,0), new Vector3Int(-1,0,0)},
            new Vector3Int[]{new Vector3Int(1,0,0), new Vector3Int(0,0,0), new Vector3Int(0,1,0), new Vector3Int(-1,1,0)},
            new Vector3Int[]{new Vector3Int(-1,0,0), new Vector3Int(-1,1,0), new Vector3Int(0,1,0), new Vector3Int(0,2,0)},
            }
        },
    };

    public TetrominoShape shape;
    public Piece(TetrominoEntry[] tiles) {
        int i = Random.Range(0, tiles.Length);
        // position = positions[tiles[i].shape][0];
        tile = tiles[i].tile;
        shape = tiles[i].shape;
    }

    public Vector3Int[] getPosition() {
        return positions[shape][rotation];
    }

    public Vector3Int[] downMostPosition() {
        Vector3Int[] position = getPosition();
        switch (rotation) {
            case 0:
                switch (shape) {
                    case TetrominoShape.I:
                        return new Vector3Int[]{position[0]};
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[2], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    default:
                        return null;
                }
            case 1:
                switch (shape) {
                    case TetrominoShape.I:
                        return position;
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[1], position[2]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[1], position[2], position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[2]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[1], position[3]};
                    default:
                        return null;
                }
            case 2:
                switch (shape) {
                    case TetrominoShape.I:
                        return new Vector3Int[]{position[3]};
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[2], position[3]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[1], position[2], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    default:
                        return null;
                }
            case 3:
                switch (shape) {
                    case TetrominoShape.I:
                        return position;
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[1], position[2]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[1], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[2]};
                    default:
                        return null;
                }
            default: return null;
        }
    }

    public Vector3Int[] rightMostPosition() {
        Vector3Int[] position = getPosition();
        switch (rotation) {
            case 0:
                switch (shape) {
                    case TetrominoShape.I:
                        return position;
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[1], position[2]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[1], position[2],position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[2]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[1], position[3]};
                    default:
                        return null;
                }
            case 1:
                switch (shape) {
                    case TetrominoShape.I:
                        return new Vector3Int[]{position[3]};
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[2], position[3]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[1], position[2], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    default:
                        return null;
                }
            case 2:
                switch (shape) {
                    case TetrominoShape.I:
                        return position;
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[2],position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[1], position[2]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[1], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[2]};
                    default:
                        return null;
                }
            case 3:
                switch (shape) {
                    case TetrominoShape.I:
                        return new Vector3Int[]{position[0]};
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[2], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    default:
                        return null;
                }
            default: return null;
        }
    }

    public Vector3Int[] leftMostPosition() {
        Vector3Int[] position = getPosition();
        switch (rotation) {
            case 0:
                switch (shape) {
                    case TetrominoShape.I:
                        return position;
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[2],position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[1], position[2]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[1], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[2]};
                    default:
                        return null;
                }
            case 1:
                switch (shape) {
                    case TetrominoShape.I:
                        return new Vector3Int[]{position[0]};
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[1]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[2], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[2], position[3]};
                    default:
                        return null;
                }
            case 2:
                switch (shape) {
                    case TetrominoShape.I:
                        return position;
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[1], position[2]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[1], position[2],position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[2]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[1], position[3]};
                    default:
                        return null;
                }
            case 3:
                switch (shape) {
                    case TetrominoShape.I:
                        return new Vector3Int[]{position[3]};
                    case TetrominoShape.O:
                        return new Vector3Int[]{position[2], position[3]};
                    case TetrominoShape.J:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.L:
                        return new Vector3Int[]{position[0], position[3]};
                    case TetrominoShape.S:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    case TetrominoShape.T:
                        return new Vector3Int[]{position[1], position[2], position[3]};
                    case TetrominoShape.Z:
                        return new Vector3Int[]{position[0], position[1], position[3]};
                    default:
                        return null;
                }
            default: return null;
        }
    }

    

    public void rotate(Vector3Int direction) {
        //逆时针
        if (direction == Vector3Int.left) {
            rotation += 3;
            rotation %= 4;
        }
        if (direction == Vector3Int.right) {
            rotation += 1;
            rotation %= 4;
        }

    }
}
