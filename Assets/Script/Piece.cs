using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece
{

    public Vector3Int[] position  = {Vector3Int.zero, Vector3Int.zero, Vector3Int.zero, Vector3Int.zero};
    public TileBase tile;

    private Dictionary<TetrominoShape, Vector3Int[]> positions = new Dictionary<TetrominoShape, Vector3Int[]>{
        {TetrominoShape.I, new Vector3Int[]{new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(0,-1,0), new Vector3Int(0,-2,0)} },
        {TetrominoShape.O, new Vector3Int[]{new Vector3Int(-1,0,0), new Vector3Int(0,0,0), new Vector3Int(0,-1,0), new Vector3Int(-1,-1,0)} },
        {TetrominoShape.J, new Vector3Int[]{new Vector3Int(0,1,0), new Vector3Int(0,0,0), new Vector3Int(0,-1,0), new Vector3Int(-1,-1,0)} },
        {TetrominoShape.L, new Vector3Int[]{new Vector3Int(-1,1,0), new Vector3Int(-1,0,0), new Vector3Int(-1,-1,0), new Vector3Int(-1,0,0)} },
        {TetrominoShape.S, new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(1,0,0), new Vector3Int(-1,-1,0), new Vector3Int(-2,-1,0)} },
        {TetrominoShape.T, new Vector3Int[]{new Vector3Int(0,0,0), new Vector3Int(1,-1,0), new Vector3Int(1,0,0), new Vector3Int(1,1,0)} },
        {TetrominoShape.Z, new Vector3Int[]{new Vector3Int(-2,0,0), new Vector3Int(-1,0,0), new Vector3Int(0,-1,0), new Vector3Int(-1,-1,0)} },
    };

    public TetrominoShape shape;
    public Piece(TetrominoEntry[] tiles) {
        int i = Random.Range(0, tiles.Length);
        position = positions[tiles[i].shape];
        tile = tiles[i].tile;
    }

    public Vector3Int[] leftMostPosition() {
        switch (shape) {
            case TetrominoShape.I:
                return position;
            case TetrominoShape.O:
                return new Vector3Int[]{position[0], position[3]};
            case TetrominoShape.J:
                return new Vector3Int[]{position[0], position[1], position[3]};
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
    }

    public Vector3Int[] rightMostPosition() {
        switch (shape) {
            case TetrominoShape.I:
                return position;
            case TetrominoShape.O:
                return new Vector3Int[]{position[1], position[2]};
            case TetrominoShape.J:
                return new Vector3Int[]{position[0], position[1],position[2]};
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
    }

    public Vector3Int[] downMostPosition() {
        switch (shape) {
            case TetrominoShape.I:
                return new Vector3Int[]{position[3]};
            case TetrominoShape.O:
                return new Vector3Int[]{position[2], position[3]};
            case TetrominoShape.J:
                return new Vector3Int[]{position[2], position[3]};
            case TetrominoShape.L:
                return new Vector3Int[]{position[2], position[3]};
            case TetrominoShape.S:
                return new Vector3Int[]{position[2], position[3]};
            case TetrominoShape.T:
                return new Vector3Int[]{position[0], position[1], position[3]};
            case TetrominoShape.Z:
                return new Vector3Int[]{position[2], position[3]};
            default:
                return null;
        }
    }
    

    public void rotate(Vector3Int direction) {
        //逆时针
        if (direction == Vector3Int.left) {
            position = position.Select(p => Rotate90DegreesCCW(p)).ToArray();
        }
        if (direction == Vector3Int.right) {
            position = position.Select(p => Rotate90DegreesCW(p)).ToArray();
        }

    }
    //逆时针
    Vector3Int Rotate90DegreesCCW(Vector3Int position)
    {
        //获取中心点的坐标，先放大10倍
        int x = position.x * 10 + 5;
        int y = position.y * 10 + 5;
        //旋转
        int x1 = (-y - 5) / 10;
        int y1 = (x - 5) / 10;
        return new Vector3Int(x1, y1, position.z);
    }

    //顺时针
    Vector3Int Rotate90DegreesCW(Vector3Int position)
    {
        //获取中心点的坐标，先放大10倍
        int x = position.x * 10 + 5;
        int y = position.y * 10 + 5;
        //旋转
        int x1 = (y - 5) / 10;
        int y1 = (-x - 5) / 10;
        return new Vector3Int(x1, y1, position.z);
    }
}
