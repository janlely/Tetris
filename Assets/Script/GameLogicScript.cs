using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameLogicScript : MonoBehaviour
{

    public Tilemap boardTilemap;
    public Tilemap next1Tilemap;
    public Tilemap next2Tilemap;
    public float fallInterval;
    public float hitBottomInterval;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode downKey;
    public KeyCode dropKey;
    public KeyCode lRotKey;
    public KeyCode rRotKey;

    [SerializeField]
    public TetrominoEntry[] tiles;
    private float autoFallTimer = 0f;
    private Piece currentPiece;
    private Piece next1;
    private Piece next2;
    private Dictionary<TetrominoShape, Vector3Int> positions = new Dictionary<TetrominoShape, Vector3Int>(){
        {TetrominoShape.I, new Vector3Int(0, 8, 0)},
        {TetrominoShape.O, new Vector3Int(0, 8, 0)},
        {TetrominoShape.J, new Vector3Int(0, 8, 0)},
        {TetrominoShape.L, new Vector3Int(0, 8, 0)},
        {TetrominoShape.S, new Vector3Int(0, 8, 0)},
        {TetrominoShape.T, new Vector3Int(0, 9, 0)},
        {TetrominoShape.Z, new Vector3Int(0, 9, 0)}
    };
    private Vector3Int currentPiecePosition = new Vector3Int(0, 0, 0);
    private bool isAlive= true;
    private float hitBottomTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        spawnNewPiece();
    }

    // Update is called once per frame
    void Update() {
        if (autoFallTimer >= fallInterval) {
            movePiece(Vector3Int.down);
            autoFallTimer = 0f;
        } else {
            autoFallTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(leftKey)) {
            movePiece(Vector3Int.left);
        }
        if (Input.GetKeyDown(rightKey)) {
            movePiece(Vector3Int.right);
        }
        if (Input.GetKeyDown(downKey)) {
            movePiece(Vector3Int.down);
            autoFallTimer = 0f;
        }
        if (Input.GetKeyDown(dropKey)) {
            dropPiece();
        }
        if (Input.GetKeyDown(lRotKey)) {
            rotatePiece(Vector3Int.left);
        }
        if (Input.GetKeyDown(rRotKey)) {
            rotatePiece(Vector3Int.right);
        }
    }


    private void rotatePiece(Vector3Int direction) {
        currentPiece.rotate(direction);
    }

    private void dropPiece() {
        Vector3Int[] downMostPosition = currentPiece.downMostPosition().Select(p => p + currentPiecePosition).ToArray();
        Vector3Int[] position = currentPiece.position.Select(p => p + currentPiecePosition).ToArray();
        while (true) {
            if (downMostPosition.All(p => !boardTilemap.HasTile(p))) {
                currentPiecePosition += Vector3Int.down;
            }else {
                break;
            }
            downMostPosition = downMostPosition.Select(p => p + Vector3Int.down).ToArray();
        }
        //消除原来的tile
        for (int i = 0; i < position.Length; i++) {
            boardTilemap.SetTile(position[i], null);
        }
        //设置新的tile
        updateTilemap(boardTilemap, currentPiecePosition, currentPiece);
        spawnNewPiece();
    }

    
    private void movePiece(Vector3Int direction) {
        Vector3Int[] position = currentPiece.position.Select(p => p + currentPiecePosition).ToArray();
        //左移，不能碰壁，不能碰到其他方块
        if (direction == Vector3Int.left && position.All(p => p.x > -5)
            && currentPiece.leftMostPosition().Select(p => p + currentPiecePosition).All(p => !boardTilemap.HasTile(p))) {

            doMovePiece(direction, position);
            return;
        }
        //右移，不能碰壁，不能碰到其他方块
        if (direction == Vector3Int.right && position.All(p => p.x > 4)
            && currentPiece.rightMostPosition().Select(p => p + currentPiecePosition).All(p => !boardTilemap.HasTile(p))) {

            doMovePiece(direction, position);
            return;
        }
        //下移，不能碰壁，不能碰到其他方块
        if (direction == Vector3Int.down && position.All(p => p.x > 4)
            && currentPiece.downMostPosition().Select(p => p + currentPiecePosition).All(p => !boardTilemap.HasTile(p))) {

            doMovePiece(direction, position);
            //下移需要刷新hitBottomTimer
            hitBottomTimer = 0;
            return;
        }else {
            spawnNewPiece();
        }

    }
    
    private void gameOver() {

    }

    private void spawnNewPiece() {

    }

    private bool hitBottomHit() {
        //判断是否到达底部
        Vector3Int[] position = currentPiece.position.Select(p => p + currentPiecePosition + Vector3Int.down).ToArray();
        if (position.Any(p => p.x < -5)
            || currentPiece.downMostPosition().Select(p => p + currentPiecePosition + Vector3Int.down).Any(p => boardTilemap.HasTile(p))) {
            
            return true;
        }
        return false;
    }

    private void doMovePiece(Vector3Int direction, Vector3Int[] position) {
        //清空原来的tile
        for (int i = 0; i < position.Length; i++) {
            boardTilemap.SetTile(position[i], null);
        }
        //更新位置并绘制新tile
        currentPiecePosition += direction;
        updateTilemap(boardTilemap, currentPiecePosition, currentPiece);
    }

    private void updateTilemap(Tilemap tilemap, Vector3Int basePosition, Piece piece, bool checkConfilict = false) {
        Vector3Int[] position = piece.position.Select(p => p + basePosition).ToArray();
        if (checkConfilict) {
            for (int i = 0; i < position.Length; i++) {
                if (tilemap.HasTile(position[i])) {
                    gameOver();
                    return;
                }
            }
        }
        for(int i = 0; i < position.Length; i++) {
            tilemap.SetTile(position[i], piece.tile);
        }
    }

}


[System.Serializable]
public struct TetrominoEntry
{
    public TetrominoShape shape;
    public TileBase tile;
}