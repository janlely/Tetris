using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameLogicScript : MonoBehaviour
{

    public GameObject gameOverUI;
    // public TileBase ghost;
    public Tilemap boardTilemap;
    public GameObject next1Area;
    private GameObject currentNext1Prefab;
    public GameObject next2Area;
    private GameObject currentNext2Prefab;
    public Vector3Int initPiecePosition;
    public float fallInterval;
    public float keyRepeatDelay;
    public float firstRepeatDelay;
    private float moveTimer;
    public float hitBottomInterval;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode downKey;
    public KeyCode dropKey;
    public KeyCode lRotKey;
    public KeyCode rRotKey;

    [SerializeField]
    public TetrominoEntry[] tiles;
    [SerializeField]
    public TetrominoPrefab[] prefabs;
    private Dictionary<TetrominoShape, int> prefabMap = new Dictionary<TetrominoShape, int>();
    private float stepTimer = 0f;
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
    private Vector3Int currentPiecePosition;
    private bool isAlive= true;
    private float hitBottomTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        for (int i = 0; i < prefabs.Length; i++) {
            prefabMap[prefabs[i].shape] = i;
        }
        Debug.Log("start game logic");
        spawnNewPiece();
        drawPiece();
        stepTimer = Time.time + fallInterval;
    }


    void Update() {
        if (!isAlive) {
            return;
        }

        clearPiece();

        //自动下落
        if (Time.time > stepTimer) {
            fall();
        }

        handlerMoveByKeDown();

        //处理左右移动和下降
        if (Time.time > moveTimer) {
            handlerMove();
        }

        //直接丢下
        if (Input.GetKeyDown(dropKey)) {
            dropPiece();
        }

        //变换形
        if (Input.GetKeyDown(lRotKey)) {
            rotatePiece(Vector3Int.left);
        }
        if (Input.GetKeyDown(rRotKey)) {
            rotatePiece(Vector3Int.right);
        }

        //判断是否到达底部
        if (hitBottomHit()) {
            drawPiece();
            clearLines();
            Debug.Log("spawnNewPiece");
            spawnNewPiece();
        }

        drawPiece();

    }

    private void handlerMoveByKeDown() {
        if (Input.GetKeyDown(leftKey)) {
            movePiece(Vector3Int.left);
            moveTimer = Time.time + firstRepeatDelay;
        }
        if (Input.GetKeyDown(rightKey)) {
            movePiece(Vector3Int.right);
            moveTimer = Time.time + firstRepeatDelay;
        }
        if (Input.GetKeyDown(downKey)) {
            fall();
            moveTimer = Time.time + firstRepeatDelay;
        }
    }

    private void handlerMove() {
        if (Input.GetKey(leftKey)) {
            movePiece(Vector3Int.left);
        }
        if (Input.GetKey(rightKey)) {
            movePiece(Vector3Int.right);
        }
        if (Input.GetKey(downKey)) {
            fall();
        }
        moveTimer = Time.time + keyRepeatDelay;
    }

    private void fall() {
        movePiece(Vector3Int.down);
        stepTimer = Time.time + fallInterval;
    }


    private void clearLines() {
        int lowestPosition = currentPiece.downMostPosition().Select(p => p + currentPiecePosition).Min(p => p.y);
        //一次最多消除4行
        int startLine = lowestPosition;
        int[] linesToClear = new int[4];
        int count =0;
        for (int i = 0; i < 4; i++) {
            if (isFullLine(startLine)) {
                linesToClear[count++] = startLine;
                clearLine(startLine);
            }
            startLine++;
        }
        //没有行需要消除
        if (count == 0) {
            return;
        }
        //消除被标记的行,并把其他行下移,采用双指针循环方法
        //找到第一个被标记的行
        int p1 = linesToClear[0];
        int p2 = p1 + 1;
        while(p1 < p2 && p2 < 10) {
            if (!isLineEmpty(p2)) {
                moveLineDown(p2, p1);
                p1++;
            }
            p2++;
        }
    }

    private void moveLineDown(int from, int to) {
        for (int x = -5; x < 5; x++) {
            boardTilemap.SetTile(new Vector3Int(x, to, 0), boardTilemap.GetTile(new Vector3Int(x, from, 0)));
        }
        //删除from
        for (int x = -5; x < 5; x++) {
            boardTilemap.SetTile(new Vector3Int(x, from, 0), null);
        }
    }

    private bool isLineEmpty(int y) {
        for (int x = -5; x < 5; x++) {
            if (boardTilemap.HasTile(new Vector3Int(x, y, 0))) {
                return false;
            }
        }
        return true;
    }

    private void clearLine(int y) {
        for (int x = -5; x < 5; x++) {
            boardTilemap.SetTile(new Vector3Int(x, y, 0), null);
        }
    }

    private bool isFullLine(int y) {
        for (int x = -5; x < 5; x++) {
            if (!boardTilemap.HasTile(new Vector3Int(x, y, 0))) {
                return false;
            }
        }
        return true;
    }

    private void clearPiece() {
        Vector3Int[] position = currentPiece.getPosition();
        for (int i = 0; i < position.Length; i++) {
            boardTilemap.SetTile(position[i] + currentPiecePosition, null);
        }
    }

    private void rotatePiece(Vector3Int direction) {
        // Vector3Int[] orgPosition = currentPiece.getPosition().Select(p => p + currentPiecePosition).ToArray();
        // for (int i = 0; i < orgPosition.Length; i++) {
        //     boardTilemap.SetTile(orgPosition[i], null);
        // }
        currentPiece.rotate(direction);
        // updateTilemap(boardTilemap, currentPiecePosition, currentPiece);
    }

    private void dropPiece() {
        while (movePiece(Vector3Int.down)) {
            continue;
        }

        hitBottomTimer += hitBottomInterval;
        
    }

    
    private bool movePiece(Vector3Int direction) {
        Vector3Int[] position = currentPiece.getPosition().Select(p => p + currentPiecePosition).ToArray();
        //左移，不能碰壁，不能碰到其他方块
        if (direction == Vector3Int.left && position.All(p => p.x > -5)
            && currentPiece.leftMostPosition().Select(p => p + currentPiecePosition + direction).All(p => !boardTilemap.HasTile(p))) {

            currentPiecePosition += direction;
            return true;
        }
        //右移，不能碰壁，不能碰到其他方块
        if (direction == Vector3Int.right && position.All(p => p.x < 4)
            && currentPiece.rightMostPosition().Select(p => p + currentPiecePosition + direction).All(p => !boardTilemap.HasTile(p))) {

            currentPiecePosition += direction;
            return true;
        }

        //下移，不能碰壁，不能碰到其他方块
        if (direction == Vector3Int.down && position.All(p => p.y > -10)
            && currentPiece.downMostPosition().Select(p => p + currentPiecePosition + direction).All(p => !boardTilemap.HasTile(p))) {

            currentPiecePosition += direction;
            hitBottomTimer = 0;
            return true;
        }

        return false;
    }
    
    private void gameOver() {
        isAlive = false;
        gameOverUI.SetActive(true);
    }

    private void spawnNewPiece() {
        currentPiecePosition = initPiecePosition;
        next2 ??= new(tiles);
        next1 ??= new(tiles);

        Piece newPiece = new(tiles);
        currentPiece = next1;

        next1 = next2;
        next2 = newPiece;

        if (currentNext1Prefab != null) {
            Destroy(currentNext1Prefab);
        }
        if (currentNext2Prefab != null) {
            Destroy(currentNext2Prefab);
        }

        // updateTilemap(boardTilemap, initPiecePosition, currentPiece, true);
        //放置预览
        currentNext1Prefab = Instantiate(prefabs[prefabMap[next1.shape]].prefab);
        currentNext2Prefab = Instantiate(prefabs[prefabMap[next2.shape]].prefab);
        currentNext1Prefab.transform.SetParent(next1Area.transform);
        currentNext1Prefab.transform.position = next1Area.transform.position;
        currentNext1Prefab.transform.localScale = new Vector3(0.8f, 0.8f, 0);
        currentNext2Prefab.transform.SetParent(next2Area.transform);
        currentNext2Prefab.transform.position = next2Area.transform.position;
        currentNext2Prefab.transform.localScale = new Vector3(0.8f, 0.8f, 0);

        //游戏结束判定
        if (currentPiece.getPosition().Select(p => p + currentPiecePosition).Any(p => boardTilemap.HasTile(p))) {
            gameOver();
        }
        moveTimer = Time.time + firstRepeatDelay;
    }

    private bool hitBottomHit() {
        //延迟触底判断
        if (hitBottomTimer < hitBottomInterval) {
            hitBottomTimer += Time.deltaTime;
            return false;
        }
        hitBottomTimer = 0f;
        //判断是否到达底部
        Vector3Int[] position = currentPiece.getPosition().Select(p => p + currentPiecePosition + Vector3Int.down).ToArray();
        return position.Any(p => p.y < -10)
            || currentPiece.downMostPosition().Select(p => p + currentPiecePosition + Vector3Int.down).Any(p => boardTilemap.HasTile(p));
    }


    private void drawPiece() {
        Vector3Int[] position = currentPiece.getPosition().Select(p => p + currentPiecePosition).ToArray();
        for(int i = 0; i < position.Length; i++) {
            boardTilemap.SetTile(position[i], currentPiece.tile);
        }
    }

    public void restart() {
        isAlive = true;
        hitBottomTimer = 0f;
        gameOverUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        spawnNewPiece();
    }

}


[System.Serializable]
public struct TetrominoEntry
{
    public TetrominoShape shape;
    public TileBase tile;
}

[System.Serializable]
public struct TetrominoPrefab
{
    public TetrominoShape shape;
    public GameObject prefab;
}