using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsVisual : MonoBehaviour
{
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private List<HeartImage> heartImageList;
    public Health healthSystem;

    public class HeartImage
    {
        // represents a single heart

        private int state;
        private Image heartImage;
        private HeartsVisual heartsVisual;

        public HeartImage(HeartsVisual heartsVisual, Image heartImage)
        {
            this.heartsVisual = heartsVisual;
            this.heartImage = heartImage;
        }

        public void SetHeartStates(int state)
        {
            this.state = state;
            switch (state)
            {
                case 0: heartImage.sprite = heartsVisual.emptyHeartSprite; break;
                case 1: heartImage.sprite = heartsVisual.halfHeartSprite; break;
                case 2: heartImage.sprite = heartsVisual.fullHeartSprite; break;
            }
        }

        public int GetState()
        {
            return state;
        }

        public void AddHeartVisual()
        {
            SetHeartStates(state + 1);
        }
    }

    private void Awake()
    {
        heartImageList = new List<HeartImage>();
    }

    private void Start()
    {
        // start with 1 heart
        Health healthSystem = new Health(1);
        SetHealthSystem(healthSystem);
    }

    public void SetHealthSystem(Health healthSystem)
    {
        this.healthSystem = healthSystem;

        List<Health.Heart> heartList = healthSystem.GetHeartList();
        Vector2 heartAnchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < heartList.Count; i++)
        {
            Health.Heart heart = heartList[i];
            CreateHeartImage(heartAnchoredPosition).SetHeartStates(heart.GetStates());
            heartAnchoredPosition += new Vector2(30, 0);
        }

        healthSystem.OnDamaged += Health_OnDamaged;
        healthSystem.OnHealed += Health_OnHealed;
        healthSystem.OnDead += Health_OnDead;
    }

    
    private void Health_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateHeartVisuals();
    }    

    private void Health_OnHealed(object sender, System.EventArgs e)
    {
        UpdateHeartVisuals();
    }

    private void Health_OnDead(object sender, System.EventArgs e)
    {
        Debug.Log("dead");
    }

    private void UpdateHeartVisuals()
    {
        List<Health.Heart> heartList = healthSystem.GetHeartList();
        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            Health.Heart heart = heartList[i];
            heartImage.SetHeartStates(heart.GetStates());
        }
    }

    private HeartImage CreateHeartImage(Vector2 anchoredPosition)
    {
        GameObject heartGameObject = new GameObject("Heart", typeof(Image));

        // set up transform
        heartGameObject.transform.SetParent(transform);
        heartGameObject.transform.localPosition = Vector3.zero;
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);

        // set heart sprite
        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = fullHeartSprite;

        HeartImage heartImage = new HeartImage(this, heartImageUI);
        heartImageList.Add(heartImage);

        return heartImage;

    }
}
