using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform up;
    [SerializeField] private Transform down;

    [SerializeField] private PlayerSkill skill;
    private Image image;

    public PlayerSkill Skill => skill;

    private void Start()
    {
        image = GetComponentsInChildren<Image>()[1];
        image.sprite = skill.Img;

        if (!skill.IsUnlock)
        {
            image.color = LockColor();
        }
        else
        {
            image.color = UnlockColor();
        }

    }
    public Transform Left => left;
    public Transform Right => right;
    public Transform Up => up;
    public Transform Down => down;

    private Color LockColor()
    {
        return new Color32(113, 113, 113, 255);
    }
    private Color UnlockColor()
    {
        return Color.white;
    }

    public void Unlock()
    {
        skill.Unlock(true);
        image.color = UnlockColor();
        SkillManager.instance.SetSkillSlot(skill);

        if (!skill.IsActiveSkill)
        {
            skill.Activate();
        }
    }

}
