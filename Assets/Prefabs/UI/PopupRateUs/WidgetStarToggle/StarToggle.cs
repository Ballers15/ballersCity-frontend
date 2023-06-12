using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarToggle : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private Toggle m_toggle;
    
    public Image image => this.m_image;
    public Toggle toggle => this.m_toggle;
}
