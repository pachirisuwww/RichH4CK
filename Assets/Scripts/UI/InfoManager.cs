using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoManager : MonoBehaviour
{
    public RectTransform rectTrans;

    public RectTransform Sample;

    [System.Serializable]
    internal struct Info
    {
        public RectTransform Rect;
        public Animation Anim;
        public CanvasGroup CanvasGroup;

        public Image Icon;
        public InputField ChaName;
        public InputField PlayerName;

        public TextAnim Cash;
        public TextAnim Bank;
    }
    internal Info[] Infos = new Info[4];

    struct InfoTarget
    {
        public Vector2 pos;
        public Vector2 velocity;
    }
    InfoTarget[] infoTargets = new InfoTarget[4];

    void Awake()
    {
        Spawn();
    }

    void Spawn()
    {
        //生成物件 改名
        for (int i = 2; i <= 4; i++)
        {
            GameObject go = Instantiate(Sample.gameObject, transform, false);
            go.name = string.Format("P{0}", i);
        }

        for (int i = 0; i < 4; i++)
        {
            string name = string.Format("P{0}", i + 1);
            Transform child = transform.Find(name);

            System.Action<Transform> action = (t) =>
            {
                if (t.name == "ChaIcon")
                {
                    Infos[i].Icon = t.GetComponent<Image>();
                    Infos[i].Icon.material = new Material(Infos[i].Icon.material);
                }
                if (t.name == "ChaName") Infos[i].ChaName = t.GetComponent<InputField>();
                if (t.name == "PlayerName") Infos[i].PlayerName = t.GetComponent<InputField>();
                if (t.name == "Cash") Infos[i].Cash = t.GetComponent<TextAnim>();
                if (t.name == "Bank") Infos[i].Bank = t.GetComponent<TextAnim>();
            };
            NestedUtility.TransformTreeAction(child, action);

            Infos[i].Rect = child.GetComponent<RectTransform>();
            Infos[i].CanvasGroup = child.GetComponent<CanvasGroup>();
            Infos[i].CanvasGroup.alpha = 0;
            Infos[i].Anim = child.GetComponent<Animation>();
        }
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 target = infoTargets[i].pos;
            Infos[i].Rect.anchoredPosition = Vector2.SmoothDamp(Infos[i].Rect.anchoredPosition, target, ref infoTargets[i].velocity, 0.1f);
        }
    }

    internal void InfoTrans(int i, bool isIn = true)
    {
        Infos[i].Anim.CrossFade(isIn ? "In" : "Out");
    }

    internal void SetDisplayNum(int num)
    {
        float sampleHeight = Sample.sizeDelta.y;

        float fullHeight = sampleHeight * 4;
        for (int i = 0; i < infoTargets.Length; i++)
        {
            int tmp = num;
            if (i >= num)
                tmp = i + 1;

            float seg = fullHeight / tmp;
            float start = seg * (tmp - 1) * 0.5f;
            infoTargets[i].pos = Vector2.up * (start - seg * i);
        }
    }
}
