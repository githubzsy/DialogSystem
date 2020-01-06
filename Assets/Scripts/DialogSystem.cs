using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")] 
    public Text TextLabel;
    public Image FaceImage;

    [Header("文本文件")]
    public TextAsset TextFile;
    public float TextSpeed;

    [Header("头像")] 
    public Sprite Face01;

    public Sprite Face02;

    private List<string> m_Texts=new List<string>();

    private int m_Index;

    /// <summary>
    /// 文字是否逐字输出完成了
    /// </summary>
    private bool m_TextFinished;

    /// <summary>
    /// 是否取消逐字输出
    /// </summary>
    private bool m_CancelTyping;

    void Awake()
    {
        GetTextFromFile(TextFile);
    }

    private void OnEnable()
    {
        TextLabel.text = m_Texts[m_Index];
        StartCoroutine(SetTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && m_Index == m_Texts.Count)
        {
            gameObject.SetActive(false);
            m_Index = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (m_TextFinished && !m_CancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!m_TextFinished)
            {
                m_CancelTyping = !m_CancelTyping;
            }
        }
    }

    /// <summary>
    /// 从文件中读取文本
    /// </summary>
    /// <param name="file"></param>
    void GetTextFromFile(TextAsset file)
    {
        m_Texts.Clear();
        m_Index = 0;
        m_Texts = file.text.Split('\n').ToList();
    }

    /// <summary>
    /// 逐字输出文本
    /// </summary>
    /// <returns></returns>
    IEnumerator SetTextUI()
    {
        m_TextFinished = false;
        TextLabel.text = string.Empty;

        switch (m_Texts[m_Index])
        {
            case "A":
                FaceImage.sprite = Face01;
                m_Index++;
                break;
            case "B":
                FaceImage.sprite = Face02;
                m_Index++;
                break;
        }

        int letterIndex = 0;
        while (!m_CancelTyping && letterIndex < m_Texts[m_Index].Length - 1)
        {
            TextLabel.text += m_Texts[m_Index][letterIndex];
            letterIndex++;
            yield return new WaitForSeconds(TextSpeed);
        }

        TextLabel.text = m_Texts[m_Index];
        m_CancelTyping = false;

        m_Index++;
        m_TextFinished = true;
    }
}
