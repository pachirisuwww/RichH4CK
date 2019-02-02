using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoManager : MonoBehaviour
{
    internal static InfoManager Instance;

    public GameObject Smaple;

    [System.Serializable]
    internal struct Info
    {
        public Animation Anim;
        public CanvasGroup CanvasGroup;

        public Image Icon;
        public InputField ChaName;
        public InputField PlayerName;

        public Text Cash;
        public Text Bank;
    }
    internal Info[] Infos = new Info[4];

    void Awake()
    {
        Instance = this;

        Spawn();
    }

    void Spawn()
    {
        //生成物件 改名
        for (int i = 2; i <= 4; i++)
        {
            GameObject go = Instantiate(Smaple, transform, false);
            go.name = string.Format("P{0}", i);
        }

        for (int i = 0; i < 4; i++)
        {
            string name = string.Format("P{0}", i + 1);
            Transform child = transform.Find(name);

            Infos[i].CanvasGroup = child.GetComponent<CanvasGroup>();
            Infos[i].CanvasGroup.alpha = 0;
            Infos[i].Anim = child.GetComponent<Animation>();

            System.Action<Transform> action = (t) =>
            {
                if (t.name == "ChaIcon") Infos[i].Icon = t.GetComponent<Image>();
                if (t.name == "ChaName") Infos[i].ChaName = t.GetComponent<InputField>();
                if (t.name == "PlayerName") Infos[i].PlayerName = t.GetComponent<InputField>();
                if (t.name == "Cash") Infos[i].Cash = t.GetComponentInChildren<Text>();
                if (t.name == "Bank") Infos[i].Bank = t.GetComponentInChildren<Text>();
            };
            NestedUtility.TransformTreeAction(child, action);
        }
    }

    internal void InfoTrans(int i, float speed = 1)
    {
        Infos[i].Anim.Stop();
        Infos[i].Anim["Trans"].speed = speed;
        Infos[i].Anim.Play();
        Infos[i].Anim.wrapMode = WrapMode.Once;
    }
}
