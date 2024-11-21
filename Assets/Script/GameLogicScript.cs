using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameLogicScript : MonoBehaviour
{

    public Tilemap boardTilemap;
    public Tilemap next1Tilemap;
    public Tilemap next2Tilemap;
    public float fallInterval;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode downKey;
    public KeyCode dropKey;
    private float autoFallTimer = 0f;
    private Piece currentPiece;
    private Piece next1;
    private Piece next2;
    private Vector3Int spawnPosition = new Vector3Int(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        currentPiece = generateNewPiece();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private Piece generateNewPiece() {
        //TODO: generate new piece
        return null;
    }
    
    private void movePiece(Vector3Int direction) {
        //TODO: move piece
    }

}
