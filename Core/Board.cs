using System.Collections.Generic;
using UnityEngine;

public class Board : GraphicalObject {

    // Singleton
    public static Board MAIN; // The visible/active board of the game

    // Fields
    public int width;
    public int height;
    public List<Field> fields = new List<Field>();

    // References
    public Transform fieldContainer;
    public Transform unitContaioner;
    public GameObject fieldPrefab;
    public List<GameObject> unitPrefabs = new List<GameObject>();
    public Game game;
    public Color fieldColor;

    public bool isDummy;
    public List<Unit> graveyard = new List<Unit>();

    #region Initialization
    public void Initialize() {
        MAIN = this;
        name = "MainBoard";
        isDummy = false;
        CreateFields();
    }
    
    public void CreateFields() {
        float fieldWidth = 8f / width;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Field transforming
                Vector3 offset = -Vector3.right * 8 / 2 - Vector3.forward * 8 / 2 + new Vector3(fieldWidth/2, 0f, fieldWidth/2);
                Vector3 position = transform.position + Vector3.right * x * fieldWidth + Vector3.forward * y * fieldWidth + offset;
                GameObject fieldGO = Instantiate(fieldPrefab, position, Quaternion.identity);
                fieldGO.transform.parent = fieldContainer;

                fieldGO.GetComponentInChildren<Renderer>().material.SetColor("_Color", fieldColor);
                Vector2 randomOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
                fieldGO.GetComponentInChildren<Renderer>().material.SetTextureOffset("_MainTex", randomOffset);

                // Field initialization
                Field field = fieldGO.GetComponent<Field>();
                fields.Add(field);
                field.Initialize(this, fields.Count - 1);
            }
        }
    }
    
    public void PopulateBoard(Player regularPlayer, Player oppositePlayer, bool random) {
        if(random) {
            CacheUnits(excludeClassic: true);
            Random.InitState(System.DateTime.Now.Millisecond);
            SetupUnitsRandomly();
        } else {
            CacheUnits(excludeClassic: false);
            LoadPlayerLoadout(regularPlayer, invert: false);
            LoadPlayerLoadout(oppositePlayer, invert: true);
        }
    }    

    public void CacheUnits(bool excludeClassic) {
        unitPrefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Units"));
        if (excludeClassic)
            unitPrefabs = unitPrefabs.FindAll(prefab =>
                !prefab.GetComponent<Unit>().classic
            );
    }

    public void SetupUnitsRandomly() {
        for (int i = 0; i < 20; i++) {
            int index = Random.Range(0, width * height);
            Field field = fields[index];
            int randomUnitIndex = Random.Range(0, unitPrefabs.Count);
            if (unitPrefabs[randomUnitIndex].GetComponent<Unit>().ID == 6) continue;
            GameObject randomPrefab = unitPrefabs[randomUnitIndex];
            Player randomPlayer = game.GetRandomPlayer();
            InstantiateUnit(randomPrefab, field, randomPlayer);
        }
    }

    public void LoadPlayerLoadout(Player player, bool invert) {
        Loadout loadout = player.loadout;
        for (int i = 0; i < loadout.unitIDs.Count; i++) {
            int fieldID = i;
            if (invert) fieldID = (width*height-1) - i;
            Field field = fields[fieldID];
            GameObject unitPrefab = GetUnitPrefabById(loadout.unitIDs[i]);
            InstantiateUnit(unitPrefab, field, player);
        }
    }

    public void InstantiateUnit(GameObject unitPrefab, Field field, Player player) {
        if (field.IsEmpty() && unitPrefab != null) {
            GameObject unitGO = Instantiate(unitPrefab);
            Unit unit = unitGO.GetComponent<Unit>();
            unit.transform.parent = unitContaioner;
            unit.Initialize(player, this, unitPrefab);
            unit.Summon(field);
        }
    }

    public GameObject GetUnitPrefabById(int id) {
        return unitPrefabs.Find(unitPrefab => unitPrefab.GetComponent<Unit>().ID == id);
    }

    public override bool IsCopied() {
        return isDummy;
    }
    #endregion

    #region Utils
    public Field GetFieldWithOffset(Field field, Player playerPOV, Vec2 offsetPOV) {
        Vec2 startPosition = field.ToVec2();
        Vec2 offset = playerPOV.regularFacing ? offsetPOV : offsetPOV.Invert();
        Vec2 resultPosition = startPosition.Plus(offset);
        return GetField(resultPosition);
    }

    public Field GetField(int x, int y) {
        if (x < 0 || x >= width) return null;
        if (y < 0 || y >= height) return null;
        int returnIndex = x + y * width;
        if (0 <= returnIndex && returnIndex < width * height) return fields[returnIndex];
        return null;
    }

    public Field GetField(Vec2 vec) {
        return GetField(vec.x, vec.y);
    }

    public void RelocateFieldsAndUnits(Vector3 destination) {
        unitContaioner.position = destination;
        fieldContainer.position = destination;
        transform.Find("Background").transform.position = destination;
    }

    public void SwapUnits(Unit a, Unit b) {
        Field tmpField = a.field;
        Unit tmpUnit = a;

        a.field = b.field;
        b.field = tmpField;

        b.field.unit = b;
        a.field.unit = a;

        if (!IsCopied()) {
            SwapAnimation swapAnimation = GetAnimation<SwapAnimation>();
            swapAnimation.Init(a, b);
        }
    }
    #endregion

    #region Board interactions
    public void ExecuteAction(Action action) {
        if (action.IsPass()) Pass();
        else ExecuteMove(action.Move);
    }

    public void ExecuteMove(Move move) {
        Field startField = move.GetStart(this);
        Unit unit = startField.unit;
        unit.Move(move);

        // Advance turn
        game.EndTurn();
        game.consecutivePasses = 0;
    }

    public void Pass() {
        game.consecutivePasses++;
        if (game.consecutivePasses >= 2) {
            game.EndRound();
        } else {
            game.EndTurn();
        }
    }
    #endregion

    #region AI helper
    public List<Action> GetAllLegalActions() {
        List<Unit> units = GetAlliesOf(game.currentPlayer);
        
        // Collecting Units' moves
        List<Move> moves = new List<Move>();
        units.ForEach(unit => moves.AddRange(unit.GetMoves()));

        // Move -> Action
        List<Action> actions = moves.ConvertAll(move => new Action(move));
        return actions;
    }

    public int GetValueAdvantage() {
        return GetBoardValue(game.regularPlayer) - GetBoardValue(game.oppositePlayer);
    }

    public int GetBoardValue(Player player) {
        List<Unit> units = GetAlliesOf(player);
        int value = 0;
        units.ForEach(unit => value += unit.GetValue());
        return value;
    }

    public void RemoveGraphics() {
        name = "Board:1";
        transform.position += Vector3.right * 20f;
        isDummy = true;

        RemoveAnimations(); // Own animations
        game.RemoveGraphics();

        // Remove Unit graphics
        List<Unit> units = GetUnits();
        units.ForEach(unit => unit.RemoveGraphics());
        units.ForEach(unit => unit.RemoveAnimations());
        units.ForEach(unit => unit.MuteAnimations());

        // Remove Field graphics
        fields.ForEach(field => field.RemoveGraphics());
    }
    #endregion

    #region Units
    public List<Unit> GetUnits() {
        List<Unit> units = fields.ConvertAll(field => field.unit);
        return units.FindAll(unit => unit != null);
    }

    public List<Unit> GetAlliesOf(Player player) {
        return GetUnits().FindAll(unit => unit.owner == player);
    }

    public List<Unit> GetLeadersOf(Player player) {
        List<Unit> allies = GetAlliesOf(player);
        return allies.FindAll(ally => ally.HasKeyword<Leader>());
    }

    public void AddToGraveyard(Unit unit) {
        if (isDummy) {
            // If it's a real unit it might still play sound at DeathAnimation
            unit.gameObject.SetActive(false);
        }
        graveyard.Add(unit);
    }
    #endregion

}