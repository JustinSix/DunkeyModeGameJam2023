using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GGG : MonoBehaviour {
    private const string GAME_OBJECT_SLOT_NAME = "slot",
                            GAME_OBJECT_COLUMN = "col";

    private readonly int[] SPEED_LIMITS = new int[]{ 34, 62, 128, 234, 354 };

    private static Vector2 CENTER = new(0.5f, 0.5f);
    private const float YX_RATIO = 184f / 144f;

    private class Column {
        public enum Type {
            SPEEDING, RUN, SLOWING, WAIT
        }

        public GameObject col;
        public List<GameObject> box; //there is no order later
        public float speedingTime, runTime, a, timer, speed, height;
        public Type type;

        public int prevId; //for optimisation

        public Column(GameObject col, List<GameObject> box, float height, float speedingTime, float runTime, float a) {
            this.col = col;
            this.box = box;
            this.speedingTime = speedingTime;
            this.runTime = runTime;
            this.a = a;
            timer = speed = 0f;
            type = Type.WAIT;
            prevId = 0;
            this.height = height;
        }

        public void Start() {
            prevId = 0; //no speed
            type = Type.SPEEDING;
        }

        public bool IsRunning() {
            return type != Type.WAIT;
        }

        public void RotateAround(int rotations, float blockSize, bool cap) {
            RectTransform t;
            Vector2 xy;
            float y;

            for (int r = 0; r < rotations; r++) {
                GameObject last = box[^1];
                for (int i = box.Count - 1; i > 0; i--) {
                    box[i] = box[i - 1];
                    t = box[i].GetComponent<RectTransform>();
                    xy = t.localPosition;

                    y = cap ?
                        blockSize * (i - CENTER.y * YX_RATIO) : //last rotation is capped at start position
                        xy.y + blockSize; //normal move

                    t.localPosition = new Vector2(xy.x, y);
                }

                box[0] = last;
                t = box[0].GetComponent<RectTransform>();
                xy = t.localPosition;
                y = cap ?
                    -blockSize * YX_RATIO * CENTER.y : //last rotation is capped at start position
                    xy.y - blockSize * (box.Count - 1); //normal move

                t.localPosition = new Vector2(xy.x, y);
            }
        }
    }

    private enum State {
        START_ANIMATION, WAIT, GAME
    }

    private State state = State.START_ANIMATION;
    private RectTransform box;
    private readonly List<Column> machine = new();

    public int columns = 3, minColumns = 3, maxColumns = 20, gold = 400;
    [Space]
    public int differentIcons = 8;
    [Space]
    public float minA = 120.78f, maxA = 260.78f;
    [Space]
    public float minSpeedingTime = 2.65f, maxSpeedingTime = 3.67f;
    public float minRunTime = 3.22f, maxRunTime = 5.67f;
    [Space]
    public GameObject megaWin;
    public TextMeshProUGUI goldHolder;

    public SpriteAtlas atlas;

    private int stackSize;
    private float blockHeight;
    [HideInInspector] public int currentBet;

    public void Start() {
        box = GetComponent<RectTransform>();
        goldHolder.text = "" + gold;
        Generate();
    }

    public void Inc() {
        if (IsRunning() || columns == maxColumns) return;

        columns++;
        Rebuild();
    }

    public void Dec() {
        if (IsRunning() || columns == minColumns) return;

        columns--;
        Rebuild();
    }

    public void Update()  {
        float delta = Time.deltaTime;

        switch (state) {
            case State.START_ANIMATION: UpdateStartAnimation(); break;
            case State.WAIT: break;
            case State.GAME: UpdateRollAnimation(delta); break;
        }
    }

    private int[,] GenerateIds() {
        int[,] ids = new int[columns, stackSize];

        for (int i = 0; i < columns; i++)
            for (int j = 0; j < stackSize; j++)
                ids[i, j] = GenerateId();

        for (int i = 0; i < columns - 1; i++)
            for (int j = 0; j < stackSize; j++)
                while (ids[i, j] == ids[i + 1, j])
                    ids[i, j] = GenerateId();

        return ids;
    }

    public void Generate() {
        Vector2 holderSize = box.sizeDelta;
        float totalX = holderSize.x / columns,
              totalY = holderSize.y / totalX;

        stackSize = (int) totalY * 2;
        if (stackSize < 6) stackSize = 6; //cap to min

        blockHeight = totalX;
        Vector2 size = new(totalX, totalX * YX_RATIO);
        

        int[,] ids = GenerateIds();

        for(int i = 0; i < columns; i++) {
            GameObject col = new(GAME_OBJECT_COLUMN);
            List<GameObject> colGroup = new();

            for(int j = 0; j < stackSize; j++) {
                GameObject block = new(GAME_OBJECT_SLOT_NAME);
                RectTransform box = block.AddComponent<RectTransform>();
                box.parent = col.transform;
                box.localScale = Vector3.one;
                box.sizeDelta = size;

                Vector2 pivot = CENTER;
                box.localPosition = new Vector2(size.x * pivot.x, size.y * (j / YX_RATIO - pivot.y));
                box.pivot = pivot;

                EffScale effScale = block.AddComponent<EffScale>();
                effScale.Set(0f, 1f);
                effScale.ScaleUp();

                int id = ids[i, j];
                block.AddComponent<Image>();
                block.AddComponent<SpriteFromAtlas>().Set(atlas, IdToString(id, 0));
                block.AddComponent<Int>().value = id;

                colGroup.Add(block);
            }

            Transform t = col.transform;
            t.parent = transform;
            t.localScale = Vector2.one;
            t.localPosition = new Vector2(i * size.x, 0);
            machine.Add(GenerateColumn(col, colGroup));
        }
    }

    public void Spin() {
        if (IsRunning() || gold == 0) return;

        if (currentBet == 0) currentBet = 1;
        gold -= currentBet;

        foreach (Column col in machine) col.Start();
        state = State.GAME;
        goldHolder.text = "" + gold;
    }

    public void BetAll() {
        if (IsRunning()) return;

        currentBet = gold;
        gold = 0;

        foreach (Column col in machine) col.Start();
        state = State.GAME;
        goldHolder.text = "" + gold;
    }

    public void BetOne() {
        if (IsRunning() || gold == 0) return;

        currentBet = 1;
        --gold;
        foreach (Column col in machine) col.Start();
        state = State.GAME;
        goldHolder.text = "" + gold;
    }

    public void Rebuild() {
        Clear();
        Generate();
    }

    public void Clear() {
        foreach (Column col in machine) {
            Destroy(col.col);
            col.box.Clear();
        }

        machine.Clear();
    }

    private void UpdateRollAnimation(float delta) {
        int waiting = 0;

        foreach (Column col in machine) {
            GameObject gameObject = col.col;

            Transform t = gameObject.transform;
            Vector2 xy = t.localPosition;
            float y = xy.y;

            switch (col.type) {
                case Column.Type.SPEEDING: {
                    col.speed += col.a * delta;

                    if ((y += col.speed * delta) >= blockHeight) {
                        int rotations = (int)(y / blockHeight);
                        y %= blockHeight;
                        col.RotateAround(rotations, blockHeight, false);
                    }

                    t.localPosition = new Vector2(xy.x, y);

                    if ((col.timer += delta) >= col.speedingTime) {
                        col.timer = 0;
                        col.type = Column.Type.RUN;
                    }
                } break;

                case Column.Type.RUN: {
                    col.speed += col.a * delta;

                    if ((y += col.speed * delta) >= blockHeight) {
                        int rotations = (int)(y / blockHeight);
                        y %= blockHeight;
                        col.RotateAround(rotations, blockHeight, false);
                    }

                    t.localPosition = new Vector2(xy.x, y);

                    if ((col.timer += delta) >= col.runTime) {
                        col.timer = 0;
                        col.type = Column.Type.SLOWING;
                    }
                } break;

                case Column.Type.SLOWING: {
                    if (col.speed > SPEED_LIMITS[0])
                        if ((col.speed -= col.a * delta) < SPEED_LIMITS[0])
                            col.speed = SPEED_LIMITS[0]; //can't be 0 cause we need to go to target location

                        if ((y += col.speed * delta) >= blockHeight) {
                            if (col.speed == SPEED_LIMITS[0]) {
                                col.type = Column.Type.WAIT;
                                y = 0; //cap
                                col.RotateAround(1, blockHeight, true);
                            }
                            else if ((y += col.speed * delta) >= blockHeight) {
                                int rotations = (int)(y / blockHeight);
                                y %= blockHeight;
                                col.RotateAround(rotations, blockHeight, false);
                            }
                        }

                    t.localPosition = new Vector2(xy.x, y);
                } break;
                case Column.Type.WAIT: waiting++; break; //wait all other columns to stop
            }

            for (int i = SPEED_LIMITS.Length - 1; i >= 0; i--)
                if (col.speed > SPEED_LIMITS[i]) {
                    if (col.prevId == i) break;
                    else {
                        col.prevId = i;
                        foreach (GameObject elm in col.box)
                            elm.GetComponent<SpriteFromAtlas>().SetImage(IdToString(elm.GetComponent<Int>().value, i));

                        break;
                    }
                }
        }

        if (waiting == machine.Count) GameOver();
    }

    private void GameOver() {
        if (true) MegaWin();

        goldHolder.text = "" + gold;
    }

    private void MegaWin() {
        megaWin.SetActive(true);
    }

    private void UpdateStartAnimation() {
        int count = -machine.Count;

        foreach (Column col in machine) {
            count -= col.box.Count;
            foreach (GameObject elm in col.box)
                if (elm.GetComponent<EffScale>().IsFinished()) count++;
        }

        if (count == 0) state = State.WAIT;
    }

    private int GenerateId() {
        return Random.Range(0, differentIcons) + 1;
    }

    private string IdToString(int id, int animationState) {
        return "icon_" + id + "_0" + animationState;
    }

    private Column GenerateColumn(GameObject col, List<GameObject> box) {
        return new(col, box,
            box[0].GetComponent<RectTransform>().sizeDelta.y * box.Count,
            Random.Range(minSpeedingTime, maxSpeedingTime),
            Random.Range(minRunTime, maxRunTime),
            Random.Range(minA, maxA));
    }

    private bool IsRunning() {
        foreach (Column col in machine)
            if (col.IsRunning()) return true;
        return false;
    }
}
