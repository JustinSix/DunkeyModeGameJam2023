using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GGG : MonoBehaviour {
    private const string GAME_OBJECT_SLOT_NAME = "slot",
                            GAME_OBJECT_COLUMN = "col";

    private static Vector2 CENTER = new(0.5f, 0.5f);
    private const float YX_RATIO = 184f / 144f;

    private class Column {
        public enum Type {
            SPEEDING, RUN, SLOWING, WAIT
        }

        public GameObject col;
        public List<GameObject> box;
        public float speedingTime, runTime, slowingTime, a, timer, speed;
        public Type type;

        public Column(GameObject col, List<GameObject> box, float speedingTime, float runTime, float slowingTime, float a) {
            this.col = col;
            this.box = box;
            this.speedingTime = speedingTime;
            this.runTime = runTime;
            this.slowingTime = slowingTime;
            this.a = a;
            timer = speed = 0f;
            type = Type.WAIT;
        }

        public void Start() {
            type = Type.SPEEDING;
        }

        public bool IsRunning() {
            return type != Type.WAIT;
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
    public float minA = 1.78f, maxA = 6.78f;
    [Space]
    public float minSpeedingTime = 2.65f, maxSpeedingTime = 3.67f;
    public float minRunTime = 3.22f, maxRunTime = 5.67f;
    public float minSlowingTime = 1.86f, maxSlowingTime = 5.8f;
    [Space]
    public GameObject megaWin;

    public SpriteAtlas atlas;

    private int stackSize;
    private Vector2 size;
    private float startY;
    [HideInInspector] public int currentBet;

    public void Start() {
        box = GetComponent<RectTransform>();
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
            case State.START_ANIMATION: UpdateStartAnimation(delta); break;
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

        stackSize = (int)totalY * 2;
        if (stackSize < 6) stackSize = 6; //cap to min

        size = new Vector2(totalX, totalX * YX_RATIO);

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
                box.localPosition = new Vector2(size.x * pivot.x, j * (size.y / YX_RATIO) - size.y * pivot.y);
                box.pivot = pivot;

                EffScale effScale = block.AddComponent<EffScale>();
                effScale.ScaleUp();

                block.AddComponent<Image>();
                block.AddComponent<SpriteFromAtlas>().Set(atlas, IdToString(ids[i, j], 0));

                colGroup.Add(block);
            }

            Transform t = col.transform;
            t.parent = transform;
            t.localScale = Vector2.one;
            t.localPosition = new Vector2(i * size.x, 0);
            machine.Add(GenerateColumn(col, colGroup));
        }

        startY = -size.y * (1 - CENTER.y);
    }

    public void Spin() {
        if (IsRunning() || gold == 0) return;

        if (currentBet == 0) currentBet = 1; 
        foreach (Column col in machine) col.Start();
        state = State.GAME;
    }

    public void BetAll() {
        if (IsRunning()) return;

        currentBet = gold;
        foreach (Column col in machine) col.Start();
        state = State.GAME;
    }

    public void BetOne() {
        if (IsRunning() || gold == 0) return;

        currentBet = 1;
        foreach (Column col in machine) col.Start();
        state = State.GAME;
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
                    col.speed += col.a;

                    if ((y += col.speed * delta) >= startY)
                        y -= (int)(y / startY) * startY;

                    t.localPosition = new Vector2(xy.x, y);

                    if ((col.timer += delta) >= col.speedingTime) {
                        col.timer = 0;
                        col.type = Column.Type.RUN;
                    }
                } break;

                case Column.Type.RUN: {
                    col.speed += col.a;

                    if ((y += col.speed * delta) >= startY)
                        y -= (int)(y / startY) * startY;

                    t.localPosition = new Vector2(xy.x, y);

                    if ((col.timer += delta) >= col.runTime) {
                        col.timer = 0;
                        col.type = Column.Type.SLOWING;
                    }
                } break;

                case Column.Type.SLOWING: {
                    if (col.speed > 0)
                        if ((col.speed -= col.a) < 0) col.speed = 0;

                    if ((y += col.speed * delta) >= startY)
                        y -= (int)(y / startY) * startY;

                    t.localPosition = new Vector2(xy.x, y);

                    if ((col.timer += delta) >= col.speedingTime){
                        col.timer = 0;
                        col.type = Column.Type.WAIT;
                    }
                } break;
                case Column.Type.WAIT: waiting++; break;
            }
        }

        if (waiting == machine.Count) GameOver();
    }

    private void GameOver() {
        if (true) MegaWin();
    }

    private void MegaWin() {
        megaWin.SetActive(true);
    }

    private void UpdateStartAnimation(float delta) {
        int count = 0;

        foreach (Column col in machine) {
            count -= col.box.Count;
            foreach (GameObject elm in col.box)
                if (elm.GetComponent<EffScale>().IsFinished()) count++;
        }

        count -= machine.Count;

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
            Random.Range(minSpeedingTime, maxSpeedingTime),
            Random.Range(minRunTime, maxRunTime),
            Random.Range(minSlowingTime, maxSlowingTime),
            Random.Range(minA, maxA));
    }

    private bool IsRunning() {
        foreach (Column col in machine)
            if (col.IsRunning()) return true;
        return false;
    }


}
