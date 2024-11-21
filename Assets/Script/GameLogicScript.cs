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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    void generateNewPiece() {

    }
}
