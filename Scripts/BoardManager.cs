using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,] AllowedMoves { set; get; }

    public Chessman[,] Chessmen { set; get; }
    private Chessman _selectedChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> _activeChessman;

    private Material _previousMat;
    public Material selectedMat;

    [FormerlySerializedAs("enPassantMove")]
    public int[] EnPassantMove;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public bool isWhiteTurn = true;

    private void Start()
    {
        Instance = this;
        SpawnAllChessmen();
    }

    private void SpawnAllChessmen()
    {
        _activeChessman = new List<GameObject>();
        Chessmen = new Chessman[8, 8];
        EnPassantMove = new int[2] {-1, -1};

        // Spawn the white team!
        // King
        SpawnChessman(0, 3, 0, false);

        // Queen
        SpawnChessman(1, 4, 0, false);

        // Rooks
        SpawnChessman(2, 0, 0, false);
        SpawnChessman(2, 7, 0, false);

        // Bishops
        SpawnChessman(3, 2, 0, false);
        SpawnChessman(3, 5, 0, false);

        // Knights
        SpawnChessman(4, 1, 0, false);
        SpawnChessman(4, 6, 0, false);

        // Pawns
        for (var i = 0; i < 8; i++)
            SpawnChessman(5, i, 1, false);

        // Spawn the black team!
        // King
        SpawnChessman(6, 4, 7, true);

        // Queen
        SpawnChessman(7, 3, 7, true);

        // Rooks
        SpawnChessman(8, 0, 7, true);
        SpawnChessman(8, 7, 7, true);

        // Bishops
        SpawnChessman(9, 2, 7, true);
        SpawnChessman(9, 5, 7, true);

        // Knights
        SpawnChessman(10, 1, 7, true);
        SpawnChessman(10, 6, 7, true);

        // Pawns
        for (var i = 0; i < 8; i++)
            SpawnChessman(11, i, 6, true);
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (_selectedChessman == null)
                {
                    // Select a chessman
                    SelectChessman(selectionX, selectionY);
                }

                else
                {
                    // Move a chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        if (Chessmen[x, y] == null)
            return;

        if (Chessmen[x, y].isWhite != isWhiteTurn)
            return;


        var hasAtLeastOneMove = false;
        AllowedMoves = Chessmen[x, y].PossibleMove();
        for (int i = 0; i < 8; i++)
        for (int j = 0; j < 8; j++)
            if (AllowedMoves[i, j])
                hasAtLeastOneMove = true;

        if (!hasAtLeastOneMove)
            return;

        _selectedChessman = Chessmen[x, y];
        _previousMat = _selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = _previousMat.mainTexture;
        _selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        BoardHighlights.Instance.HighlightAllowedMoves(AllowedMoves);
    }

    private void MoveChessman(int x, int y)
    {
        if (AllowedMoves[x, y])
        {
            Chessman c = Chessmen[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                // Capture a piece

                // If it is a king
                if (c.GetType() == typeof(King))
                {
                    // End the game
                    EndGame();
                    return;
                }

                _activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                c = isWhiteTurn ? Chessmen[x, y - 1] : Chessmen[x, y + 1];
                _activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (_selectedChessman.GetType() == typeof(Pawn))
            {
                if (y == 7)
                {
                    _activeChessman.Remove(_selectedChessman.gameObject);
                    Destroy(_selectedChessman.gameObject);
                    SpawnChessman(1, x, y, false);
                    _selectedChessman = Chessmen[x, y];
                }
                
                else if (y == 0)
                {
                    _activeChessman.Remove(_selectedChessman.gameObject);
                    Destroy(_selectedChessman.gameObject);
                    SpawnChessman(7, x, y, true);
                }
                
                switch (_selectedChessman.CurrentY)
                {
                    case 1 when y == 3:
                        EnPassantMove[0] = x;
                        EnPassantMove[1] = y - 1;
                        break;
                    case 6 when y == 4:
                        EnPassantMove[0] = x;
                        EnPassantMove[1] = y + 1;
                        break;
                }
            }

            Chessmen[_selectedChessman.CurrentX, _selectedChessman.CurrentY] = null;
            _selectedChessman.transform.position = GetTileCenter(x, y);
            _selectedChessman.SetPosition(x, y);
            Chessmen[x, y] = _selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }

        _selectedChessman.GetComponent<MeshRenderer>().material = _previousMat;
        BoardHighlights.Instance.HideHightlights();
        _selectedChessman = null;
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White team wins");

        else
            Debug.Log("Black team wins");

        foreach (var go in _activeChessman)
            Destroy(go);

        isWhiteTurn = true;
        BoardHighlights.Instance.HideHightlights();
        SpawnAllChessmen();
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f,
            LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int) hit.point.x;
            selectionY = (int) hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index, int x, int y, bool isBlack)
    {
        GameObject go;
        if (isBlack)
            go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), orientation) as GameObject;
        else
            go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;

        go.transform.SetParent(transform);
        Chessmen[x, y] = go.GetComponent<Chessman>();
        Chessmen[x, y].SetPosition(x, y);
        _activeChessman.Add(go);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        var origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);

            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
            );

            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1)
            );
        }
    }
}